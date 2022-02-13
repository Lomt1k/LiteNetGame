namespace Networking
{
    using Client;
    
    public enum NetMode : byte
    {
        None = 0,
        Client = 1,
        Server = 2,
        Host = 3
    }
    
    public static class NetInfo
    {
        public const string connectionKey = "LiteNetGame";
        public const int maxPlayersLimit = ushort.MaxValue; //во всех пакетах ID игроков отсылается в ushort (2 байта)
        public const byte minNicknameLength = 3;
        public const byte maxNicknameLength = 24;

        private static NetMode _mode = NetMode.None;

        public static bool isServer => _mode == NetMode.Server || _mode == NetMode.Host;
        public static bool isClient => _mode == NetMode.Client || _mode == NetMode.Host;
        public static bool isHost => _mode == NetMode.Host;
        
        public static Server.ServerConfig serverConfig => Server.ServerConfig.defaultConfig;
        
        public static ClientPlayer minePlayer => GameClient.instance?.players?.minePlayer;
        public static Project.Units.Client.ClientUnit mineUnit => minePlayer?.unit;

        public static void SetMode(NetMode mode)
        {
            _mode = mode;
        }


    }
}
