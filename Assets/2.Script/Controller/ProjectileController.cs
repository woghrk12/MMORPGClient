using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : CreatureController
{
    #region Unity Events

    protected override void Start()
    {
        base.Start();

        switch (MoveDirection)
        {
            case EMoveDirection.UP:
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;

            case EMoveDirection.DOWN:
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;

            case EMoveDirection.LEFT:
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;

            case EMoveDirection.RIGHT:
                transform.localScale = new Vector3(1f, 1f, 1f);
                break;
        }
    }

    #endregion Unity Events

    #region Methods

    protected override void UpdateMoveState()
    {
        if (isActing == true) return;
        if (MoveDirection == EMoveDirection.NONE) return;

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
                break;

            case EMoveDirection.RIGHT:
                cellPos += Vector3Int.right;
                break;
        }

        State = ECreatureState.MOVE;

        if (Managers.Map.CheckCanMove(cellPos) == true)
        {
            GameObject go = Managers.Obj.Find(cellPos);

            if (ReferenceEquals(go, null) == true)
            {
                CellPos = cellPos;
                isActing = true;
            }
            else
            {
                Debug.Log(go.name);
                Managers.Resource.Destory(gameObject);
            }
        }
        else
        {
            Managers.Resource.Destory(gameObject);
        }
    }

    protected override void UpdateAnimation() { }

    #endregion Methods
}
