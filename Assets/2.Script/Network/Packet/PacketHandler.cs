using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

public class PacketHandler
{
    public static void HandlePlayerEnteredRoomResponse(ServerSession session, IMessage message)
    {
        PlayerEnteredRoomResponse packet = message as PlayerEnteredRoomResponse;

        Managers.Obj.AddPlayer(packet.NewPlayer, isMine: true);

        foreach(CreatureInfo info in packet.OtherPlayers)
        {
            Managers.Obj.AddPlayer(info, isMine: false);
        }
    }

    public static void HandlePlayerEnteredRoomBrodcast(ServerSession session, IMessage message)
    {
        PlayerEnteredRoomBrodcast packet = message as PlayerEnteredRoomBrodcast;

        Managers.Obj.AddPlayer(packet.NewPlayer, isMine: false);
    }

    public static void HandlePlayerLeftRoomResponse(ServerSession session, IMessage message)
    {
        PlayerLeftRoomResponse packet = message as PlayerLeftRoomResponse;
    }

    public static void HandlePlayerLeftRoomBrodcast(ServerSession session, IMessage message)
    {
        PlayerLeftRoomBrodcast packet = message as PlayerLeftRoomBrodcast;

        Managers.Obj.RemovePlayer(packet.OtherPlayerID);
    }

    public static void HandleCreatureSpawnedBrodcast(ServerSession session, IMessage message)
    {
        CreatureSpawnedBrodcast packet = message as CreatureSpawnedBrodcast;
    }

    public static void HandleCreatureDespawnedBrodcast(ServerSession session, IMessage message)
    {
        CreatureDespawnedBrodcast packet = message as CreatureDespawnedBrodcast;
    }

    public static void HandlePerformMoveResponse(ServerSession session, IMessage message)
    {
        PerformMoveResponse packet = message as PerformMoveResponse;
        LocalPlayer localPlayer = Managers.Obj.LocalPlayer;
        
        if (ReferenceEquals(localPlayer, null) == true) return;

        localPlayer.Position = new Vector3Int(packet.TargetPosX, packet.TargetPosY, 0);
        
        Managers.Map.MoveCreature(localPlayer.ID, new Vector3Int(packet.CurPosX, packet.CurPosY, 0), new Vector3Int(packet.TargetPosX, packet.TargetPosY, 0));
    }

    public static void HandlePerformMoveBroadcast(ServerSession session, IMessage message)
    {
        PerformMoveBroadcast packet = message as PerformMoveBroadcast;

        if (ReferenceEquals(Managers.Obj.LocalPlayer, null) == false && Managers.Obj.LocalPlayer.ID == packet.CreatureID) return;
        if (Managers.Obj.TryFind(packet.CreatureID, out GameObject creature) == false) return;
        if (creature.TryGetComponent(out RemoteCreature controller) == false) return;

        if (packet.MoveDirection == EMoveDirection.None)
        {
            controller.SetState(ECreatureState.Idle);
        }
        else
        {
            controller.MoveDirection = packet.MoveDirection;
            controller.Position = new Vector3Int(packet.TargetPosX, packet.TargetPosY);
            controller.SetState(ECreatureState.Move);

            Managers.Map.MoveCreature(packet.CreatureID, new Vector3Int(packet.CurPosX, packet.CurPosY, 0), new Vector3Int(packet.TargetPosX, packet.TargetPosY, 0));
        }
    }
}
