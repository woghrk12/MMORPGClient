using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

public class PacketHandler
{
    public static void HandlePlayerEnterBrodcast(ServerSession session, IMessage message)
    {
        PlayerEnterBrodcast packet = message as PlayerEnterBrodcast;

        Managers.Obj.AddPlayer(packet.Player, isMine: true);
    }

    public static void HandlePlayerLeaveBrodcast(ServerSession session, IMessage message)
    {
        PlayerLeaveBrodcast packet = message as PlayerLeaveBrodcast;
    }

    public static void HandleCreatureSpawnBrodcast(ServerSession session, IMessage message)
    {
        CreatureSpawnBrodcast packet = message as CreatureSpawnBrodcast;
    }

    public static void HandleCreatureDespawnBrodcast(ServerSession session, IMessage message)
    {
        CreatureDespawnBrodcast packet = message as CreatureDespawnBrodcast;
    }

    public static void HandleCreatureMoveBrodcast(ServerSession session, IMessage message)
    {
        CreatureMoveBrodcast packet = message as CreatureMoveBrodcast;
    }
}
