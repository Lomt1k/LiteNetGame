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
        private bool _isInitialized;
        private bool _currentVisibleState;
        private float _lastRequestPingTime;

        private ClientPlayers _clientPlayers;

        private System.Action _onInitialized;

        public override void OnCreated()
        {
            base.OnCreated();
            instance = this;
            _clientPlayers = GameClient.instance.players;
            _playerItems = new PlayersTabItem[_clientPlayers.maxPlayers];
            Initialize();
            RefreshPlayersOnline();
            SetVisible(false);
        }

        public void Initialize()
        {
            gameObject.SetActive(false);
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
            gameObject.SetActive(true);
            _isInitialized = true;
            
            _onInitialized?.Invoke();
            _onInitialized = null;
        }

        public void AddPlayerToTab(int playerId)
        {
            RefreshPlayersOnline();
            if (_isInitialized)
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
            if (_isInitialized)
            {
                _playerItems[playerId].gameObject.SetActive(false);
            }
            else _onInitialized += () => _playerItems[playerId].gameObject.SetActive(false);
        }

        public void RefreshPing(ClientPlayer player)
        {
            if (_isInitialized)
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

            if (Time.unscaledDeltaTime - _lastRequestPingTime > requestPingFrequency)
            {
                _lastRequestPingTime = Time.unscaledDeltaTime;
                RequestUpdatePlayersPingInfo();
            }
        }

        private void SetVisible(bool state)
        {
            _canvasGroup.alpha = state ? 1 : 0;
            _canvasGroup.blocksRaycasts = state;
            _canvasGroup.interactable = state;
            _currentVisibleState = state;
        }

        private static void RequestUpdatePlayersPingInfo()
        {
            Networking.Connections.Client.ClientSending_Connections.RequestPlayerPings();
        }

        private void OnDestroy()
        {
            _onInitialized = null;
        }
    }
}

