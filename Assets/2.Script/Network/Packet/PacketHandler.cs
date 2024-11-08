using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

public class PacketHandler
{
    public static void HandleConnectedResponse(ServerSession session, IMessage message)
    {
        ConnectedResponse packet = message as ConnectedResponse;

        Debug.Log("ConnectedResponse.");

        foreach (StatData statData in packet.Stats)
        {
            Managers.Data.SetData(statData.StatType, statData.Data);
            Debug.Log($"SetData. Type : {statData.StatType}");
        }

        LoginRequest loginRequestPacket = new() { Id = SystemInfo.deviceUniqueIdentifier };
        Managers.Network.Send(loginRequestPacket);
    }

    public static void HandleLoginResponse(ServerSession session, IMessage message)
    {
        LoginResponse packet = message as LoginResponse;

        Debug.Log($"LoginResponse. Result code : {packet.ResultCode}");

        if (ReferenceEquals(packet.Characters, null) == true || packet.Characters.Count == 0)
        {
            CreateCharacterRequest createCharacterRequestPacket = new()
            {
                Name = $"Player_{Random.Range(0, 10000).ToString("0000")}"
            };

            Managers.Network.Send(createCharacterRequestPacket);
        }
        else
        {
            CharacterEnterGameRoomRequest characterEnterGameRoomRequestPacket = new()
            {
                Name = packet.Characters[0].Name
            };

            Managers.Network.Send(characterEnterGameRoomRequestPacket);
        }
    }

    public static void HandleCreateCharacterResponse(ServerSession session, IMessage message)
    {
        CreateCharacterResponse packet = message as CreateCharacterResponse;

        Debug.Log($"CreateCharacterResponse. Result Code : {packet.ResultCode}, New Character Name : {packet.NewCharacter.Name}");

        CharacterEnterGameRoomRequest characterEnterGameRoomRequestPacket = new()
        {
            Name = packet.NewCharacter.Name
        };

        Managers.Network.Send(characterEnterGameRoomRequestPacket);
    }

    public static void HandleCharacterEnterGameRoomResponse(ServerSession session, IMessage message)
    {
        CharacterEnterGameRoomResponse packet = message as CharacterEnterGameRoomResponse;

        Debug.Log($"CharacterEnterGameRoomResponse. Character ID : {packet.NewCharacter.ObjectID}, # of Other Objects : {packet.OtherObjects.Count}");

        Managers.Obj.AddLocalPlayer(packet.NewCharacter);

        foreach (ObjectInfo info in packet.OtherObjects)
        {
            Managers.Obj.AddObject(info);
        }
    }

    public static void HandleCharacterEnterGameRoomBroadcast(ServerSession session, IMessage message)
    {
        CharacterEnterGameRoomBroadcast packet = message as CharacterEnterGameRoomBroadcast;

        Debug.Log($"CharacterEnterGameRoomBroadcast. New Character ID : {packet.NewCharacter.ObjectID}");

        Managers.Obj.AddObject(packet.NewCharacter);
    }

    public static void HandleCharacterLeftGameRoomResponse(ServerSession session, IMessage message)
    {
        CharacterLeftGameRoomResponse packet = message as CharacterLeftGameRoomResponse;

        Debug.Log("CharacterLeftGameRoomResponse.");
    }

    public static void HandleCharacterLeftGameRoomBroadcast(ServerSession session, IMessage message)
    {
        CharacterLeftGameRoomBroadcast packet = message as CharacterLeftGameRoomBroadcast;

        Debug.Log($"PlayerLeftRoomBroadcast. Left Player ID : {packet.LeftCharacterID}");

        Managers.Obj.RemoveObject(packet.LeftCharacterID);
    }

    public static void HandleObjectSpawnedBroadcast(ServerSession session, IMessage message)
    {
        ObjectSpawnedBroadcast packet = message as ObjectSpawnedBroadcast;

        Debug.Log($"ObjectSpawnedBroadcast. New Object ID : {packet.NewObjectInfo.ObjectID}");

        Managers.Obj.AddObject(packet.NewObjectInfo);
    }

