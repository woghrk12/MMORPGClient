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
            localPlayer.MoveDirection = info.MoveDirection;
            localPlayer.FacingDirection = info.FacingDirection;
            localPlayer.MoveSpeed = info.MoveSpeed;
            localPlayer.IsCollidable = info.IsCollidable;

            if (ReferenceEquals(info.ObjectStat, null) == false)
            {
                localPlayer.MaxHP = info.ObjectStat.MaxHP;
                localPlayer.CurHP = info.ObjectStat.CurHP;
                localPlayer.AttackPower = info.ObjectStat.AttackPower;
            }

            localPlayer.gameObject.name = localPlayer.Name;
            localPlayer.transform.position = new Vector3(localPlayer.Position.x, localPlayer.Position.y) + new Vector3(0.5f, 0.5f);

            localPlayer.SetState(info.CurState, EPlayerInput.NONE);

            Managers.Map.AddObject(localPlayer);

            LocalPlayer = localPlayer;
            objectDict.Add(localPlayer.ID, localPlayer);

            HpBar hpBar = Managers.Resource.Instantiate("UI/HpBar").GetComponent<HpBar>();
            hpBar.InitHpBar(localPlayer.transform, 0.65f);

            localPlayer.CurHpModified += hpBar.SetHpBar;

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
        remoteObject.IsCollidable = info.IsCollidable;

        if (ReferenceEquals(info.ObjectStat, null) == false)
        {
            remoteObject.MaxHP = info.ObjectStat.MaxHP;
            remoteObject.CurHP = info.ObjectStat.CurHP;
            remoteObject.AttackPower = info.ObjectStat.AttackPower;
        }

        remoteObject.gameObject.name = remoteObject.Name;
        remoteObject.transform.position = new Vector3(remoteObject.Position.x, remoteObject.Position.y) + new Vector3(0.5f, 0.5f);

        remoteObject.SetState(info.CurState);

        Managers.Map.AddObject(remoteObject);

        objectDict.Add(remoteObject.ID, remoteObject);

        if (type != EGameObjectType.Projectile)
        {
            HpBar hpBar = Managers.Resource.Instantiate("UI/HpBar").GetComponent<HpBar>();
            hpBar.InitHpBar(remoteObject.transform, 0.65f);

            remoteObject.CurHpModified += hpBar.SetHpBar;
        }
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

    public bool TryFind(int objectID, out MMORPG.Object result)
    {
        result = null;

        if (objectDict.TryGetValue(objectID, out MMORPG.Object obj) == false) return false;

        result = obj;
        return true;
    }

    public bool TryFind(Vector3Int position, out MMORPG.Object result)
    {
        result = null;

        foreach (MMORPG.Object obj in objectDict.Values)
        {
            if (obj.Position != position) continue;

            result = obj;
            return true;
        }

        return false;
    }

    public MMORPG.Object Find(Vector3Int position)
    {
        foreach (MMORPG.Object obj in objectDict.Values)
        {
            if (obj.Position != position) continue;

            return obj;
        }

        return null;
    }

    public MMORPG.Object Find(Func<MMORPG.Object, bool> condition)
    {
        if (ReferenceEquals(condition, null) == true) return null;

        foreach (MMORPG.Object obj in objectDict.Values)
        {
            if (condition.Invoke(obj) == false) continue;

            return obj;
        }

        return null;
    }

    #endregion Methods
}
