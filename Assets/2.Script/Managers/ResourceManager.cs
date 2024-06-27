using UnityEngine;

public class ResourceManager
{
    #region Methods

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if (ReferenceEquals(prefab, null))
        {
            Debug.LogWarning($"Failed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    public void Destory(GameObject go)
    {
        if (ReferenceEquals(go, null)) return;

        Object.Destroy(go);
    }

    #endregion Methods
}
