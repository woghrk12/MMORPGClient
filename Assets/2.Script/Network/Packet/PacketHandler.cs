using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

public class PacketHandler
{
    public static void HandleConnectedResponse(ServerSession session, IMessage message)
    {
        ConnectedResponse packet = message as ConnectedResponse;

        Debug.Log("ConnectedResponse.");

        LoginRequest loginRequestPacket = new() { Id = SystemInfo.deviceUniqueIdentifier };
        Managers.Network.Send(loginRequestPacket);
    }

    public static void HandleLoginResponse(ServerSession session, IMessage message)
    {
        LoginResponse packet = message as LoginResponse;

        Debug.Log($"LoginResponse. Result code : {packet.ResultCode}");
    }

    public static void HandleStatDataBroadcast(ServerSession session, IMessage message)
    {
        StatDataBroadcast packet = message as StatDataBroadcast;

        Debug.Log($"StatDataBroadcast. Data type : {packet.DataType}");

        Managers.Data.SetData(packet.DataType, packet.Data);
    }

    public static void HandlePlayerEnteredRoomResponse(ServerSession session, IMessage message)
    {
        PlayerEnteredRoomResponse packet = message as PlayerEnteredRoomResponse;

        Debug.Log($"PlayerEnteredRoomResponse. Player ID : {packet.NewPlayer.ObjectID}, # of Other Objects : {packet.OtherObjects.Count}");

        Managers.Obj.AddLocalPlayer(packet.NewPlayer);

        foreach(ObjectInfo info in packet.OtherObjects)
        {
            Managers.Obj.AddObject(info);
        }
    }

    public static void HandlePlayerEnteredRoomBroadcast(ServerSession session, IMessage message)
    {
        PlayerEnteredRoomBroadcast packet = message as PlayerEnteredRoomBroadcast;

        Debug.Log($"PlayerEnteredRoomBroadcast. New Player ID : {packet.NewPlayer.ObjectID}");

        Managers.Obj.AddObject(packet.NewPlayer);
    }

    public static void HandlePlayerLeftRoomResponse(ServerSession session, IMessage message)
    {
        PlayerLeftRoomResponse packet = message as PlayerLeftRoomResponse;

        Debug.Log("PlayerLeftRoomResponse.");
    }

    public static void HandlePlayerLeftRoomBroadcast(ServerSession session, IMessage message)
    {
        PlayerLeftRoomBroadcast packet = message as PlayerLeftRoomBroadcast;

        Debug.Log($"PlayerLeftRoomBroadcast. Left Player ID : {packet.OtherPlayerID}");

        Managers.Obj.RemoveObject(packet.OtherPlayerID);
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

        if (Managers.Obj.LocalPlayer.ID == obj.ID)
        {
            (obj as LocalPlayer).SetState(packet.NewState);
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

        if (Managers.Obj.LocalPlayer.ID != packet.ObjectID)
        {
            (obj as RemoteObject).SetState(packet.MoveDirection == EMoveDirection.None ? EObjectState.Idle : EObjectState.Move);
        }
    }

    public static void HandlePerformAttackBroadcast(ServerSession session, IMessage message)
    {
        PerformAttackBroadcast packet = message as PerformAttackBroadcast;

        Debug.Log($"PerformAttackBroadcast. Object ID : {packet.ObjectID}, Attack ID : {packet.AttackID}");

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

        Debug.Log($"AttackCompleteBroadcast. Object ID : {packet.ObjectID}");

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
