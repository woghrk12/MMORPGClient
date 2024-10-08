using Google.Protobuf.Protocol;
using System;
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

        private ObjectStat stat = new();

        private event Action<int, int> curHpModified = null;

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

        public bool IsCollidable { set; get; }

        public int MaxHP 
        {
            set 
            { 
                stat.MaxHP = value; 
            } 
            get => stat.MaxHP; 
        }

        public int CurHP 
        { 
            set 
            { 
                stat.CurHP = value;

                curHpModified?.Invoke(stat.CurHP, stat.MaxHP);
            } 
            get => stat.CurHP; 
        }

        public event Action<int, int> CurHpModified
        {
            add
            {
                curHpModified += value;
            }
            remove
            {
                curHpModified -= value;
            }
        }

        public int AttackPower 
        { 
            set 
            { 
                stat.AttackPower = value; 
            }
            get => stat.AttackPower; 
        }

        #endregion Properties

        #region Unity Events

        protected virtual void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        #endregion Unity Events

        #region Methods

        public virtual void OnDamaged(int remainHp, int damage)
        {
            CurHP = remainHp;

            StartCoroutine(OnDamagedCo(damage));
        }

        private IEnumerator OnDamagedCo(int damage)
        {
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.2f);

            spriteRenderer.color = Color.white;
        }

        #endregion Methods
    }
}