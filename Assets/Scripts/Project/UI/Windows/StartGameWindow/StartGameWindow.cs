using Networking;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Project.UI.Windows.StartGameWindow
{
    public class StartGameWindow : Window
    {
        public const string prefabPath = "Prefabs/UI/Windows/StartGameWindow/Start Game Window";
        public const string defaultIp = "127.0.0.1";
        public const int defaultPort = 5555;
        public const int defaultMaxPlayers = 1000;
        
        [SerializeField] private Transform _clientPanel;
        [SerializeField] private Image _clientTabButtonImage;
        [SerializeField] private TextMeshProUGUI _clientInputIP;
        [SerializeField] private TextMeshProUGUI _clientInputPort;
        [SerializeField] private TextMeshProUGUI _clientInputNickname;
        
        [SerializeField] private Transform _serverPanel;
        [SerializeField] private Image _serverTabButtonImage;
        [SerializeField] private TextMeshProUGUI _serverInputPlayers;
        [SerializeField] private TextMeshProUGUI _serverInputPort;
        
        [SerializeField] private Transform _hostPanel;
        [SerializeField] private Image _hostTabButtonImage;
        [SerializeField] private TextMeshProUGUI _hostInputPlayers;
        [SerializeField] private TextMeshProUGUI _hostInputPort;
        [SerializeField] private TextMeshProUGUI _hostInputNickname;

        [SerializeField] private Color _unselectedTabColor;
        [SerializeField] private Color _selectedTabColor;

        private Transform _currentPanel;
        private Image _currentTabButtonImage;
        private NetMode _selectedNetMode;

        private void Awake()
        {
            SelectModeTab(NetMode.Client);
        }

        //unity event
        public void SelectModeTab(int modeTabId)
        {
            var mode = (NetMode)(modeTabId + 1);
            SelectModeTab(mode);
        }
        
        public void SelectModeTab(NetMode mode)
        {
            if (_currentPanel != null)
            {
                _currentPanel.gameObject.SetActive(false);
                _currentTabButtonImage.color = _unselectedTabColor;
            }
            
            switch (mode)
            {
                case NetMode.Client:
                    _currentPanel = _clientPanel;
                    _currentTabButtonImage = _clientTabButtonImage;
                    break;
                case NetMode.Server:
                    _currentPanel = _serverPanel;
                    _currentTabButtonImage = _serverTabButtonImage;
                    break;
                case NetMode.Host:
                    _currentPanel = _hostPanel;
                    _currentTabButtonImage = _hostTabButtonImage;
                    break;
            }

            if (_currentPanel != null)
            {
                _currentPanel.gameObject.SetActive(true);
                _currentTabButtonImage.color = _selectedTabColor;
            }
            _selectedNetMode = mode;
        }

        //unity event
        public void OnClickStartButton()
        {
            switch (_selectedNetMode)
            {
                case NetMode.Client:
                    StartClient();
                    break;
                case NetMode.Server:
                    StartServer();
                    break;
                case NetMode.Host:
                    StartHost();
                    break;
            }
        }

        private void StartClient()
        {
            string ipStr = _clientInputIP.text.Remove(_clientInputIP.text.Length - 1);
            if (string.IsNullOrWhiteSpace(ipStr))
            {
                ipStr = defaultIp;
            }

            string portStr = _clientInputPort.text.Remove(_clientInputPort.text.Length - 1);
            if (!int.TryParse(portStr, out var port))
                port = defaultPort;
            
            string nickname = _clientInputNickname.text.Remove(_clientInputNickname.text.Length - 1);
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 3 || nickname.Length > 24)
                nickname = "Player_" + Random.Range(1_000, 10_000);

            bool success = NetStarter.TryStartClient(ipStr, port, nickname);
            if (success)
            {
                Close();
            }
        }
        
        private void StartServer()
        {
            string portStr = _serverInputPort.text.Remove(_serverInputPort.text.Length - 1);
            if (!int.TryParse(portStr, out var port))
                port = defaultPort;
            
            string playersStr = _serverInputPlayers.text.Remove(_serverInputPlayers.text.Length - 1);
            if (!int.TryParse(playersStr, out var players) || players < 1 || players > NetInfo.maxPlayersLimit)
                players = defaultMaxPlayers;

            bool success = NetStarter.TryStartServer(port, players);
            if (success)
            {
                Close();
            }
        }
        
        private void StartHost()
        {
            string portStr = _hostInputPort.text.Remove(_hostInputPort.text.Length - 1);
            if (!int.TryParse(portStr, out var port))
                port = defaultPort;
            
            string playersStr = _hostInputPlayers.text.Remove(_hostInputPlayers.text.Length - 1);
            if (!int.TryParse(playersStr, out var players) || players < 1 || players > NetInfo.maxPlayersLimit) 
                players = defaultMaxPlayers;
            
            string nickname = _hostInputNickname.text.Remove(_hostInputNickname.text.Length - 1);;
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length < 3 || nickname.Length > 24)
                nickname = "Player_" + Random.Range(1_000, 10_000);

            bool success = NetStarter.TryStartHost(port, players, nickname);
            if (success)
            {
                Close();
            }
        }
        
        
        
        
    }
}
