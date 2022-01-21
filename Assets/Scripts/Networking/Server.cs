using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using Networking;
using UnityEngine;

public class Server : MonoBehaviour, INetEventListener
{
    private readonly int defaultPort = 7777;
    private readonly int defaultMaxPlayers = 1000;
    
    private NetManager _netManager;
    
    private NetPeer[] _peers;
    private NetDataWriter _dataWriter;
    private NetPacketProcessor _packetProcessor;
    
    
    [ContextMenu(nameof(StartServer))]
    public void StartServer()
    {
        StartServer(defaultPort, defaultMaxPlayers);
    }

    public void StartServer(int port, int maxPlayers)
    {
        _peers = new NetPeer[maxPlayers];
        
        _dataWriter = new NetDataWriter();
        _packetProcessor = new NetPacketProcessor();
        NetDataExtensions.RegisterDataTypes(_packetProcessor);
        
        _packetProcessor.SubscribeReusable<JoinPacket, NetPeer>(OnJoinReceived);
        
        _netManager = new NetManager(this)
        {
            BroadcastReceiveEnabled = true,
            AutoRecycle = true,
            UpdateTime = 15
        };
        _netManager.Start(port);
        
        Debug.Log($"{name} | Started on port: {port}");
    }
    
    private void Update()
    {
        _netManager?.PollEvents();
    }
    
    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log($"{name} | OnPeerConnected (IP: {peer.EndPoint})");
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log($"{name} | OnPeerDisconnected (IP: {peer.EndPoint}, {disconnectInfo})");
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        Debug.Log($"{name} | OnNetworkError (socketError: {socketError})");
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        Debug.Log($"{name} | OnNetworkReceive | bytes: {reader.AvailableBytes}");
        _packetProcessor.ReadAllPackets(reader, peer);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        //Debug.Log($"{name} | OnNetworkLatencyUpdate {peer.EndPoint} | latency: {latency}");
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        var netPeer = request.AcceptIfKey("LiteNetGame");
    }

    private void OnJoinReceived(JoinPacket joinPacket, NetPeer peer)
    {
        Debug.Log($"{name} | OnJoinReceived {joinPacket.nickname} (ID {peer.Id})");
    }
    
    public void SendPacket<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod) where T : ServerPacket, new()
    {
        _dataWriter.Reset();
        _packetProcessor.Write(_dataWriter, packet);
        peer.Send(_dataWriter, deliveryMethod);
    }
    
    public void SendPacketToAll<T>(T packet, DeliveryMethod deliveryMethod) where T : ServerPacket, new()
    {
        _dataWriter.Reset();
        _packetProcessor.Write(_dataWriter, packet);
        _netManager.SendToAll(_dataWriter, deliveryMethod);
    }
    
    public void SendPacketToAll<T>(T packet, DeliveryMethod deliveryMethod, NetPeer excludePeer) where T : ServerPacket, new()
    {
        _dataWriter.Reset();
        _packetProcessor.Write(_dataWriter, packet);
        _netManager.SendToAll(_dataWriter, deliveryMethod, excludePeer);
    }
    
}
