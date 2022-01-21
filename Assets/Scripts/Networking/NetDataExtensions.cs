using LiteNetLib.Utils;
using UnityEngine;

namespace Networking
{
    public static class NetDataExtensions
    {
        public static void RegisterDataTypes(NetPacketProcessor pc)
        {
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector2());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector3());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector4());
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetQuaternion());
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
        
        
        
    }
}