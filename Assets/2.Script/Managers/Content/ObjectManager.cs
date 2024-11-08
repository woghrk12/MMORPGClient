using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    #region Variables

    private Dictionary<int, MMORPG.Object> objectDict = new();

    #endregion Variables

    #region Methods

    public static EGameObjectType GetObjectTypeByID(int objectID) => (EGameObjectType)(objectID >> 24 & 0x7F);

    public void AddObject(ObjectInfo info, bool isMine = false)
    {
        EGameObjectType type = GetObjectTypeByID(info.ObjectID);
        MMORPG.Object obj = null;

        switch (type)
        {
            case EGameObjectType.Character:
                obj = Managers.Resource.Instantiate("Object/Character").GetComponent<Character>();
                break;

            case EGameObjectType.Monster:
                obj = Managers.Resource.Instantiate("Object/Monster").GetComponent<Monster>();
                break;

            case EGameObjectType.Projectile:
                obj = Managers.Resource.Instantiate("Object/Projectile").GetComponent<Projectile>();
                break;
        }

        if (isMine)
        {
            // TODO : Add controller component to local character object

            Character character = (Character)obj;
            character.Updated += () => { Camera.main.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, -10f); };
            character.CreatureDead += () => Managers.Resource.Instantiate("UI/DeadUI");
        }

        obj.Init(info);

        Managers.Map.AddObject(obj);

        objectDict.Add(obj.ID, obj);
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
