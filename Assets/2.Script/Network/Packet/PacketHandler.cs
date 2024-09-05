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

    public static void HandleUpdateCreatureInfoBroadcast(ServerSession session, IMessage message)
    {
        UpdateCreatureInfoBroadcast packet = message as UpdateCreatureInfoBroadcast;
        CreatureInfo info = packet.CreatureInfo;
        
        if (Managers.Obj.TryFind(info.CreatureID, out GameObject creature) == false) return;
        if (creature.TryGetComponent(out Creature controller) == false) return;

        // TODO : Classify the creature by the id
        //controller.CurState = info.CurState;
        controller.CellPos = new Vector3Int(info.CellPosX, info.CellPosY, 0);
        controller.FacingDirection = info.FacingDirection;
        controller.MoveSpeed = info.MoveSpeed;
    }
}
