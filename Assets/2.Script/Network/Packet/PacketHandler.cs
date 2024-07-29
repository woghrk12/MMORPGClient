using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;

public class PacketHandler
{
    // Implement the methods for handling the packets that the client needs.
    // Example
    // {0} : The class name of the packet.
    //public static void Handle{0}(PacketSession session, IMessage message)
    //{
    //    throw new NotImplementedException();
    //}

    public static void HandlePlayerMoveBrodcast(PacketSession session, IMessage message)
    {
        PlayerMoveBrodcast packet = message as PlayerMoveBrodcast;
        ServerSession serverSession = session as ServerSession;

        Debug.Log(packet.PlayerID + " : " + packet.Direction);
    }
}
