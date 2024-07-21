using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
    #region Variables

    private Coroutine coPatrol = null;
    private Vector3Int destCellPos = Vector3Int.zero;

    #endregion Variables

    #region Methods

    #region States

    protected override void UpdateIdleState()
    {
        if (ReferenceEquals(coPatrol, null) == false) return;

        coPatrol = StartCoroutine(PatrolCo());
    }

    #endregion States

    protected override void MoveToNextPos()
    {
        Vector3Int moveVector = destCellPos - CellPos;

        if (moveVector.x != 0)
        {
            MoveDirection = moveVector.x > 0 ? EMoveDirection.RIGHT : EMoveDirection.LEFT;
        }
        else if (moveVector.y != 0)
        {
            MoveDirection = moveVector.y > 0 ? EMoveDirection.UP : EMoveDirection.DOWN;
        }
        else
        {
            MoveDirection = EMoveDirection.NONE;
            State = ECreatureState.IDLE;
            return;
        }

        Vector3Int cellPos = CellPos;

        switch (MoveDirection)
        {
            case EMoveDirection.UP:
                cellPos += Vector3Int.up;
                break;

            case EMoveDirection.DOWN:
                cellPos += Vector3Int.down;
                break;

            case EMoveDirection.LEFT:
                cellPos += Vector3Int.left;
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;

            case EMoveDirection.RIGHT:
                cellPos += Vector3Int.right;
                transform.localScale = new Vector3(1f, 1f, 1f);
                break;
        }

        if (Managers.Map.CheckCanMove(cellPos) == true && ReferenceEquals(Managers.Obj.Find(cellPos), null) == true)
        {
            CellPos = cellPos;
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
            coPatrol = null;
            yield break;
        }

        if (ReferenceEquals(coPatrol, null) == false)
        {
            StopCoroutine(coPatrol);
            coPatrol = null;
        }

        State = ECreatureState.IDLE;
    }

    #region Events

    public override void OnDamaged()
    {
        GameObject effect = Managers.Resource.Instantiate("Effect/DieEffect");
        effect.transform.position = transform.position + new Vector3(0f, 0.2f, 0f);
        GameObject.Destroy(effect, 0.5f);

        Debug.Log(gameObject.name);
        Managers.Obj.Remove(gameObject);
        Managers.Resource.Destory(gameObject);
    }

    #endregion Events

    #endregion Methods
}
