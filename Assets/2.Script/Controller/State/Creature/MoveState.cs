using Google.Protobuf.Protocol;
using UnityEngine;

namespace Creature
{
    public class MoveState<T> : CreatureState where T : CreatureController
    {
        #region Variables

        [SerializeField] protected float moveSpeed = 0f;

        protected T controller = null;

        #endregion Variables

        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.MOVE;

        #endregion Properties

        #region Unity Events

        protected override void Awake()
        {
            base.Awake();

            controller = GetComponent<T>();
        }

        #endregion Unity Events

        #region Methods

        protected virtual void MoveToNextPos()
        {
            Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(controller.CellPos) + new Vector3(0.5f, 0.5f, 0f);
            Vector3 moveVector = destPos - transform.position;

            if (moveVector.sqrMagnitude < (moveSpeed * Time.deltaTime) * (moveSpeed * Time.deltaTime))
            {
                transform.position = destPos;
                SetNextPos();
            }
            else
            {
                transform.position += moveSpeed * Time.deltaTime * moveVector.normalized;
            }
        }

        protected virtual void SetNextPos()
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

                default:
                    controller.SetState(ECreatureState.IDLE);
                    return;
            }

            if (Managers.Map.CheckCanMove(cellPos) == true && ReferenceEquals(Managers.Obj.Find(cellPos), null) == true)
            {
                controller.CellPos = cellPos;
            }
        }

        #region Events

        public override void OnStart()
        {
            animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, true);
        }

        public override void OnFixedUpdate()
        {
            MoveToNextPos();
        }

        public override void OnExit()
        {
            animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, false);
        }

        #endregion Events

        #endregion Methods
    }
}