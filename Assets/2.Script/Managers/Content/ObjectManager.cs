using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    #region Variables

    private Dictionary<int, MMORPG.Object> objectDict = new();

    #endregion Variables

    #region Properties

    public LocalPlayer LocalPlayer { set; get; } = null;

    #endregion Properties

    #region Methods

    public static EGameObjectType GetObjectTypeByID(int objectID) => (EGameObjectType)(objectID >> 24 & 0x7F);

    public void AddObject(ObjectInfo info, bool isMine = false)
    {
        if (isMine)
        {
            LocalPlayer localPlayer = Managers.Resource.Instantiate("Object/LocalPlayer").GetComponent<LocalPlayer>();

            localPlayer.ID = info.ObjectID;
            localPlayer.Name = info.Name;
            localPlayer.Position = new Vector3Int(info.PosX, info.PosY, 0);
            LocalPlayer.MoveDirection = info.MoveDirection;
            localPlayer.FacingDirection = info.FacingDirection;
            localPlayer.MoveSpeed = info.MoveSpeed;

            localPlayer.gameObject.name = localPlayer.Name;
            localPlayer.transform.position = new Vector3(localPlayer.Position.x, localPlayer.Position.y) + new Vector3(0.5f, 0.5f);

            localPlayer.SetState(info.CurState, EPlayerInput.NONE);

            Managers.Map.AddObject(localPlayer);

            LocalPlayer = localPlayer;
            objectDict.Add(localPlayer.ID, localPlayer);

            return;
        }

        EGameObjectType type = GetObjectTypeByID(info.ObjectID);
        RemoteObject remoteObject = null;

        switch (type)
        {
            case EGameObjectType.Player:
                remoteObject = Managers.Resource.Instantiate("Object/Player").GetComponent<RemoteObject>();
                break;

            case EGameObjectType.Monster:
                remoteObject = Managers.Resource.Instantiate("Object/Monster").GetComponent<RemoteObject>();
                break;

            case EGameObjectType.Projectile:
                remoteObject = Managers.Resource.Instantiate("Object/Projectile").GetComponent<RemoteObject>();
                break;
        }

        remoteObject.ID = info.ObjectID;
        remoteObject.Name = info.Name;
        remoteObject.Position = new Vector3Int(info.PosX, info.PosY);
        remoteObject.MoveDirection = info.MoveDirection;
        remoteObject.FacingDirection = info.FacingDirection;
        remoteObject.MoveSpeed = info.MoveSpeed;

        remoteObject.gameObject.name = remoteObject.Name;
        remoteObject.transform.position = new Vector3(remoteObject.Position.x, remoteObject.Position.y) + new Vector3(0.5f, 0.5f);

        remoteObject.SetState(info.CurState);

        Managers.Map.AddObject(remoteObject);

        objectDict.Add(remoteObject.ID, remoteObject);
    }

    public void RemoveObject(int oldObjectID)
    {
        if (objectDict.TryGetValue(oldObjectID, out MMORPG.Object obj) == false) return;

        objectDict.Remove(obj.ID);

        Managers.Map.RemoveObject(obj);
        Managers.Resource.Destory(obj.gameObject);
    }

    public void Clear()
    {
        objectDict.Clear();
    }

    public bool TryFind(int objectID, out GameObject obj)
    {
        obj = null;

        if (objectDict.TryGetValue(objectID, out MMORPG.Object gameobject) == false) return false;

        obj = gameobject.gameObject;
        return true;
    }

    public GameObject Find(Vector3Int cellPos)
    {
        foreach (MMORPG.Object obj in objectDict.Values)
        {
            if (obj.Position != cellPos) continue;
         
            return obj.gameObject;
        }

        return null;
    }

    public bool TryFind(Vector3Int cellPos, out GameObject obj)
    {
        obj = null;

        foreach (MMORPG.Object go in objectDict.Values)
        {
            if (go.Position != cellPos) continue;

            obj = go.gameObject;
            return true;
        }

        return false;   
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        if (ReferenceEquals(condition, null) == true) return null;

        foreach (MMORPG.Object go in objectDict.Values)
        {
            if (condition.Invoke(go.gameObject) == false) continue;

            return go.gameObject;
        }

        return null;
    }
    
    #endregion Methods
}
