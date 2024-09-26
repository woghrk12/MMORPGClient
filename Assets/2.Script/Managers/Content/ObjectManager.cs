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

    public void AddPlayer(ObjectInfo info, bool isMine = false)
    {
        if (isMine)
        {
            GameObject go = Managers.Resource.Instantiate("Object/LocalPlayer");

            go.name = info.Name;
            objectDict.Add(info.ObjectID, go);

            LocalPlayer localPlayer = go.GetComponent<LocalPlayer>();

            localPlayer.ID = info.ObjectID;
            localPlayer.Name = info.Name;
            localPlayer.Position = new Vector3Int(info.PosX, info.PosY, 0);
            localPlayer.MoveDirection = info.FacingDirection;
            localPlayer.MoveSpeed = info.MoveSpeed;

            localPlayer.transform.position = new Vector3(localPlayer.Position.x, localPlayer.Position.y, 0f) + new Vector3(0.5f, 0.5f, 0f);
            localPlayer.SetState(info.CurState, EPlayerInput.NONE);

            Managers.Map.AddObject(localPlayer);

            LocalPlayer = localPlayer;
        }
        else
        { 
            GameObject go = Managers.Resource.Instantiate("Object/Player");

            go.name = info.Name;
            objectDict.Add(info.ObjectID, go);

            RemoteObject remoteObject = go.GetComponent<RemoteObject>();

            remoteObject.ID = info.ObjectID;
            remoteObject.Name = info.Name;
            remoteObject.Position = new Vector3Int(info.PosX, info.PosY, 0);
            remoteObject.MoveDirection = info.FacingDirection;
            remoteObject.MoveSpeed = info.MoveSpeed;

            remoteObject.transform.position = new Vector3(remoteObject.Position.x, remoteObject.Position.y, 0f) + new Vector3(0.5f, 0.5f, 0f);
            remoteObject.SetState(info.CurState);

            Managers.Map.AddObject(remoteObject);
        }
    }

    public void Add(int id, GameObject go)
    {
        objectDict.Add(id, go);
    }

    public void RemovePlayer(int leftPlayerID)
    {
        if (objectDict.TryGetValue(leftPlayerID, out GameObject go) == false) return;
        if (go.TryGetComponent(out MMORPG.Object obj) == false) return;

        objectDict.Remove(leftPlayerID);

        Managers.Map.Removeobject(obj);
        Managers.Resource.Destory(go);
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

    public GameObject Find(int objectID)
    {
        return objectDict.TryGetValue(objectID, out GameObject gameObject) ? gameObject : null;
    }

    public bool TryFind(int objectID, out GameObject obj)
    {
        obj = null;

        if (objectDict.TryGetValue(objectID, out GameObject gameobject) == false) return false;

        obj = gameobject;
        return true;
    }

    public GameObject Find(Vector3Int cellPos)
    {
        foreach (GameObject go in objectDict.Values)
        {
            if (go.TryGetComponent(out MMORPG.Object controller) == false) continue;
            if (controller.Position != cellPos) continue;
         
            return go;
        }

        return null;
    }

    public bool TryFind(Vector3Int cellPos, out GameObject obj)
    {
        obj = null;

        foreach (GameObject go in objectDict.Values)
        {
            if (go.TryGetComponent(out MMORPG.Object controller) == false) continue;
            if (controller.Position != cellPos) continue;

            obj = go;
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
