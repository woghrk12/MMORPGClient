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
        receivedPacketHandlerDict.Add((ushort)EMessageID.StatDataBroadcast, MakePacket<StatDataBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PlayerEnteredRoomResponse, MakePacket<PlayerEnteredRoomResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PlayerEnteredRoomBroadcast, MakePacket<PlayerEnteredRoomBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PlayerLeftRoomResponse, MakePacket<PlayerLeftRoomResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PlayerLeftRoomBroadcast, MakePacket<PlayerLeftRoomBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.ObjectSpawnedBroadcast, MakePacket<ObjectSpawnedBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.ObjectDespawnedBroadcast, MakePacket<ObjectDespawnedBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PerformMoveBroadcast, MakePacket<PerformMoveBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PerformAttackBroadcast, MakePacket<PerformAttackBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.HitBroadcast, MakePacket<HitBroadcast>);

        handlerDict.Add((ushort)EMessageID.StatDataBroadcast, PacketHandler.HandleStatDataBroadcast);
        handlerDict.Add((ushort)EMessageID.PlayerEnteredRoomResponse, PacketHandler.HandlePlayerEnteredRoomResponse);
        handlerDict.Add((ushort)EMessageID.PlayerEnteredRoomBroadcast, PacketHandler.HandlePlayerEnteredRoomBroadcast);
        handlerDict.Add((ushort)EMessageID.PlayerLeftRoomResponse, PacketHandler.HandlePlayerLeftRoomResponse);
        handlerDict.Add((ushort)EMessageID.PlayerLeftRoomBroadcast, PacketHandler.HandlePlayerLeftRoomBroadcast);
        handlerDict.Add((ushort)EMessageID.ObjectSpawnedBroadcast, PacketHandler.HandleObjectSpawnedBroadcast);
        handlerDict.Add((ushort)EMessageID.ObjectDespawnedBroadcast, PacketHandler.HandleObjectDespawnedBroadcast);
        handlerDict.Add((ushort)EMessageID.PerformMoveBroadcast, PacketHandler.HandlePerformMoveBroadcast);
        handlerDict.Add((ushort)EMessageID.PerformAttackBroadcast, PacketHandler.HandlePerformAttackBroadcast);
        handlerDict.Add((ushort)EMessageID.HitBroadcast, PacketHandler.HandleHitBroadcast);
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
