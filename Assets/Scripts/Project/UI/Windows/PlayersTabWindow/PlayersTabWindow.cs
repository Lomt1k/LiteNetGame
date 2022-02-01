using Networking;
using Networking.Client;
using TMPro;
using UnityEngine;

namespace Project.UI.Windows.PlayersTabWindow
{
    public class PlayersTabWindow : Window
    {
        public const string prefabPath = "Prefabs/UI/Windows/PlayersTabWindow/PlayersTabWindow";
        public const float requestPingFrequency = 5f;
        public static PlayersTabWindow instance;

        [SerializeField] private PlayersTabItem _itemPrefab;
        [SerializeField] private Transform _playersTabContent;
        [SerializeField] private TextMeshProUGUI _playersOnlineCounter;
        [SerializeField] private CanvasGroup _canvasGroup;

        private PlayersTabItem[] _playerItems;
        private bool _isTabInitializationStarted;
        private bool _isTabInitialized;
        private bool _currentVisibleState;
        private float _currentRequestPingTime;

        private ClientPlayers _clientPlayers;

        private System.Action _onInitialized;

        public override void OnCreated()
        {
            base.OnCreated();
            instance = this;
            _clientPlayers = GameClient.instance.players;
            _playerItems = new PlayersTabItem[_clientPlayers.maxPlayers];
            RefreshPlayersOnline();
            SetVisible(false);
        }

        //TODO позже обязательно перенести инициализацию на момент загрузочного экрана! Жрет много!
        public void Initialize()
        {
            _playersTabContent.gameObject.SetActive(false);
            for (int i = 0; i < _playerItems.Length; i++)
            {
                var tabItem = Instantiate(_itemPrefab, _playersTabContent).GetComponent<PlayersTabItem>();
                if (_clientPlayers[i] != null)
                {
                    tabItem.SetData(_clientPlayers[i]);
                    tabItem.gameObject.SetActive(true);
                }
                else
                {
                    tabItem.gameObject.SetActive(false);
                }
                _playerItems[i] = tabItem;
            }

            var minePlayerId = NetInfo.minePlayer.playerId;
            _playerItems[minePlayerId].transform.SetAsFirstSibling();
            _playersTabContent.gameObject.SetActive(true);
            _isTabInitialized = true;
            
            _onInitialized?.Invoke();
            _onInitialized = null;
        }

        public void AddPlayerToTab(int playerId)
        {
            RefreshPlayersOnline();
            if (_isTabInitialized)
            {
                _playerItems[playerId].SetData(_clientPlayers[playerId]);
                _playerItems[playerId].gameObject.SetActive(true);
            }
            else
                _onInitialized += () =>
                {
                    _playerItems[playerId].SetData(_clientPlayers[playerId]);
                    _playerItems[playerId].gameObject.SetActive(true);
                };
        }

        public void RemovePlayerFromTab(int playerId)
        {
            RefreshPlayersOnline();
            if (_isTabInitialized)
            {
                _playerItems[playerId].gameObject.SetActive(false);
            }
            else _onInitialized += () => _playerItems[playerId].gameObject.SetActive(false);
        }

        public void RefreshPing(ClientPlayer player)
        {
            if (_isTabInitialized)
            {
                _playerItems[player.playerId].UpdatePing(player.ping);
            }
            else _onInitialized += () => _playerItems[player.playerId].UpdatePing(player.ping);
        }

        private void RefreshPlayersOnline()
        {
            _playersOnlineCounter.text = _clientPlayers.playersOnline.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SetVisible(!_currentVisibleState);
            }

            if (!_currentVisibleState)
                return;

            _currentRequestPingTime -= Time.unscaledDeltaTime;
            if (_currentRequestPingTime < float.Epsilon)
            {
                RequestUpdatePlayersPingInfo();
                _currentRequestPingTime = requestPingFrequency;
            }
        }

        private void SetVisible(bool state)
        {
            _canvasGroup.alpha = state ? 1 : 0;
            _canvasGroup.blocksRaycasts = state;
            _canvasGroup.interactable = state;
            _currentVisibleState = state;
            
            //TODO перенести инициализацию на момент создания окна, а окно создавать в окне загрузки
            if (state && !_isTabInitializationStarted)
            {
                _isTabInitializationStarted = true;
                Initialize();
            }
        }

        private static void RequestUpdatePlayersPingInfo()
        {
            Networking.Client.Sending.ClientSending_Connections.RequestPlayerPings();
        }

        private void OnDestroy()
        {
            _onInitialized = null;
        }
    }
}

