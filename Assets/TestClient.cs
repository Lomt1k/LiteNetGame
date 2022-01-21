using System;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

public class TestClient : MonoBehaviour, INetEventListener
{
    private NetManager _netManager;
    
    private NetDataWriter _dataWriter;
    private NetDataReader _dataReader;

    private void Start()
    {
        StartClient();

        _dataWriter = new NetDataWriter();
        _dataReader = new NetDataReader();
    }

    private void StartClient()
    {
        _netManager = new NetManager(this)
        {
            UnconnectedMessagesEnabled = true,
            UpdateTime = 15
        };
        _netManager.Start();
    }

    [ContextMenu(nameof(ConnectLocalhost))]
    public void ConnectLocalhost()
    {
        var ip = IPAddress.Parse("127.0.0.1");
        int port = 7777;
        var endPoint = new IPEndPoint(ip, port);
        Connect(endPoint);
    }

    public void Connect(IPEndPoint endPoint)
    {
        Debug.Log($"{name} | Connecting to {endPoint}");
        _netManager.Connect(endPoint, "LiteNetGame");
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        Debug.Log($"{name} | OnNetworkError (socketError: {socketError})");
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
    }
    
    public void OnPeerConnected(NetPeer peer)
    {
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
    }
}
