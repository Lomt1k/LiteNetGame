using LiteNetLib.Utils;
using UnityEngine;

namespace Networking
{
    public static class NetDataExtensions
    {
        public static void RegisterDataTypes(NetPacketProcessor pc)
        {
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector2());
            pc.RegisterNestedType<Vector3>((w, v) => w.Put(v), reader => reader.GetVector3());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector4());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetQuaternion());
            
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetPlayerConnectionData());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetPlayerConnectionDataArray());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetPlayerPingInfo());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetPlayerPingInfoArray());
        }
        
        // Vector2
        public static void Put(this NetDataWriter writer, Vector2 vector)
        {
            writer.Put(vector.x);
            writer.Put(vector.y);
        }

        public static Vector2 GetVector2(this NetDataReader reader)
        {
            return new Vector2
            {
                x = reader.GetFloat(),
                y = reader.GetFloat()
            };
        }
        
        // Vector3
        public static void Put(this NetDataWriter writer, Vector3 vector)
        {
            writer.Put(vector.x);
            writer.Put(vector.y);
            writer.Put(vector.z);
        }

        public static Vector3 GetVector3(this NetDataReader reader)
        {
            return new Vector3
            {
                x = reader.GetFloat(),
                y = reader.GetFloat(),
                z = reader.GetFloat()
            };
        }
        
        // Vector4
        public static void Put(this NetDataWriter writer, Vector4 vector)
        {
            writer.Put(vector.x);
            writer.Put(vector.y);
            writer.Put(vector.z);
            writer.Put(vector.w);
        }

        public static Vector4 GetVector4(this NetDataReader reader)
        {
            return new Vector4
            {
                x = reader.GetFloat(),
                y = reader.GetFloat(),
                z = reader.GetFloat(),
                w = reader.GetFloat()
            };
        }
        
        // Quaternion
        public static void Put(this NetDataWriter writer, Quaternion quaternion)
        {
            writer.Put(quaternion.x);
            writer.Put(quaternion.y);
            writer.Put(quaternion.z);
            writer.Put(quaternion.w);
        }

        public static Quaternion GetQuaternion(this NetDataReader reader)
        {
            return new Quaternion
            {
                x = reader.GetFloat(),
                y = reader.GetFloat(),
                z = reader.GetFloat(),
                w = reader.GetFloat()
            };
        }
        
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
        
        
    }
}