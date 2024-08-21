using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

public class PacketHandler
{
    public static void HandlePlayerEnteredRoomResponse(ServerSession session, IMessage message)
    {
        PlayerEnteredRoomResponse packet = message as PlayerEnteredRoomResponse;

        Managers.Obj.AddPlayer(packet.MyInfo, isMine: true);
        foreach (PlayerInfo info in packet.OtherPlayers)
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

        Managers.Obj.Remove(packet.OtherPlayerID);
    }

    public static void HandleCreatureSpawnedBrodcast(ServerSession session, IMessage message)
    {
        CreatureSpawnedBrodcast packet = message as CreatureSpawnedBrodcast;
    }

    public static void HandleCreatureDespawnedBrodcast(ServerSession session, IMessage message)
    {
        CreatureDespawnedBrodcast packet = message as CreatureDespawnedBrodcast;
    }

    public static void HandleCreatureMoveBrodcast(ServerSession session, IMessage message)
    {
        CreatureMoveBrodcast packet = message as CreatureMoveBrodcast;
        
        Debug.Log($"CreatureMoveBroadcast. Session ID : {packet.CreatureID} ({packet.PosX}, {packet.PosY}, {packet.MoveDirection})");

        if (Managers.Obj.TryFind(packet.CreatureID, out GameObject creature) == false) return;
        if (creature.TryGetComponent(out CreatureController controller) == false) return;

        controller.SetNextPos(packet.MoveDirection);
    }
}
