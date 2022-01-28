using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using Project.UI.Windows;
using Project.UI.Windows.TextChatWindow;
using UnityEngine;

namespace Networking.Client
{
    using Sending;
    
    public class GameClient : MonoBehaviour, INetEventListener
    {
        public static GameClient instance { get; set; }
        
        private NetManager _netManager;
        private NetPeer _server;
        private ClientPacketSender _packetSender;
        private ClientPacketReceiver _packetReceiver;
        
        public ClientPlayers players { get; private set; }
        public ClientPacketSender sender => _packetSender;

        public bool isStarted => _netManager != null;
        public ConnectionState connectionState => _server?.ConnectionState ?? ConnectionState.Disconnected;

        private void Awake()
        {
            instance = this;
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
            InitializePacketProcessor();
            InitializeClientGlobalManagers();
            TextChatWindow.instance.AddMessage($"Client started");
        }
        
        private void InitializePacketProcessor()
        {
            var packetProcessor = new NetPacketProcessor();
            NetDataExtensions.RegisterDataTypes(packetProcessor);
            _packetSender = new ClientPacketSender(_netManager, packetProcessor);
            _packetReceiver = new ClientPacketReceiver(packetProcessor);
        }

        private void InitializeClientGlobalManagers()
        {
            WindowsManager.CreateWindow<TextChatWindow>(Layer.InGameHUD);
        }
        
        private void Update()
        {
            _netManager?.PollEvents();
        }

        public void Connect(IPEndPoint endPoint)
        {
            if (_netManager == null)
            {
                StartClient();
            }
            TextChatWindow.instance.AddMessage($"Connecting to <color=grey>{endPoint}</color>");
            _netManager.Connect(endPoint, NetInfo.connectionKey);
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
        
        public void OnPeerConnected(NetPeer peer)
        {
            _server = peer;
            ClientSending_Connections.SendJoinServerPacket();
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

        public void CreatePlayersList(int maxPlayers)
        {
            players = new ClientPlayers(maxPlayers);
        }
        
        
    }
}
