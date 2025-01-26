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
                CharacterID = packet.Characters[0].CharacterID
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
            CharacterID = packet.NewCharacter.CharacterID
        };

        Managers.Network.Send(characterEnterGameRoomRequestPacket);
    }

    public static void HandleCharacterEnterGameRoomResponse(ServerSession session, IMessage message)
    {
        CharacterEnterGameRoomResponse packet = message as CharacterEnterGameRoomResponse;

        Debug.Log($"CharacterEnterGameRoomResponse. Character ID : {packet.NewCharacter.ObjectID}, # of Other Objects : {packet.OtherObjects.Count}");

        Managers.Obj.AddLocalCharacter(packet.NewCharacter);

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

        Debug.Log($"PlayerLeftRoomBroadcast. Left Character ID : {packet.LeftCharacterID}");

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

    public static void HandleUpdateCreatureStateBroadcast(ServerSession session, IMessage message)
    {
        UpdateCreatureStateBroadcast packet = message as UpdateCreatureStateBroadcast;

        Debug.Log($"UpdateObjectStateBroadcast. Creature ID : {packet.CreatureID}. New State : {packet.NewState}");

        if (Managers.Obj.TryFind(packet.CreatureID, out MMORPG.Object obj) == false) return;

        Creature creature = obj as Creature;
        if (ReferenceEquals(creature, null) == true) return;

        creature.CurState = packet.NewState;
    }

    public static void HandleMoveResponse(ServerSession session, IMessage message)
    {
        MoveResponse packet = message as MoveResponse;

        Debug.Log($"MoveResponse. Result Code : {packet.ResultCode}");

        if (packet.ResultCode == 0) return;

        LocalCharacter localCharacter = Managers.Obj.LocalCharacter;
        if (ReferenceEquals(localCharacter, null) == true) return;

        Managers.Map.MoveObject(localCharacter, new Vector2Int(packet.FixedPosX, packet.FixedPosY));
    }

    public static void HandleMoveBroadcast(ServerSession session, IMessage message)
    {
        MoveBroadcast packet = message as MoveBroadcast;

        Debug.Log($"MoveBroadcast. Object ID : {packet.ObjectID}. Target Pos : ({packet.TargetPosX}, {packet.TargetPosY}), MoveDirection : {packet.MoveDirection}");

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;
        
        EGameObjectType type = ObjectManager.GetObjectTypeByID(packet.ObjectID);

        switch (type)
        {
            case EGameObjectType.Character:
            case EGameObjectType.Monster:
                Creature creature = obj as Creature;

                if (ReferenceEquals(creature, null) == true) return;

                creature.Move(new Vector2Int(packet.TargetPosX, packet.TargetPosY), packet.MoveDirection);

                break;

            case EGameObjectType.Projectile:
                Projectile projectile = obj as Projectile;

                if (ReferenceEquals(projectile, null) == true) return;

                projectile.MoveDirection = packet.MoveDirection;

                break;
        }
    }

    public static void HandlePerformAttackBroadcast(ServerSession session, IMessage message)
    {
        PerformAttackBroadcast packet = message as PerformAttackBroadcast;

        Debug.Log($"PerformAttackBroadcast. Creature ID : {packet.CreatureID}, Attack ID : {packet.AttackID}");

        if (Managers.Obj.TryFind(packet.CreatureID, out MMORPG.Object obj) == false) return;

        Creature creature = obj as Creature;
        
        if (ReferenceEquals(creature, null) == true) return;

        creature.Attack(packet.AttackID, packet.FacingDirection);
    }

    public static void HandleHitBroadcast(ServerSession session, IMessage message)
    {
        HitBroadcast packet = message as HitBroadcast;

        Debug.Log($"HitBroadcast. Attacker ID : {packet.AttackerID}, Defender ID : {packet.DefenderID} Remain HP : {packet.RemainHp}, Damage : {packet.Damage}");

        if (Managers.Obj.TryFind(packet.DefenderID, out MMORPG.Object obj) == false) return;

        Creature creature = obj as Creature;

        if (ReferenceEquals(creature, null) == true) return;

        creature.OnDamaged(packet.RemainHp, packet.Damage);
    }

    public static void HandleCreatureDeadBroadcast(ServerSession session, IMessage message)
    {
        CreatureDeadBroadcast packet = message as CreatureDeadBroadcast;

        Debug.Log($"ObjectDeadBroadcast. Object ID : {packet.CreatureID}, Attacker ID : {packet.AttackerID}");

        if (Managers.Obj.TryFind(packet.CreatureID, out MMORPG.Object obj) == false) return;

        Creature creature = obj as Creature;

        if (ReferenceEquals(creature, null) == true) return;

        creature.OnDead(packet.AttackerID);
    }

    public static void HandleCharacterReviveBroadcast(ServerSession session, IMessage message)
    {
        CharacterReviveBroadcast packet = message as CharacterReviveBroadcast;

        Debug.Log($"ObjectReviveBroadcast. Object ID : {packet.CharacterID}, Revive Pos : ({packet.RevivePosX}, {packet.RevivePosY})");

        if (Managers.Obj.TryFind(packet.CharacterID, out MMORPG.Object obj) == false) return;

        Character character = obj as Character;

        if (ReferenceEquals(character, null) == true) return;

        character.OnRevive(new Vector2Int(packet.RevivePosX, packet.RevivePosY));
    }
}
