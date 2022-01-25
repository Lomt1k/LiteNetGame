using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

namespace Networking.Server
{
    public class GameServer : MonoBehaviour, INetEventListener
    {
        private readonly int defaultPort = 7777;
        private readonly int defaultMaxPlayers = 1000;
        
        private NetManager _netManager;
        private ServerPacketSender _packetSender;
        private ServerPacketReceiver _packetReceiver;

        public ServerPlayers players { get; private set; }
        
        
        public static GameServer instance { get; set; }

        public bool isRunning => _netManager != null && _netManager.IsRunning;
        
        
        [ContextMenu(nameof(StartServer))]
        public void StartServer()
        {
            StartServer(defaultPort, defaultMaxPlayers);
        }

        public void StartServer(int port, int maxPlayers)
        {
            instance = this;
            players = new ServerPlayers(maxPlayers);
            _netManager = new NetManager(this)
            {
                BroadcastReceiveEnabled = true,
                AutoRecycle = true,
                UpdateTime = 15
            };
            _netManager.Start(port);
            InitializePacketProcessor();

            Debug.Log($"{name} | Started on port: {port} maxPlayers: {maxPlayers}");
        }

        private void InitializePacketProcessor()
        {
            var packetProcessor = new NetPacketProcessor();
            NetDataExtensions.RegisterDataTypes(packetProcessor);
            _packetSender = new ServerPacketSender(_netManager, packetProcessor);
            _packetReceiver = new ServerPacketReceiver(packetProcessor);
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
            _packetReceiver.OnNetworkReceive(peer, reader, deliveryMethod);
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
            var netPeer = request.AcceptIfKey(NetInfo.connectionKey);
        }
        
    }
}
