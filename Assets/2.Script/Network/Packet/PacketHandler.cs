using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

public class PacketHandler
{
    public static void HandlePlayerEnteredRoomResponse(ServerSession session, IMessage message)
    {
        PlayerEnteredRoomResponse packet = message as PlayerEnteredRoomResponse;

        Managers.Obj.AddObject(packet.NewPlayer, isMine: true);

        foreach(ObjectInfo info in packet.OtherPlayers)
        {
            Managers.Obj.AddObject(info, isMine: false);
        }
    }

    public static void HandlePlayerEnteredRoomBroadcast(ServerSession session, IMessage message)
    {
        PlayerEnteredRoomBroadcast packet = message as PlayerEnteredRoomBroadcast;

        Managers.Obj.AddObject(packet.NewPlayer, isMine: false);
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

        Managers.Obj.RemoveObject(packet.OldObjectID);
    }

    public static void HandlePerformMoveResponse(ServerSession session, IMessage message)
    {
        PerformMoveResponse packet = message as PerformMoveResponse;
        LocalPlayer localPlayer = Managers.Obj.LocalPlayer;
        
        if (ReferenceEquals(localPlayer, null) == true) return;

        localPlayer.Position = new Vector3Int(packet.TargetPosX, packet.TargetPosY, 0);
        
        Managers.Map.MoveObject(localPlayer.ID, new Vector3Int(packet.CurPosX, packet.CurPosY, 0), new Vector3Int(packet.TargetPosX, packet.TargetPosY, 0));
    }

    public static void HandlePerformMoveBroadcast(ServerSession session, IMessage message)
    {
        PerformMoveBroadcast packet = message as PerformMoveBroadcast;
        LocalPlayer localPlayer = Managers.Obj.LocalPlayer;

        if (ReferenceEquals(localPlayer, null) == false && localPlayer.ID == packet.ObjectID) return;
        if (Managers.Obj.TryFind(packet.ObjectID, out GameObject obj) == false) return;
        if (obj.TryGetComponent(out RemoteObject controller) == false) return;

        controller.MoveDirection = packet.MoveDirection;
        controller.Position = new Vector3Int(packet.TargetPosX, packet.TargetPosY, 0);
        
        if (packet.MoveDirection == EMoveDirection.None)
        {
            controller.SetState(EObjectState.Idle);
        }
        else
        {
            controller.SetState(EObjectState.Move);

            Managers.Map.MoveObject(packet.ObjectID, new Vector3Int(packet.CurPosX, packet.CurPosY, 0), new Vector3Int(packet.TargetPosX, packet.TargetPosY, 0));
        }
    }

    public static void HandlePerformAttackResponse(ServerSession session, IMessage message)
    {
        PerformAttackResponse packet = message as PerformAttackResponse;
        LocalPlayer localPlayer = Managers.Obj.LocalPlayer;

        if (ReferenceEquals(localPlayer, null) == true) return;

        localPlayer.PerformAttack(packet.AttackStartTime, packet.AttackInfo);
    }

    public static void HandlePerformAttackBroadcast(ServerSession session, IMessage message)
    {
        PerformAttackBroadcast packet = message as PerformAttackBroadcast;
        LocalPlayer localPlayer = Managers.Obj.LocalPlayer;

        if (ReferenceEquals(localPlayer, null) == false && localPlayer.ID == packet.ObjectID) return;
        if (Managers.Obj.TryFind(packet.ObjectID, out GameObject obj) == false) return;
        if (obj.TryGetComponent(out RemoteObject controller) == false) return;

        controller.PerformAttack(packet.AttackStartTime, packet.AttackInfo);
    }

    public static void HandleHitBroadcast(ServerSession session, IMessage message)
    {
        HitBroadcast packet = message as HitBroadcast;

        if (Managers.Obj.TryFind(packet.AttackerID, out GameObject attackerObj) == false) return;
        if (attackerObj.TryGetComponent(out MMORPG.Object attacker) == false) return;
        if (Managers.Obj.TryFind(packet.DefenderID, out GameObject defenderObj) == false) return;
        if (defenderObj.TryGetComponent(out MMORPG.Object defender) == false) return;

        defender.OnDamaged();
    }
}
