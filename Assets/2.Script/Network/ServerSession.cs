using ServerCore;
using System;
using System.Net;
using UnityEngine;

public class ServerSession : PacketSession
{
    #region Methods

    #region Events

    public override void OnConnected(EndPoint endPoint)
    {
        Debug.Log($"OnConnected : {endPoint}");
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        Debug.Log($"OnDisconnected : {endPoint}");
    }

    public override void OnRecvPacket(ArraySegment<byte> buffer)
    {
        PacketManager.Instance.OnReceivePacket(this, buffer);
    }

    public override void OnSend(int numOfBytes)
    {

    }

    #endregion Events

    #endregion Methods
}
