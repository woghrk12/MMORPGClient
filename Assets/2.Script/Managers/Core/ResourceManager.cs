using UnityEngine;

public class ResourceManager
{
    #region Methods

    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            int index = path.LastIndexOf('/');

            if (index >= 0)
            {
                path = path.Substring(index + 1);
            }

            GameObject go = Managers.Pool.GetOriginal(path);

            if (!ReferenceEquals(go, null))
            {
                return go as T;
            }
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if (ReferenceEquals(original, null))
        {
            Debug.LogWarning($"Failed to load prefab : {path}");
            return null;
        }

        if (original.TryGetComponent(out Poolable poolObj))
        {
            return Managers.Pool.Pop(original, parent).gameObject;
        }

        GameObject go = Object.Instantiate(original, parent);
        int index = go.name.IndexOf("(Clone)");
        
        if (index > 0)
        {
            go.name = go.name.Substring(0, index);
        }

        return go;
    }

    public void Destory(GameObject go)
    {
        if (ReferenceEquals(go, null)) return;

        if (go.TryGetComponent(out Poolable poolObj))
        {
            Managers.Pool.Push(poolObj);
            return;
        }

        Object.Destroy(go);
    }

    #endregion Methods
}
