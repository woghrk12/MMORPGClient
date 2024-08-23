using Google.Protobuf.Protocol;
using UnityEngine;

namespace Creature
{
    public class MoveState<T> : CreatureState where T : CreatureController
    {
        #region Variables

        [SerializeField] protected float moveSpeed = 0f;

        protected T controller = null;

        private bool isCompleted = false;

        #endregion Variables

        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Move;

        public bool IsCompleted
        {
            private set
            {
                if (isCompleted == value) return;

                isCompleted = value;

                if (isCompleted == true)
                {
                    NotifyMoveCompleted();
                }
            }
            get => isCompleted;
        }

        #endregion Properties

        #region Unity Events

        protected override void Awake()
        {
            base.Awake();

            controller = GetComponent<T>();
        }

        #endregion Unity Events

        #region Methods

        protected virtual void NotifyMoveCompleted() { }

        protected void MoveToNextPos()
        {
            if (IsCompleted) return;

            Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(controller.CellPos) + new Vector3(0.5f, 0.5f, 0f);
            Vector3 moveVector = destPos - transform.position;

            if (moveVector.sqrMagnitude < (moveSpeed * Time.deltaTime) * (moveSpeed * Time.deltaTime))
            {
                transform.position = destPos;
                IsCompleted = true;
            }
            else
            {
                transform.position += moveSpeed * Time.deltaTime * moveVector.normalized;
            }
        }

        #region Events

        public override void OnStart()
        {
            animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, true);

            IsCompleted = false;
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