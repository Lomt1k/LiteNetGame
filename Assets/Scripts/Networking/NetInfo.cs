namespace Networking
{
    using Client;
    using Server;
    
    public enum NetMode : byte
    {
        None,
        Client,
        Server,
        Host
    }
    
    public static class NetInfo
    {
        public const string connectionKey = "LiteNetGame";
        public const int maxPlayersLimit = ushort.MaxValue; //во всех пакетах ID игроков отсылается в ushort (2 байта)
        public const byte minNicknameLength = 3;
        public const byte maxNicknameLength = 24;

        public static NetMode mode { get; private set; } = NetMode.None;

        public static bool isServer => mode == NetMode.Server || mode == NetMode.Host;
        public static bool isClient => mode == NetMode.Client || mode == NetMode.Host;
        public static bool isHost => mode == NetMode.Host;
        
        public static ClientPlayer minePlayer => GameClient.instance?.players?.minePlayer;

        public static void SetMode(NetMode _mode)
        {
            mode = _mode;
        }


    }
}
