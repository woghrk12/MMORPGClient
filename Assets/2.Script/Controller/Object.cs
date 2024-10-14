using Google.Protobuf.Protocol;
using System;
using System.Collections;
using UnityEngine;

namespace MMORPG
{
    public abstract class Object : MonoBehaviour
    {
        #region Variables

        private EMoveDirection moveDirection = EMoveDirection.None;
        private EMoveDirection facingDirection = EMoveDirection.Right;

        private ObjectStat stat = new();

        private event Action<int, int> curHpModified = null;

        #endregion Variables

        #region Properties

        public SpriteRenderer SpriteRenderer { private set; get; }

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
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        #endregion Unity Events

        #region Methods

        #region Events

        public virtual void OnDamaged(int remainHp, int damage)
        {
            CurHP = remainHp;

            StartCoroutine(OnDamagedCo(damage));
        }

        public virtual void OnDead(Object attacker)
        {
            Debug.Log($"{ID} object dies by {attacker.ID} object!");
        }

        public virtual void OnRevive(Vector3Int revivePos)
        {
            MoveDirection = EMoveDirection.None;
            CurHP = MaxHP;

            Managers.Map.MoveObject(ID, Position, revivePos);
            Position = revivePos;

            transform.position = new Vector3(Position.x, Position.y) + new Vector3(0.5f, 0.5f);
        }

        #endregion Events

        private IEnumerator OnDamagedCo(int damage)
        {
            SpriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.2f);

            SpriteRenderer.color = Color.white;
        }

        #endregion Methods
    }
}