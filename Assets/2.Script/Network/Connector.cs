using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Connector
{
    #region Variables

    private Func<ServerSession> sessionFactory = null;

    #endregion Variables

    #region Methods

    public void Connect(IPEndPoint endPoint, Func<ServerSession> sessionFactory)
    {
        Socket socket = new(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        SocketAsyncEventArgs args = new();

        args.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectCompleted);
        args.RemoteEndPoint = endPoint;
        args.UserToken = socket;

        this.sessionFactory += sessionFactory;

        RegisterConnect(args);
    }

    public void RegisterConnect(SocketAsyncEventArgs args)
    {
        Socket socket = args.UserToken as Socket;

        if (ReferenceEquals(socket, null) == true) return;

        if (socket.ConnectAsync(args) == true) return;

        OnConnectCompleted(null, args);
    }

    public void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
    {
        if (args.SocketError != SocketError.Success)
        {
            Debug.LogError($"OnConnected Failed. {args.SocketError}");
            return;
        }

        ServerSession session = sessionFactory.Invoke();
        session.Init(args.ConnectSocket);
    }

    #endregion Methods
}