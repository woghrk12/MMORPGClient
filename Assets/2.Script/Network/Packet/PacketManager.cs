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
        receivedPacketHandlerDict.Add((ushort)EMessageID.ConnectedResponse, MakePacket<ConnectedResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.LoginResponse, MakePacket<LoginResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.CreateCharacterResponse, MakePacket<CreateCharacterResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.CharacterEnterGameRoomResponse, MakePacket<CharacterEnterGameRoomResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.CharacterEnterGameRoomBroadcast, MakePacket<CharacterEnterGameRoomBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.CharacterLeftGameRoomResponse, MakePacket<CharacterLeftGameRoomResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.CharacterLeftGameRoomBroadcast, MakePacket<CharacterLeftGameRoomBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.ObjectSpawnedBroadcast, MakePacket<ObjectSpawnedBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.ObjectDespawnedBroadcast, MakePacket<ObjectDespawnedBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.UpdateCreatureStateBroadcast, MakePacket<UpdateCreatureStateBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.MoveResponse, MakePacket<MoveResponse>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.MoveBroadcast, MakePacket<MoveBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.PerformAttackBroadcast, MakePacket<PerformAttackBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.HitBroadcast, MakePacket<HitBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.CreatureDeadBroadcast, MakePacket<CreatureDeadBroadcast>);
        receivedPacketHandlerDict.Add((ushort)EMessageID.CharacterReviveBroadcast, MakePacket<CharacterReviveBroadcast>);

        handlerDict.Add((ushort)EMessageID.ConnectedResponse, PacketHandler.HandleConnectedResponse);
        handlerDict.Add((ushort)EMessageID.LoginResponse, PacketHandler.HandleLoginResponse);
        handlerDict.Add((ushort)EMessageID.CreateCharacterResponse, PacketHandler.HandleCreateCharacterResponse);
        handlerDict.Add((ushort)EMessageID.CharacterEnterGameRoomResponse, PacketHandler.HandleCharacterEnterGameRoomResponse);
        handlerDict.Add((ushort)EMessageID.CharacterEnterGameRoomBroadcast, PacketHandler.HandleCharacterEnterGameRoomBroadcast);
        handlerDict.Add((ushort)EMessageID.CharacterLeftGameRoomResponse, PacketHandler.HandleCharacterLeftGameRoomResponse);
        handlerDict.Add((ushort)EMessageID.CharacterLeftGameRoomBroadcast, PacketHandler.HandleCharacterLeftGameRoomBroadcast);
        handlerDict.Add((ushort)EMessageID.ObjectSpawnedBroadcast, PacketHandler.HandleObjectSpawnedBroadcast);
        handlerDict.Add((ushort)EMessageID.ObjectDespawnedBroadcast, PacketHandler.HandleObjectDespawnedBroadcast);
        handlerDict.Add((ushort)EMessageID.UpdateCreatureStateBroadcast, PacketHandler.HandleUpdateCreatureStateBroadcast);
        handlerDict.Add((ushort)EMessageID.MoveResponse, PacketHandler.HandleMoveResponse);
        handlerDict.Add((ushort)EMessageID.MoveBroadcast, PacketHandler.HandleMoveBroadcast);
        handlerDict.Add((ushort)EMessageID.PerformAttackBroadcast, PacketHandler.HandlePerformAttackBroadcast);
        handlerDict.Add((ushort)EMessageID.HitBroadcast, PacketHandler.HandleHitBroadcast);
        handlerDict.Add((ushort)EMessageID.CreatureDeadBroadcast, PacketHandler.HandleCreatureDeadBroadcast);
        handlerDict.Add((ushort)EMessageID.CharacterReviveBroadcast, PacketHandler.HandleCharacterReviveBroadcast);
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
