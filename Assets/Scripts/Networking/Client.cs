using System;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using Networking;
using UnityEngine;

public class Client : MonoBehaviour, INetEventListener
{
    private NetManager _netManager;
    private NetPeer _server;
    private NetDataWriter _dataWriter;
    private NetPacketProcessor _packetProcessor;

    private void Awake()
    {
        StartClient();

        _dataWriter = new NetDataWriter();
        _packetProcessor = new NetPacketProcessor();
        NetDataExtensions.RegisterDataTypes(_packetProcessor);
    }

    private void StartClient()
    {
        _netManager = new NetManager(this)
        {
            UnconnectedMessagesEnabled = true,
            AutoRecycle = true,
            UpdateTime = 15
        };
        _netManager.Start();
    }
    
    private void Update()
    {
        _netManager?.PollEvents();
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
        Debug.Log($"{name} | OnNetworkReceive | bytes: {reader.AvailableBytes}");
        _packetProcessor.ReadAllPackets(reader, peer);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
    }
    
    public void OnPeerConnected(NetPeer peer)
    {
        _server = peer;
        Debug.Log($"{name} | Connected to server");
        
        SendPacket(new JoinPacket {nickname = "Lomt1k"}, DeliveryMethod.ReliableOrdered);
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        request.Reject();
    }
    
    public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : ClientPacket, new()
    {
        _dataWriter.Reset();
        _packetProcessor.Write(_dataWriter, packet);
        _server.Send(_dataWriter, deliveryMethod);
    }
}
