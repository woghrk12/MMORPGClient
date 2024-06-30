using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public class Pool
    {
        #region Variables

        private Stack<Poolable> poolStack = new();

        #endregion Variables

        #region Properties

        public Transform Root { private set; get; }

        public GameObject Original { private set; get; }

        #endregion Properties

        #region Constructor

        public Pool(GameObject original, int count = 5)
        {
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            Original = original;

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        #endregion Constructor
        
        #region Methods

        private Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);

            go.name = Original.name;

            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolObj)
        {
            if (ReferenceEquals(poolObj, null)) return;

            poolObj.transform.parent = Root;
            poolObj.gameObject.SetActive(false);
            poolObj.IsUsing = false;

            poolStack.Push(poolObj);
        }

        public Poolable Pop(Transform parent)
        {
            if (!poolStack.TryPop(out Poolable poolObj))
            {
                poolObj = Create();
            }

            poolObj.gameObject.SetActive(true);
            poolObj.transform.parent = parent;
            poolObj.IsUsing = true;

            return poolObj;
        }

        #endregion Methods
    }

    #region Variables

    private Transform root = null;

    private Dictionary<string, Pool> poolDictionary = new();

    #endregion Variables

    #region Methods

    public void Init()
    {
        if (ReferenceEquals(root, null))
        {
            root = new GameObject { name = "@Pool" }.transform;

            Object.DontDestroyOnLoad(root);
        }
    }

    public Pool CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new(original, count);

        pool.Root.parent = root;

        poolDictionary.Add(original.name, pool);

        return pool;
    }

    public void Push(Poolable poolObj)
    {
        string name = poolObj.gameObject.name;

        if (!poolDictionary.ContainsKey(name))
        {
            GameObject.Destroy(poolObj.gameObject);
            return;
        }

        poolDictionary[name].Push(poolObj);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (!poolDictionary.TryGetValue(original.name, out Pool pool))
        {
            pool = CreatePool(original);
        }

        return pool.Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (!poolDictionary.ContainsKey(name)) return null;

        return poolDictionary[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in root)
        {
            GameObject.Destroy(child.gameObject);
        }

        poolDictionary.Clear();
    }

    #endregion Methods
}