    public static void HandleObjectDespawnedBroadcast(ServerSession session, IMessage message)
    {
        ObjectDespawnedBroadcast packet = message as ObjectDespawnedBroadcast;
        
        Debug.Log($"ObjectDespawnedBroadcast. Old Object ID : {packet.OldObjectID}");

        Managers.Obj.RemoveObject(packet.OldObjectID);
    }

    public static void HandleUpdateObjectStateBroadcast(ServerSession session, IMessage message)
    {
        UpdateObjectStateBroadcast packet = message as UpdateObjectStateBroadcast;

        Debug.Log($"UpdateObjectStateBroadcast. Object ID : {packet.ObjectID}. New State : {packet.NewState}");

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;

        if (Managers.Obj.LocalCharacter.ID == obj.ID)
        {
            (obj as LocalCharacter).SetState(packet.NewState);
        }
        else
        {
            (obj as RemoteObject).SetState(packet.NewState);
        }
    }

    public static void HandlePerformMoveBroadcast(ServerSession session, IMessage message)
    {
        PerformMoveBroadcast packet = message as PerformMoveBroadcast;

        Debug.Log($"PerformMoveBroadcast. Object ID : {packet.ObjectID}. Target Pos : ({packet.TargetPosX}, {packet.TargetPosY})");

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;

        obj.MoveDirection = packet.MoveDirection;

        Managers.Map.MoveObject(obj, new Vector3Int(packet.TargetPosX, packet.TargetPosY));

        if (Managers.Obj.LocalCharacter.ID != packet.ObjectID)
        {
            (obj as RemoteObject).SetState(packet.MoveDirection == EMoveDirection.None ? ECreatureState.Idle : ECreatureState.Move);
        }
    }

    public static void HandlePerformAttackBroadcast(ServerSession session, IMessage message)
    {
        PerformAttackBroadcast packet = message as PerformAttackBroadcast;

        Debug.Log($"PerformAttackBroadcast. Object ID : {packet.ObjectID}, Attack ID : {packet.AttackID}");

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;

        if (Managers.Obj.LocalCharacter.ID == packet.ObjectID)
        {
            (obj as LocalCharacter).PerformAttack(packet.AttackID);
        }
        else
        {
            (obj as RemoteObject).PerformAttack(packet.AttackID);
        }
    }

    public static void HandleAttackCompleteBroadcast(ServerSession session, IMessage message)
    {
        AttackCompleteBroadcast packet = message as AttackCompleteBroadcast;

        Debug.Log($"AttackCompleteBroadcast. Object ID : {packet.ObjectID}");

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;

        if (Managers.Obj.LocalCharacter.ID == packet.ObjectID)
        {
            (obj as LocalCharacter).SetState(ECreatureState.Idle, EPlayerInput.NONE);
        }
        else
        {
            (obj as RemoteObject).SetState(ECreatureState.Idle);
        }
    }

    public static void HandleHitBroadcast(ServerSession session, IMessage message)
    {
        HitBroadcast packet = message as HitBroadcast;

        Debug.Log($"HitBroadcast. Object ID : {packet.ObjectID}, Remain HP : {packet.CurHp}, Damage : {packet.Damage}");

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;

        obj.OnDamaged(packet.CurHp, packet.Damage);
    }

    public static void HandleObjectDeadBroadcast(ServerSession session, IMessage message)
    {
        ObjectDeadBroadcast packet = message as ObjectDeadBroadcast;

        Debug.Log($"ObjectDeadBroadcast. Object ID : {packet.ObjectID}, Attacker ID : {packet.AttackerID}");

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;
        if (Managers.Obj.TryFind(packet.AttackerID, out MMORPG.Object attacker) == false) return;

        obj.OnDead(attacker);
    }

    public static void HandleObjectReviveBroadcast(ServerSession session, IMessage message)
    {
        ObjectReviveBroadcast packet = message as ObjectReviveBroadcast;

        Debug.Log($"ObjectReviveBroadcast. Object ID : {packet.ObjectID}, Revive Pos : ({packet.RevivePosX}, {packet.RevivePosY})");

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;

        obj.OnRevive(new Vector3Int(packet.RevivePosX, packet.RevivePosY));
    }
}
