using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class MoveState : Creature.MoveState<MonsterController>
    {
        #region Methods

        protected override void SetNextPos()
        {
            Vector3Int cellPos = controller.CellPos;
            Vector3Int destCellPos = controller.DestCellPos;
            PlayerController target = controller.Target;

            if (ReferenceEquals(target, null) == false)
            {
                destCellPos = target.CellPos;

                if (Utility.CalculateDistance(destCellPos, cellPos) <= controller.AttackRange)
                {
                    controller.SetState(ECreatureState.ATTACK);
                    return;
                }
            }

            if (cellPos == destCellPos)
            {
                controller.SetState(ECreatureState.IDLE);
                return;
            }

            if (Managers.Map.FindPath(cellPos, destCellPos, out List<Vector3Int> path) == false)
            {
                controller.SetState(ECreatureState.IDLE);
                return;
            }

            Vector3Int nextPos = path[1];
            Vector3Int moveVector = nextPos - cellPos;

            controller.MoveDirection = Utility.GetDirFromVec(moveVector);

            if (Managers.Map.CheckCanMove(nextPos) == true && ReferenceEquals(Managers.Obj.Find(nextPos), null) == true)
            {
                controller.CellPos = nextPos;
            }
            else
            {
                controller.SetState(ECreatureState.IDLE);
            }
        }

        #endregion Methods
    }
}