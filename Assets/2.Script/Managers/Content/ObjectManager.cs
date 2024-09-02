using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    #region Variables

    private Dictionary<int, GameObject> objectDict = new();

    #endregion Variables

    #region Properties

    public LocalPlayerController LocalPlayer { set; get; } = null;

    #endregion Properties

    #region Methods

    public void AddPlayer(CreatureInfo info, bool isMine = false)
    {
        GameObject go = Managers.Resource.Instantiate("Creature/" + (isMine ? "LocalPlayer" : "Player"));
        go.name = info.Name;
        objectDict.Add(info.CreatureID, go);

        PlayerController controller = go.GetComponent<PlayerController>();
        controller.ID = info.CreatureID;
        controller.Name = info.Name;
        controller.CurState = info.CurState;
        controller.CellPos = new Vector3Int(info.CellPosX, info.CellPosY, 0);
        controller.FacingDirection = info.FacingDirection;
        controller.MoveSpeed = info.MoveSpeed;

        controller.transform.position = new Vector3(controller.CellPos.x, controller.CellPos.y, 0f) + new Vector3(0.5f, 0.5f, 0f);

        if (isMine)
        {
            LocalPlayer = controller as LocalPlayerController;
        }
    }

    public void Add(int id, GameObject go)
    {
        objectDict.Add(id, go);
    }

    public void Remove(int id)
    {
        if (objectDict.TryGetValue(id, out GameObject go) == false) return;
            
        objectDict.Remove(id);
        Managers.Resource.Destory(go);
    }

    public void Clear()
    {
        objectDict.Clear();
    }

    public GameObject Find(int creatureID)
    {
        return objectDict.TryGetValue(creatureID, out GameObject gameObject) ? gameObject : null;
    }

    public bool TryFind(int creatureID, out GameObject creature)
    {
        creature = null;

        if (objectDict.TryGetValue(creatureID, out GameObject gameobject) == false) return false;

        creature = gameobject;
        return true;
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
