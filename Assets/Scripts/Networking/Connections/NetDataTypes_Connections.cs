using LiteNetLib.Utils;

namespace Networking.Connections.DataTypes
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

    public static class NetDataTypes_Connections
    {
        public static void RegisterDataTypes(NetPacketProcessor pc)
        {
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetPlayerConnectionData());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetPlayerConnectionDataArray());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetPlayerPingInfo());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetPlayerPingInfoArray());
        }


        #region PlayerConnectionData

        // PlayerConnectionData
        public static void Put(this NetDataWriter writer, PlayerConnectionData connectionData)
        {
            writer.Put(connectionData.playerId);
            writer.Put(connectionData.ping);
            writer.Put(connectionData.nickname);
        }
        
        public static PlayerConnectionData GetPlayerConnectionData(this NetDataReader reader)
        {
            return new PlayerConnectionData
            {
                playerId = reader.GetUShort(),
                ping = reader.GetUShort(),
                nickname = reader.GetString()
            };
        }
        
        // PlayerConnectionData[]
        public static void Put(this NetDataWriter writer, PlayerConnectionData[] dataArray)
        {
            writer.Put(dataArray.Length);
            foreach (var data in dataArray)
            {
                writer.Put(data);
            }
        }
        
        public static PlayerConnectionData[] GetPlayerConnectionDataArray(this NetDataReader reader)
        {
            int size = reader.GetInt();
            var resultArray = new PlayerConnectionData[size];
            for (int i = 0; i < size; i++)
            {
                var data = new PlayerConnectionData
                {
                    playerId = reader.GetUShort(),
                    ping = reader.GetUShort(),
                    nickname = reader.GetString()
                };
                resultArray[i] = data;
            }
            return resultArray;
        }

        #endregion

        #region PlayerPingInfo

        // PlayerPingInfo
        public static void Put(this NetDataWriter writer, PlayerPingInfo pingInfo)
        {
            writer.Put(pingInfo.playerId);
            writer.Put(pingInfo.ping);
        }
        
        public static PlayerPingInfo GetPlayerPingInfo(this NetDataReader reader)
        {
            return new PlayerPingInfo
            {
                playerId = reader.GetUShort(),
                ping = reader.GetUShort()
            };
        }
        
        // PlayerPingInfo[]
        public static void Put(this NetDataWriter writer, PlayerPingInfo[] pingInfoArray)
        {
            writer.Put(pingInfoArray.Length);
            foreach (var pingInfo in pingInfoArray)
            {
                writer.Put(pingInfo);
            }
        }
        
        public static PlayerPingInfo[] GetPlayerPingInfoArray(this NetDataReader reader)
        {
            int size = reader.GetInt();
            var resultArray = new PlayerPingInfo[size];
            for (int i = 0; i < size; i++)
            {
                var data = new PlayerPingInfo
                {
                    playerId = reader.GetUShort(),
                    ping = reader.GetUShort()
                };
                resultArray[i] = data;
            }
            return resultArray;
        }

        #endregion
        
        
        
        
    }
}
