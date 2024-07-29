using Google.Protobuf;
using System.Collections.Generic;

public struct PacketMessage
{ 
    public ushort ID { set; get; }
    public IMessage Message { set; get; }
}

public class PacketQueue
{
    #region Variables

    private object lockObj = new();

    private Queue<PacketMessage> packetQueue = new();
    private List<PacketMessage> packetList = new();

    #endregion Variables

    #region Properties

    public static PacketQueue Instance { get; } = new();

    #endregion Properties

    #region Methods

    public void Push(ushort id, IMessage packet)
    {
        lock (lockObj)
        {
            packetQueue.Enqueue(new PacketMessage() { ID = id, Message = packet });
        }
    }

    public PacketMessage? Pop()
    {
        lock (lockObj)
        {
            if (packetQueue.Count == 0) return null;

            return packetQueue.Dequeue();
        }
    }

    public List<PacketMessage> PopAll()
    {
        packetList.Clear();

        lock (lockObj)
        {
            while (packetQueue.Count > 0)
            {
                packetList.Add(packetQueue.Dequeue());
            }
        }

        return packetList;
    }

    #endregion Methods
}
