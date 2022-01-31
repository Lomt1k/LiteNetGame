using Networking.Client;
using UnityEngine;
using TMPro;

namespace Project.UI.Windows.PlayersTabWindow
{
    public class PlayersTabItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerId;
        [SerializeField] private TextMeshProUGUI _nickname;
        [SerializeField] private TextMeshProUGUI _ping;

        public void SetData(ClientPlayer player)
        {
            _playerId.text = player.playerId.ToString();
            _ping.text = player.ping.ToString();
            _nickname.text = player.nickname;
        }

        public void UpdatePing(int ping)
        {
            _ping.text = ping.ToString();
        }


    }
}

