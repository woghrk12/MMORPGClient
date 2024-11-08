using Google.Protobuf.Protocol;
using System;
using UnityEngine;

namespace MMORPG
{
    public abstract class Object : MonoBehaviour
    {
        #region Variables

        private event Action updated = null;

        #endregion Variables

        #region Properties

        public SpriteRenderer SpriteRenderer { private set; get; }

        public int ID { set; get; } = -1;

        public string Name { set; get; } = string.Empty;

        public abstract EGameObjectType GameObjectType { get; }

        public Vector3Int Position { set; get; } = Vector3Int.zero;

        public bool IsCollidable { set; get; }

        public event Action Updated { add { updated += value; } remove { updated -= value; } }

        #endregion Properties

        #region Unity Events

        protected virtual void Awake()
        {
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        protected virtual void Update()
        {
            updated?.Invoke();
        }

        #endregion Unity Events

        #region Methods

        public abstract void Init(ObjectInfo info);

        #endregion Methods
    }
}