using Networking.Client;
using Networking.Server;

namespace Networking
{
    public static class NetInfo
    {
        public const string connectionKey = "LiteNetGame";

        public static bool isServer => GameServer.instance != null && GameServer.instance.isRunning;
        public static bool isClient => GameClient.instance != null && GameClient.instance.isStarted;
        public static bool isHost => isClient && isServer;

        public static ClientPlayer minePlayer => GameClient.instance?.players?.minePlayer;


    }
}
