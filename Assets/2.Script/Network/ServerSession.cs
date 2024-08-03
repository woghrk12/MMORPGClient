using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ServerSession
{
    #region Variables

    private static readonly int HEADER_SIZE = 2;

    private Socket socket = null;
    private int isDisconnected = 0;

    private object lockObj = new();

    private SocketAsyncEventArgs sendArgs = new();
    private Queue<ArraySegment<byte>> sendQueue = new();
    private List<ArraySegment<byte>> pendingList = new();

    private SocketAsyncEventArgs recvArgs = new();
    private RecvBuffer recvBuffer = new RecvBuffer(65535);

    #endregion Variables

    #region Methods

    public void Init(Socket socket)
    {
        this.socket = socket;

        recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);
        sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

        OnConnected(socket.RemoteEndPoint);

        RegisterRecv();
    }

    public void Disconnect()
    {
        if (Interlocked.Exchange(ref isDisconnected, 1) == 1) return;

        OnDisconnected(socket.RemoteEndPoint);

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();

        Clear();
    }

    public void Send(IMessage packet)
    {
        EMessageID packetID = (EMessageID)Enum.Parse(typeof(EMessageID), packet.Descriptor.Name);
        ushort size = (ushort)packet.CalculateSize();

        byte[] sendBuffer = new byte[size + 4];

        Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
        Array.Copy(BitConverter.GetBytes((ushort)packetID), 0, sendBuffer, 2, sizeof(ushort));
        Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

        ArraySegment<byte> segment = new ArraySegment<byte>(sendBuffer);

        lock (lockObj)
        {
            sendQueue.Enqueue(segment);

            if (pendingList.Count == 0)
            {
                RegisterSend();
            }
        }
    }

    private void Clear()
    {
        lock (lockObj)
        {
            sendQueue.Clear();
            pendingList.Clear();
        }
    }

    private void RegisterRecv()
    {
        if (isDisconnected == 1) return;

        recvBuffer.Clear();

        ArraySegment<byte> segment = recvBuffer.WriteSegment;
        recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

        try
        {
            if (socket.ReceiveAsync(recvArgs) == true) return;

            OnReceiveCompleted(null, recvArgs);
        }
        catch (Exception e)
        {
            Debug.LogError($"RegisterRecv Failed. {e}");
        }
    }

    private void RegisterSend()
    {
        if (isDisconnected == 1) return;

        while (sendQueue.Count > 0)
        {
            pendingList.Add(sendQueue.Dequeue());
        }

        sendArgs.BufferList = pendingList;

        try
        {
            if (socket.SendAsync(sendArgs) == true) return;

            OnSendCompleted(null, sendArgs);
        }
        catch (Exception e)
        {
            Debug.LogError($"RegisterSend Failed. {e}");
        }
    }

    #region Events

    private void OnConnected(EndPoint endPoint)
    {
        Debug.Log($"OnConnected : {endPoint}");
    }

    private void OnDisconnected(EndPoint endPoint)
    {
        Debug.Log($"OnDisconnected : {endPoint}");

        recvArgs.Completed -= new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);
        sendArgs.Completed -= new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
    }

    private int OnReceive(ArraySegment<byte> buffer)
    {
        int processLen = 0;

        while (true)
        {
            if (buffer.Count < HEADER_SIZE) break;

            ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);

            if (buffer.Count < dataSize) break;

            PacketManager.Instance.OnReceivePacket(this, new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));

            processLen += dataSize;
            buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
        }

        return processLen;
    }

    private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
    {
        if (args.BytesTransferred == 0 || args.SocketError != SocketError.Success)
        {
            Debug.LogError($"OnReceiveCompleted Failed.\nBytes Transferred : {args.BytesTransferred}.\nState : {args.SocketError}");
            Disconnect();
            return;
        }

        try
        {
            if (recvBuffer.OnWrite(args.BytesTransferred) == false)
            {
                Disconnect();
                return;
            }

            int processLen = OnReceive(recvBuffer.ReadSegment);

            if (processLen < 0 || processLen > recvBuffer.DataSize)
            {
                Disconnect();
                return;
            }

            if (recvBuffer.OnRead(processLen) == false)
            {
                Disconnect();
                return;
            }

            RegisterRecv();
        }
        catch (Exception e)
        {
            Debug.LogError($"OnReceiveCompleted Failed. {e}");
        }
    }

    private void OnSend(int numOfBytes)
    { 
    
    }

    private void OnSendCompleted(object sender, SocketAsyncEventArgs args)
    {
        lock (lockObj)
        {
            if (args.BytesTransferred == 0 || args.SocketError != SocketError.Success)
            {
                Debug.LogError($"OnSendCompleted Failed.\nBytes Transferred : {args.BytesTransferred}.\nState : {args.SocketError}");
                Disconnect();
                return;
            }

            try
            {
                sendArgs.BufferList = null;
                pendingList.Clear();

                OnSend(args.BytesTransferred);

                if (sendQueue.Count > 0)
                {
                    RegisterSend();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"OnSendCompleted Failed. {e}");
            }
        }
    }

    #endregion Events

    #endregion Methods
}
