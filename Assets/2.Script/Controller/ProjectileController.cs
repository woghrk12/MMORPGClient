using Google.Protobuf.Protocol;
using UnityEngine;

public class ProjectileController : CreatureController
{
    #region Unity Events

    protected override void Start()
    {
        base.Start();

        SetState(ECreatureState.Move);
    }

    #endregion Unity Events

    #region Methods

    public void SetProjectile(EMoveDirection moveDirection)
    {
        MoveDirection = moveDirection;

        switch (MoveDirection)
        {
            case EMoveDirection.Up:
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;

            case EMoveDirection.Down:
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;

            case EMoveDirection.Left:
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;

            case EMoveDirection.Right:
                transform.localScale = new Vector3(1f, 1f, 1f);
                break;
        }
    }

    #endregion Methods
}
