
namespace Networking
{
    public struct PlayerConnectionData
    {
        public ushort playerId;
        public ushort ping;
        public string nickname;
    }

    public struct PlayerPingInfo
    {
        public ushort playerId;
        public ushort ping;
    }
}
