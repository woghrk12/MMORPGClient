using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MMORPG.Object
{
    #region Variables

    private Dictionary<ECreatureState, IBaseState<Creature>> stateDictionary = new();
    private IBaseState<Creature> curState = null;

    private EMoveDirection moveDirection = EMoveDirection.None;
    private EMoveDirection facingDirection = EMoveDirection.Right;  

    private CreatureStat stat = new();
    private event Action<int> levelModified = null;
    private event Action<int> curHpModified = null;
    private event Action<int> maxHpModified = null;
    private event Action<int> attackPowerModified = null;

    #endregion Variables

    #region Properties

    public ECreatureState CurState 
    { 
        set
        {
            if (ReferenceEquals(curState, null) == false)
            {
                if (curState.StateID == value) return;

                curState.OnExit();
                curState = null;
            }

            if (stateDictionary.ContainsKey(value) == false) return;

            curState = stateDictionary[value];
            curState.OnEnter();
        }
        get => ReferenceEquals(curState, null) == false ? curState.StateID : ECreatureState.Idle;
    }

    public EMoveDirection MoveDirection
    {
        set
        {
            if (moveDirection == value) return;

            moveDirection = value;

            if (moveDirection != EMoveDirection.None)
            {
                FacingDirection = moveDirection;
            }
        }
        get => moveDirection;
    }

    public EMoveDirection FacingDirection
    {
        set
        {
            if (facingDirection == value) return;

            facingDirection = value;

            if (facingDirection == EMoveDirection.Left)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            if (facingDirection == EMoveDirection.Right)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        get => facingDirection;
    }

    public int MoveSpeed { set; get; } = 5;

    public int Level
    {
        set
        {
            stat.Level = Mathf.Max(value, 1);
            levelModified?.Invoke(stat.Level);
        }
        get => stat.Level;
    }

    public event Action<int> LevelModified { add { levelModified += value; } remove { levelModified -= value; } }

    public int CurHp
    {
        set
        {
            stat.CurHP = Mathf.Clamp(value, 0, stat.MaxHP);
            curHpModified?.Invoke(stat.CurHP);
        }
        get => stat.CurHP;
    }

    public event Action<int> CurHpModified { add { curHpModified += value; } remove { curHpModified -= value; } }

    public int MaxHp
    {
        set
        {
            stat.MaxHP = Mathf.Max(value, 1);
            maxHpModified?.Invoke(stat.MaxHP);
        }
        get => stat.MaxHP;
    }

    public event Action<int> MaxHpModified { add { maxHpModified += value; } remove { maxHpModified -= value; } }

    public int AttackPower
    {
        set
        {
            stat.AttackPower = Mathf.Max(value, 0);
            attackPowerModified?.Invoke(stat.AttackPower);
        }
        get => stat.AttackPower;
    }

    public event Action<int> AttackPowerModified { add { attackPowerModified += value; } remove { attackPowerModified -= value; } }

    #endregion Properties

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        AddState(ECreatureState.Idle, new BaseIdleState<Creature>(this));
        AddState(ECreatureState.Move, new BaseMoveState<Creature>(this));
        AddState(ECreatureState.Attack, new BaseAttackState<Creature>(this));
        AddState(ECreatureState.Dead, new BaseDeadState<Creature>(this));
    }

    protected override void Update()
    {
        base.Update();

        curState?.OnUpdate();
    }

    private void FixedUpdate()
    {
        Vector3 destPos = new Vector3(Position.x, Position.y) + new Vector3(0.5f, 0.5f);
        Vector3 moveDir = destPos - transform.position;

        if (moveDir.sqrMagnitude < (MoveSpeed * Time.deltaTime) * (MoveSpeed * Time.deltaTime))
        {
            transform.position = destPos;
        }
        else
        {
            transform.position += MoveSpeed * Time.deltaTime * moveDir.normalized;
        }
    }

    #endregion Unity Events

    #region Methods

    public override void Init(ObjectInfo info)
    {
        base.Init(info);

        HpBar hpBar = Managers.Resource.Instantiate("UI/HpBar").GetComponent<HpBar>();
        hpBar.InitHpBar(transform, 0.65f);

        MaxHpModified += hpBar.SetMaxHp;
        CurHpModified += hpBar.SetCurHp;
        ObjectDestroyed += () => Destroy(hpBar.gameObject);

        CurState = info.CreatureInfo.CurState;
        MoveDirection = info.CreatureInfo.MoveDirection;
        FacingDirection = info.CreatureInfo.FacingDirection;
        MoveSpeed = info.CreatureInfo.MoveSpeed;

        Level = info.CreatureInfo.Stat.Level;
        MaxHp = info.CreatureInfo.Stat.MaxHP;
        CurHp = info.CreatureInfo.Stat.CurHP;
        AttackPower = info.CreatureInfo.Stat.AttackPower;
    }

    public void AddState(ECreatureState stateID, IBaseState<Creature> state)
    {
        if(stateDictionary.ContainsKey(stateID) == true)
        {
             stateDictionary.Remove(stateID);
        }

        stateDictionary.Add(stateID, state);
    }

    public virtual void Move(Vector2Int targetPos, EMoveDirection moveDirection)
    {
        Managers.Map.MoveObject(this, targetPos);
        MoveDirection = moveDirection;

        CachedAnimator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, MoveDirection != EMoveDirection.None);

        CurState = MoveDirection != EMoveDirection.None ? ECreatureState.Move : ECreatureState.Idle;
    }

    public virtual void Attack(int attackID, EMoveDirection facingDirection)
    {
        if (Managers.Data.AttackStatDictionary.TryGetValue(attackID, out Data.AttackStat attackStat) == false) return;

        FacingDirection = facingDirection;
        CachedAnimator.SetTrigger(attackStat.AnimationKey);

        CurState = ECreatureState.Attack;
    }

    public virtual void OnDamaged(int remaindHp, int damage)
    {
        CurHp = remaindHp;

        StartCoroutine(OnDamagedCo());
    }

    public virtual void OnDead(int attackerID)
    {
        Debug.Log($"{ID} object dies by {attackerID} object!");

        GameObject dieEffect = Managers.Resource.Instantiate("Effect/DieEffect");
        dieEffect.transform.position = CachedTransform.position;
        Managers.Push(Managers.Resource.Destory, dieEffect, 1000);

        CurState = ECreatureState.Dead;
        IsCollidable = false;
        CachedSpriteRenderer.enabled = false;
    }

    private IEnumerator OnDamagedCo()
    {
        CachedSpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        CachedSpriteRenderer.color = Color.white;
    }

    #endregion Methods
}
