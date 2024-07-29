using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Net;
using ServerCore;

public class NetworkManager
{
    #region Variables

    private ServerSession session = new();

    #endregion Variables

    #region Methods

    public void Init()
    {
        IPAddress ipAddr = IPAddress.Parse(GlobalDefine.IP_ADDRESS);
        IPEndPoint endPoint = new(ipAddr, GlobalDefine.PORT_NUMBER);

        Connector connector = new();
        connector.Connect(endPoint, () => session);
    }

    public void Update()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in list)
        {
            if (PacketManager.Instance.TryGetPacketHandler(packet.ID, out Action<PacketSession, IMessage> handler) == false) continue;

            handler.Invoke(session, packet.Message);
        }
    }

    public void Send(ArraySegment<byte> sendBuff)
    {
        session.Send(sendBuff);
    }

    #endregion Methods
}
