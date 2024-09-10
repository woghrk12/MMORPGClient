using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;

public class PacketManager
{
    #region Variables

    private static PacketManager instance = new();

    private Dictionary<ushort, Action<ServerSession, ArraySegment<byte>, ushort>> receivedPacketHandlerDict = new();
    private Dictionary<ushort, Action<ServerSession, IMessage>> handlerDict = new();

    #endregion Variables

    #region Properties

    public static PacketManager Instance => instance;

    #endregion Properties

    #region Constructor

    public PacketManager()
    {
        receivedPacketHandlerDict.Add((ushort)EMessageID.PlayerEnteredRoomResponse, MakePacket<PlayerEnteredRoomResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PlayerEnteredRoomBrodcast, MakePacket<PlayerEnteredRoomBrodcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PlayerLeftRoomResponse, MakePacket<PlayerLeftRoomResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PlayerLeftRoomBrodcast, MakePacket<PlayerLeftRoomBrodcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.CreatureSpawnedBrodcast, MakePacket<CreatureSpawnedBrodcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.CreatureDespawnedBrodcast, MakePacket<CreatureDespawnedBrodcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PerformMoveResponse, MakePacket<PerformMoveResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PerformMoveBroadcast, MakePacket<PerformMoveBroadcast>);

        handlerDict.Add((ushort)EMessageID.PlayerEnteredRoomResponse, PacketHandler.HandlePlayerEnteredRoomResponse);
        handlerDict.Add((ushort)EMessageID.PlayerEnteredRoomBrodcast, PacketHandler.HandlePlayerEnteredRoomBrodcast);
        handlerDict.Add((ushort)EMessageID.PlayerLeftRoomResponse, PacketHandler.HandlePlayerLeftRoomResponse);
        handlerDict.Add((ushort)EMessageID.PlayerLeftRoomBrodcast, PacketHandler.HandlePlayerLeftRoomBrodcast);
        handlerDict.Add((ushort)EMessageID.CreatureSpawnedBrodcast, PacketHandler.HandleCreatureSpawnedBrodcast);
        handlerDict.Add((ushort)EMessageID.CreatureDespawnedBrodcast, PacketHandler.HandleCreatureDespawnedBrodcast);
        handlerDict.Add((ushort)EMessageID.PerformMoveResponse, PacketHandler.HandlePerformMoveResponse);
        handlerDict.Add((ushort)EMessageID.PerformMoveBroadcast, PacketHandler.HandlePerformMoveBroadcast);
    }

    #endregion Constructor

    #region Methods

    public Action<ServerSession, IMessage> GetPacketHandler(ushort id)
    {
        return handlerDict.TryGetValue(id, out Action<ServerSession, IMessage> action) == true ? action : null;
    }

    public bool TryGetPacketHandler(ushort id, out Action<ServerSession, IMessage> handler)
    {
        if (handlerDict.TryGetValue(id, out Action<ServerSession, IMessage> action) == false)
        {
            handler = null;
            return false;
        }

        handler = action;
        return true;
    }

    private void MakePacket<T>(ServerSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
    {
        T packet = new();

        packet.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

        PacketQueue.Instance.Push(id, packet);
    }

    #region Events

    public void OnReceivePacket(ServerSession session, ArraySegment<byte> buffer)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        if (receivedPacketHandlerDict.TryGetValue(id, out Action<ServerSession, ArraySegment<byte>, ushort> action))
        {
            action.Invoke(session, buffer, id);
        }
    }

    #endregion Events

    #endregion Methods
}
