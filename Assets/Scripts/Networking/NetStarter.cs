using System.Net;
using Networking.Client;
using Networking.Server;
using UnityEngine;

namespace Networking
{
    public static class NetStarter
    {
        public const string localhost = "127.0.0.1";
        
        public static void StartServer(int port, int maxPlayers)
        {
            if (maxPlayers < 1 || maxPlayers > NetInfo.maxPlayersLimit)
            {
                Debug.LogError($"Установлен некорректный лимит игроков (требуется от 1 до {NetInfo.maxPlayersLimit})");
                return;
            }
            if (GameServer.instance == null)
            {
                Debug.LogError($"На сцене отсутствует объект с компонентом {nameof(GameServer)}");
                return;
            }
            
            GameServer.instance.StartServer(port, maxPlayers);
            NetInfo.SetMode(NetMode.Server);
        }

        public static void StartClient(string ipString, int port, string nickname)
        {
            if (!IPAddress.TryParse(ipString, out var ipAddress))
            {
                Debug.LogError("IP адрес введен некорректно");
                return;
            }
            if (nickname.Length > NetInfo.maxNicknameLength || nickname.Length < NetInfo.minNicknameLength)
            {
                Debug.LogError($"Некорретный никнейм (длина должна быть " +
                               $"от {NetInfo.minNicknameLength} до {NetInfo.maxNicknameLength} символов)");
                return;
            }
            if (GameServer.instance == null)
            {
                Debug.LogError($"На сцене отсутствует объект с компонентом {nameof(GameClient)}");
                return;
            }
            
            var endPoint = new IPEndPoint(ipAddress, port);
            GameClient.instance.Connect(endPoint);
            NetInfo.SetMode(NetMode.Client);
        }
        
        public static void StartHost(int port, int maxPlayers, string nickname)
        {
            StartServer(port, maxPlayers);
            StartClient(localhost, port, nickname);
            NetInfo.SetMode(NetMode.Host);
        }
        
        
    }
}
