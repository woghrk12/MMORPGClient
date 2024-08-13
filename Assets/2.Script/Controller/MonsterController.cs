using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
    #region Variables

    private Coroutine coPatrol = null;
    private Coroutine coSearch = null;
    private Coroutine coAttack = null;

    private Vector3Int destCellPos = Vector3Int.zero;

    private PlayerController target = null;
    
    [SerializeField] private int searchRange = 5;
    [SerializeField] private int attackRange = 1;

    #endregion Variables

    #region Properties

    public override ECreatureState State
    {
        protected set
        {
            if (state == value) return;

            base.State = value;

            if (ReferenceEquals(coPatrol, null) == false)
            {
                StopCoroutine(coPatrol);
                coPatrol = null;
            }

            if (ReferenceEquals(coSearch, null) == false)
            {
                StopCoroutine(coSearch);
                coSearch = null;
            }
        }
        get => state;
    }

    #endregion Properties

    #region Methods

    #region States

    protected override void UpdateIdleState()
    {
        if (ReferenceEquals(coPatrol, null) == true)
        {
            coPatrol = StartCoroutine(PatrolCo());
        }

        if (ReferenceEquals(coSearch, null) == true)
        {
            coSearch = StartCoroutine(SearchCo());
        }
    }

    #endregion States

    protected override void MoveToNextPos()
    {
        Vector3Int destCellPos = this.destCellPos;
        if (ReferenceEquals(target, null) == false)
        {
            if (Utility.CalculateDistance(target.CellPos, CellPos) <= searchRange)
            {
                destCellPos = target.CellPos;

                if (Utility.CalculateDistance(destCellPos, CellPos) <= attackRange)
                {
                    State = ECreatureState.ATTACK;
                    coAttack = StartCoroutine(AttackCo());
                    return;
                }
            }
            else
            {
                target = null;
                State = ECreatureState.IDLE;
                return;
            }
        }

        if (CellPos == destCellPos)
        {
            target = null;
            State = ECreatureState.IDLE;
            return;
        }

        if (Managers.Map.FindPath(CellPos, destCellPos, out List<Vector3Int> path) == false)
        {
            State = ECreatureState.IDLE;
            return;
        }

        Vector3Int nextPos = path[1];
        Vector3Int moveVector = nextPos - CellPos;

        MoveDirection = GetDirFromVec(moveVector);

        if (moveVector.x != 0)
        {
            MoveDirection = moveVector.x > 0 ? EMoveDirection.Right : EMoveDirection.Left;
            LastMoveDirection = MoveDirection;
            transform.localScale = new Vector3(moveVector.x > 0 ? 1f : -1f, 1f, 1f);
        }
        else if (moveVector.y != 0)
        {
            MoveDirection = moveVector.y > 0 ? EMoveDirection.Up : EMoveDirection.Down;
            LastMoveDirection = MoveDirection;
        }
        else
        {
            MoveDirection = EMoveDirection.None;
            State = ECreatureState.IDLE;
            return;
        }

        if (Managers.Map.CheckCanMove(nextPos) == true && ReferenceEquals(Managers.Obj.Find(nextPos), null) == true)
        {
            CellPos = nextPos;
        }
        else
        {
            State = ECreatureState.IDLE;
        }
    }

    private IEnumerator PatrolCo()
    {
        yield return new WaitForSeconds(Random.Range(1, 4));

        for (int i = 0; i < 10; i++)
        {
            Vector3Int randPos = CellPos + new Vector3Int(Random.Range(-5, 5), Random.Range(-5, 5), 0);

            if (Managers.Map.CheckCanMove(randPos) == false) continue;
            if (ReferenceEquals(Managers.Obj.Find(randPos), null) == false) continue;

            destCellPos = randPos;
            State = ECreatureState.MOVE;
            yield break;
        }

        State = ECreatureState.IDLE;
    }

    private IEnumerator SearchCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // If the target player object is null
            if (ReferenceEquals(target, null) == true)
            {
                // Find a player object within range
                target = Managers.Obj.Find((go) =>
                {
                    if (go.TryGetComponent(out PlayerController controller) == false) return false;
                    if ((controller.CellPos - CellPos).sqrMagnitude > searchRange * searchRange) return false;

                    return true;
                })?.GetComponent<PlayerController>();

                // Nevertheless, if the object does not exist, proceed to the next loop
                if (ReferenceEquals(target, null) == true) continue;
            }
        }
    }

    private IEnumerator AttackCo()
    {
        if (ReferenceEquals(target, null) == false)
        {
            target.OnDamaged();
        }

        yield return new WaitForSeconds(0.5f);

        State = ECreatureState.MOVE;
    }

    #region Events

    public override void OnDamaged()
    {
        GameObject effect = Managers.Resource.Instantiate("Effect/DieEffect");
        effect.transform.position = transform.position + new Vector3(0f, 0.2f, 0f);
        GameObject.Destroy(effect, 0.5f);

        Debug.Log(gameObject.name);
        Managers.Resource.Destory(gameObject);
    }

    #endregion Events

    #endregion Methods
}
