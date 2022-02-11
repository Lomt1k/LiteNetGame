using LiteNetLib.Utils;

namespace Networking.SendalbeDataTypes
{
    
    public static class NetDataExtensions
    {
        public static void RegisterDataTypes(NetPacketProcessor pc)
        {
            NetDataTypes_UnityEngine.RegisterDataTypes(pc);
            Connections.DataTypes.NetDataTypes_Connections.RegisterDataTypes(pc);
        }
        
        
    }
}