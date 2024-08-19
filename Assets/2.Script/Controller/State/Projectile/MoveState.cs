using Google.Protobuf.Protocol;
using UnityEngine;

namespace Projectile
{
    public class MoveState : Creature.MoveState<ProjectileController>
    {
        #region Methods

        protected override void SetNextPos()
        {
            Vector3Int cellPos = controller.CellPos;

            switch (controller.MoveDirection)
            {
                case EMoveDirection.Up:
                    cellPos += Vector3Int.up;
                    break;

                case EMoveDirection.Down:
                    cellPos += Vector3Int.down;
                    break;

                case EMoveDirection.Left:
                    cellPos += Vector3Int.left;
                    break;

                case EMoveDirection.Right:
                    cellPos += Vector3Int.right;
                    break;
            }

            if (Managers.Map.CheckCanMove(cellPos) == true)
            {
                GameObject go = Managers.Obj.Find(cellPos);

                if (ReferenceEquals(go, null) == true)
                {
                    controller.CellPos = cellPos;
                }
                else
                {
                    if (go.TryGetComponent(out CreatureController controller) == true)
                    {
                        controller.OnDamaged();
                    }

                    Managers.Resource.Destory(this.controller.gameObject);
                }
            }
            else
            {
                Managers.Resource.Destory(this.controller.gameObject);
            }
        }

        #endregion Methods
    }
}
