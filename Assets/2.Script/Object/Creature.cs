using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MMORPG.Object
{
    #region Variables

    protected Dictionary<ECreatureState, CreatureState> stateDictionary = new();
    protected CreatureState curState = null;

    private EMoveDirection moveDirection = EMoveDirection.None;
    private EMoveDirection facingDirection = EMoveDirection.Right;  

    private CreatureStat stat = new();
    private event Action<int> levelModified = null;
    private event Action<int> curHpModified = null;
    private event Action<int> maxHpModified = null;
    private event Action<int> attackPowerModified = null;
    private event Action creatureDead = null;

    #endregion Variables

    #region Properties

    public ECreatureState CurState
    {
        set 
        {
            if (stateDictionary.ContainsKey(value) == false) return;

            if (ReferenceEquals(curState, null) == false)
            {
                if (curState.StateID == value) return;

                curState.OnExit();
            }

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

    public event Action CreatureDead { add { creatureDead += value; } remove { creatureDead -= value; } }

    public Data.AttackStat AttackStat { set; get; } = null;

    #endregion Properties

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        stateDictionary.Add(ECreatureState.Idle, gameObject.AddComponent<IdleState>());
        stateDictionary.Add(ECreatureState.Move, gameObject.AddComponent<MoveState>());
        stateDictionary.Add(ECreatureState.Attack, gameObject.AddComponent<AttackState>());
        stateDictionary.Add(ECreatureState.Dead, gameObject.AddComponent<DeadState>());
    }

    protected override void Update()
    {
        base.Update();

        curState?.OnUpdate();
    }

    #endregion Unity Events

    #region Methods

    public sealed override void Init(ObjectInfo info)
    {
        HpBar hpBar = Managers.Resource.Instantiate("UI/HpBar").GetComponent<HpBar>();
        hpBar.InitHpBar(transform, 0.65f);

        MaxHpModified += hpBar.SetMaxHp;
        CurHpModified += hpBar.SetCurHp;
        ObjectDestroyed += () => Destroy(hpBar.gameObject);

        ID = info.ObjectID;
        Name = info.Name;
        Position = new Vector3Int(info.PosX, info.PosY);
        IsCollidable = info.IsCollidable;

        CurState = info.CreatureInfo.CurState;
        MoveDirection = info.CreatureInfo.MoveDirection;
        FacingDirection = info.CreatureInfo.FacingDirection;
        MoveSpeed = info.CreatureInfo.MoveSpeed;

        Level = info.CreatureInfo.Stat.Level;
        MaxHp = info.CreatureInfo.Stat.MaxHP;
        CurHp = info.CreatureInfo.Stat.CurHP;
        AttackPower = info.CreatureInfo.Stat.AttackPower;
    }

    public virtual void OnDamaged(int remaindHp, int damage)
    {
        CurHp = remaindHp;

        StartCoroutine(OnDamagedCo());
    }

    public virtual void OnDead(int attackerID)
    {
        Debug.Log($"{ID} object dies by {attackerID} object!");

        CurState = ECreatureState.Dead;

        creatureDead?.Invoke();
    }

    private IEnumerator OnDamagedCo()
    {
        SpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        SpriteRenderer.color = Color.white;
    }

    #endregion Methods
}
