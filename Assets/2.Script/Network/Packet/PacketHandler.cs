using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

public class PacketHandler
{
    public static void HandleStatDataBroadcast(ServerSession session, IMessage message)
    {
        StatDataBroadcast packet = message as StatDataBroadcast;

        Debug.Log($"StatDataBroadcast. Data type : {packet.DataType}");

        Managers.Data.SetData(packet.DataType, packet.Data);
    }

    public static void HandlePlayerEnteredRoomResponse(ServerSession session, IMessage message)
    {
        PlayerEnteredRoomResponse packet = message as PlayerEnteredRoomResponse;

        Managers.Obj.AddLocalPlayer(packet.NewPlayer);

        foreach(ObjectInfo info in packet.OtherObjects)
        {
            Managers.Obj.AddObject(info);
        }
    }

    public static void HandlePlayerEnteredRoomBroadcast(ServerSession session, IMessage message)
    {
        PlayerEnteredRoomBroadcast packet = message as PlayerEnteredRoomBroadcast;

        Managers.Obj.AddObject(packet.NewPlayer);
    }

    public static void HandlePlayerLeftRoomResponse(ServerSession session, IMessage message)
    {
        PlayerLeftRoomResponse packet = message as PlayerLeftRoomResponse;
    }

    public static void HandlePlayerLeftRoomBroadcast(ServerSession session, IMessage message)
    {
        PlayerLeftRoomBroadcast packet = message as PlayerLeftRoomBroadcast;

        Managers.Obj.RemoveObject(packet.OtherPlayerID);
    }

    public static void HandleObjectSpawnedBroadcast(ServerSession session, IMessage message)
    {
        ObjectSpawnedBroadcast packet = message as ObjectSpawnedBroadcast;

        Managers.Obj.AddObject(packet.NewObjectInfo);
    }

    public static void HandleObjectDespawnedBroadcast(ServerSession session, IMessage message)
    {
        ObjectDespawnedBroadcast packet = message as ObjectDespawnedBroadcast;
        
        Debug.Log($"ObjectDespawnedBroadcast. Old Object ID : {packet.OldObjectID}");

        Managers.Obj.RemoveObject(packet.OldObjectID);
    }

    public static void HandlePerformMoveBroadcast(ServerSession session, IMessage message)
    {
        PerformMoveBroadcast packet = message as PerformMoveBroadcast;

        Debug.Log($"PerformMoveBroadcast. Object ID : {packet.ObjectID}. Target Pos : ({packet.TargetPosX}, {packet.TargetPosY})");

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;

        obj.MoveDirection = packet.MoveDirection;

        Managers.Map.MoveObject(obj, new Vector3Int(packet.TargetPosX, packet.TargetPosY));

        if (Managers.Obj.LocalPlayer.ID != packet.ObjectID)
        {
            (obj as RemoteObject).SetState(packet.MoveDirection == EMoveDirection.None ? EObjectState.Idle : EObjectState.Move);
        }
    }

    public static void HandlePerformAttackBroadcast(ServerSession session, IMessage message)
    {
        PerformAttackBroadcast packet = message as PerformAttackBroadcast;

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;

        if (Managers.Obj.LocalPlayer.ID == packet.ObjectID)
        {
            (obj as LocalPlayer).PerformAttack(packet.AttackID);
        }
        else
        {
            (obj as RemoteObject).PerformAttack(packet.AttackID);
        }
    }

    public static void HandleAttackCompleteBroadcast(ServerSession session, IMessage message)
    {
        AttackCompleteBroadcast packet = message as AttackCompleteBroadcast;

        if (Managers.Obj.TryFind(packet.ObjectID, out MMORPG.Object obj) == false) return;

        if (Managers.Obj.LocalPlayer.ID == packet.ObjectID)
        {
            (obj as LocalPlayer).SetState(EObjectState.Idle, EPlayerInput.NONE);
        }
        else
        {
            (obj as RemoteObject).SetState(EObjectState.Idle);
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
