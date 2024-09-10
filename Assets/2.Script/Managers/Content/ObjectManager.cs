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

    public LocalPlayer LocalPlayer { set; get; } = null;

    #endregion Properties

    #region Methods

    public void AddPlayer(CreatureInfo info, bool isMine = false)
    {
        if (isMine)
        {
            GameObject go = Managers.Resource.Instantiate("Creature/LocalPlayer");

            go.name = info.Name;
            objectDict.Add(info.CreatureID, go);

            LocalPlayer localPlayer = go.GetComponent<LocalPlayer>();

            localPlayer.ID = info.CreatureID;
            localPlayer.Name = info.Name;
            localPlayer.Position = new Vector3Int(info.PosX, info.PosY, 0);
            localPlayer.MoveDirection = info.FacingDirection;
            localPlayer.MoveSpeed = info.MoveSpeed;
            localPlayer.transform.position = new Vector3(localPlayer.Position.x, localPlayer.Position.y, 0f) + new Vector3(0.5f, 0.5f, 0f);

            localPlayer.SetState(info.CurState, EPlayerInput.NONE);

            LocalPlayer = localPlayer;
        }
        else
        { 
            GameObject go = Managers.Resource.Instantiate("Creature/Player");

            go.name = info.Name;
            objectDict.Add(info.CreatureID, go);

            RemoteCreature remoteCreature = go.GetComponent<RemoteCreature>();

            remoteCreature.ID = info.CreatureID;
            remoteCreature.Name = info.Name;
            remoteCreature.Position = new Vector3Int(info.PosX, info.PosY, 0);
            remoteCreature.MoveDirection = info.FacingDirection;
            remoteCreature.MoveSpeed = info.MoveSpeed;

            remoteCreature.SetState(info.CurState);
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
            if (go.TryGetComponent(out Creature controller) == false) continue;
            if (controller.Position != cellPos) continue;
         
            return go;
        }

        return null;
    }

    public bool TryFind(Vector3Int cellPos, out GameObject creature)
    {
        creature = null;

        foreach (GameObject go in objectDict.Values)
        {
            if (go.TryGetComponent(out Creature controller) == false) continue;
            if (controller.Position != cellPos) continue;

            creature = go;
            return true;
        }

        return false;   
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
