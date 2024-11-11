using Google.Protobuf.Protocol;
using System;
using UnityEngine;

namespace MMORPG
{
    public abstract class Object : MonoBehaviour
    {
        #region Variables

        private event Action updated = null;
        private event Action objectDestroyed = null;

        #endregion Variables

        #region Properties

        public SpriteRenderer SpriteRenderer { private set; get; }

        public int ID { set; get; } = -1;

        public string Name { set; get; } = string.Empty;

        public abstract EGameObjectType GameObjectType { get; }

        public Vector2Int Position { set; get; } = Vector2Int.zero;

        public bool IsCollidable { set; get; }

        public event Action Updated { add { updated += value; } remove { updated -= value; } }

        public event Action ObjectDestroyed { add { objectDestroyed += value; } remove { objectDestroyed -= value; } }

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

        protected virtual void OnDestroy()
        {
            objectDestroyed?.Invoke();
        }

        #endregion Unity Events

        #region Methods

        public abstract void Init(ObjectInfo info);

        #endregion Methods
    }
}