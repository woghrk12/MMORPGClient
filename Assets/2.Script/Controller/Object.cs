using Google.Protobuf.Protocol;
using System.Collections;
using UnityEngine;

namespace MMORPG
{
    public abstract class Object : MonoBehaviour
    {
        #region Variables

        private SpriteRenderer spriteRenderer = null;

        private EMoveDirection moveDirection = EMoveDirection.None;
        private EMoveDirection facingDirection = EMoveDirection.Right;

        #endregion Variables

        #region Properties

        public int ID { set; get; } = -1;

        public string Name { set; get; } = string.Empty;

        public Vector3Int Position { set; get; } = Vector3Int.zero;

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

        public int MoveSpeed { set; get; } = 0;

        #endregion Properties

        #region Unity Events

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        #endregion Unity Events

        #region Methods

        public virtual void OnDamaged()
        {
            StartCoroutine(OnDamagedCo());
        }

        private IEnumerator OnDamagedCo()
        {
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.2f);

            spriteRenderer.color = Color.white;
        }

        #endregion Methods
    }
}