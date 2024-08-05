using UnityEngine;

public class ProjectileController : CreatureController
{
    #region Unity Events

    protected override void Start()
    {
        base.Start();

        State = ECreatureState.MOVE;
    }

    #endregion Unity Events

    #region Methods

    public void SetProjectile(EMoveDirection moveDirection)
    {
        MoveDirection = moveDirection;

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

    protected override void UpdateAnimation() { }

    protected override void MoveToNextPos()
    {
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

        if (Managers.Map.CheckCanMove(cellPos) == true)
        {
            GameObject go = Managers.Obj.Find(cellPos);

            if (ReferenceEquals(go, null) == true)
            {
                CellPos = cellPos;
            }
            else
            {
                if (go.TryGetComponent(out CreatureController controller) == true)
                {
                    controller.OnDamaged();
                }

                Managers.Resource.Destory(gameObject);
            }
        }
        else
        {
            Managers.Resource.Destory(gameObject);
        }
    }

    #endregion Methods
}
