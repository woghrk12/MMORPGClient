using UnityEngine;

public class Utility 
{
    #region Methods

    /// <summary>
    /// Find a child object of a specified type within the given GameObject.
    /// </summary>
    /// <typeparam name="T">The type of the child object to find, which must be derived from UnityEngine.Object.</typeparam>
    /// <param name="go">The GameObject to search within.</param>
    /// <param name="name">The name of the child object to find. If null, the first object of type T is returned.</param>
    /// <param name="isRecursive">If true, the search will include all children and their descendants; otherwise, only direct children will be searched.</param>
    /// <returns>The found child object of type T, or null if no such object is found.</returns>
    public static T FindChild<T>(GameObject go, string name = null, bool isRecursive = false) where T : Object
    {
        if (ReferenceEquals(go, null)) return null;

        if (isRecursive)
        {
            T[] components = go.GetComponentsInChildren<T>();

            foreach (T component in components)
            {
                if (ReferenceEquals(component, go)) continue;
                if (!string.IsNullOrEmpty(name) && !component.name.Equals(name)) continue;

                return component;
            }
        }
        else
        {
            Transform transform = go.transform;

            foreach (Transform child in transform)
            {
                if (!child.TryGetComponent(out T component)) continue;
                if (!string.IsNullOrEmpty(name) && !component.name.Equals(name)) continue;

                return component;
            }
        }

        return null;
    }

    #endregion Methods
}
