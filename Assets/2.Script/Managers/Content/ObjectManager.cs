using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    #region Variables

    List<GameObject> objectList = new();

    #endregion Variables

    #region Methods

    public void Add(GameObject go)
    {
        objectList.Add(go);
    }

    public void Remove(GameObject go)
    {
        objectList.Remove(go);
    }

    public void Clear()
    {
        objectList.Clear();
    }

    public GameObject Find(Vector3Int cellPos)
    {
        foreach (GameObject go in objectList)
        {
            if (go.TryGetComponent(out CreatureController controller) == false) continue;
            if (controller.CellPos != cellPos) continue;
         
            return go;
        }

        return null;
    }
    
    #endregion Methods
}