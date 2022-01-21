using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

public class TestServer : MonoBehaviour, INetEventListener
{
    private readonly int defaultPort = 7777;
    private readonly int defaultMaxPlayers = 1000;
    
    private NetManager _netManager;
    
    private NetPeer[] _peers;
    private NetDataReader _dataReader;
    private NetDataWriter _dataWriter;
    
    
    [ContextMenu(nameof(StartServer))]
    public void StartServer()
    {
        StartServer(defaultPort, defaultMaxPlayers);
    }

    public void StartServer(int port, int maxPlayers)
    {
        _peers = new NetPeer[maxPlayers];
        _dataReader = new NetDataReader();
        _dataWriter = new NetDataWriter();
        
        _netManager = new NetManager(this)
        {
            BroadcastReceiveEnabled = true,
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
        Debug.Log($"{name} | OnNetworkReceive");
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        Debug.Log($"{name} | OnNetworkReceiveUnconnected");
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        //Debug.Log($"{name} | OnNetworkLatencyUpdate {peer.EndPoint} | latency: {latency}");
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        Debug.Log($"{name} | OnConnectionRequest from {request.RemoteEndPoint}");
        var netPeer = request.AcceptIfKey("LiteNetGame");
    }
    
}
