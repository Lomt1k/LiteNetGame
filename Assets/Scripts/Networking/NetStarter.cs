using System.Net;
using Networking.Client;
using Networking.Server;
using UnityEngine;

namespace Networking
{
    public static class NetStarter
    {
        public const string localhost = "127.0.0.1";
        
        public static bool TryStartServer(int port, int maxPlayers)
        {
            if (maxPlayers < 1 || maxPlayers > NetInfo.maxPlayersLimit)
            {
                Debug.LogError($"Установлен некорректный лимит игроков (требуется от 1 до {NetInfo.maxPlayersLimit})");
                return false;
            }
            if (GameServer.instance == null)
            {
                Debug.LogError($"На сцене отсутствует объект с компонентом {nameof(GameServer)}");
                return false;
            }
            
            GameServer.instance.StartServer(port, maxPlayers);
            NetInfo.SetMode(NetMode.Server);
            return true;
        }

        public static bool TryStartClient(string ipString, int port, string nickname)
        {
            if (!IPAddress.TryParse(ipString, out var ipAddress))
            {
                Debug.LogError("IP адрес введен некорректно");
                return false;
            }
            if (nickname.Length > NetInfo.maxNicknameLength || nickname.Length < NetInfo.minNicknameLength)
            {
                Debug.LogError($"Некорретный никнейм (длина должна быть " +
                               $"от {NetInfo.minNicknameLength} до {NetInfo.maxNicknameLength} символов)");
                return false;
            }
            if (GameServer.instance == null)
            {
                Debug.LogError($"На сцене отсутствует объект с компонентом {nameof(GameClient)}");
                return false;
            }
            
            var endPoint = new IPEndPoint(ipAddress, port);
            GameClient.instance.Connect(endPoint, nickname);
            NetInfo.SetMode(NetMode.Client);
            return true;
        }
        
        public static bool TryStartHost(int port, int maxPlayers, string nickname)
        {
            if (!TryStartServer(port, maxPlayers))
                return false;
            if (!TryStartClient(localhost, port, nickname))
                return false;
            
            NetInfo.SetMode(NetMode.Host);
            return true;
        }
        
        
    }
}
