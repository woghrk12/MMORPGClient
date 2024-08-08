using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    #region Variables

    private Dictionary<int, GameObject> objectDict = new();

    #endregion Variables

    #region Methods

    public void Add(int id, GameObject go)
    {
        objectDict.Add(id, go);
    }

    public void Remove(int id)
    {
        objectDict.Remove(id);
    }

    public void Clear()
    {
        objectDict.Clear();
    }

    public GameObject Find(Vector3Int cellPos)
    {
        foreach (GameObject go in objectDict.Values)
        {
            if (go.TryGetComponent(out CreatureController controller) == false) continue;
            if (controller.CellPos != cellPos) continue;
         
            return go;
        }

        return null;
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        if (ReferenceEquals(condition, null) == true) return null;

        foreach (GameObject go in objectDict.Values)
        {
            if (condition.Invoke(go) == false) continue;

            return go;
        }

        return null;
    }
    
    #endregion Methods
}
