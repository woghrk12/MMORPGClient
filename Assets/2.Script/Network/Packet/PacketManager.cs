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
        // Implement the constructor to register methods for handling the packets that the client needs.
        // Example
        // {0} : The class name of the packet.
        // receivedPacketHandlerDict.Add((ushort)EMessageID.{0}}, MakePacket<{0}}>);
        // handlerDict.Add((ushort)EMessageID.{0}}, PacketHandler.Handle{0}});
        receivedPacketHandlerDict.Add((ushort)EMessageID.PlayerMoveBrodcast, MakePacket<PlayerMoveBrodcast>);
        handlerDict.Add((ushort)EMessageID.PlayerMoveBrodcast, PacketHandler.HandlePlayerMoveBrodcast);
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

        if (handlerDict.TryGetValue(id, out Action<ServerSession, IMessage> action))
        {
            action.Invoke(session, packet);
        }
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
