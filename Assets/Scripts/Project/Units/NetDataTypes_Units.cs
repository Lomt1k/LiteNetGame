using LiteNetLib.Utils;
using Project.Units.Client;
using Utils;

namespace Project.Units.DataTypes
{
    public struct UnitStateInfo
    {
        public float speed;
        public byte bitArray_0;
    }

    public static class NetDataTypes_Units
    {
        public static void RegisterDataTypes(NetPacketProcessor pc)
        {
            pc.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetUnitStateInfo());
        }
        
        #region UnitStateInfo

        // UnitStateInfo
        public static void Put(this NetDataWriter writer, UnitStateInfo unitStateInfo)
        {
            writer.Put(unitStateInfo.speed);
            writer.Put(unitStateInfo.bitArray_0);
        }

        public static UnitStateInfo GetUnitStateInfo(this NetDataReader reader)
        {
            return new UnitStateInfo
            {
                speed = reader.GetFloat(),
                bitArray_0 = reader.GetByte()
            };
        }

        public static UnitStateInfo GetStateInfoFromUnit(ClientUnit unit)
        {
            var basicBehaviour = unit.basicBehaviour;
            var animator = unit.animator;
            float speed = basicBehaviour.GetAnim.GetFloat(AnimatorHashes.hashSpeed);
            
            // --- maximum 8 booleans (0 - 7 bits) in 1 byte!
            byte bitArray_0 = ByteHelper.PackBooleansToByte
            (
                basicBehaviour.GetH > 0f, // 0 - isPositiveHorizontal
                basicBehaviour.GetH < 0f, // 1 - isNegativeHorizontal
                basicBehaviour.GetV > 0f, // 2 - isPositiveVertical
                basicBehaviour.GetV < 0f, // 3 - isNegativeVertical,
                animator.GetBool(AnimatorHashes.hashGrounded), // 4 - isGrounded
                animator.GetBool(AnimatorHashes.hashJump), // 5 - isJumping,
                animator.GetBool(AnimatorHashes.hashAim), // 6 - isAiming
                animator.GetBool(AnimatorHashes.hashFly) // 7 - isFlying
            );

            return new UnitStateInfo
            {
                speed = speed,
                bitArray_0 = bitArray_0
            };
        }

        public static void ApplyStateInfoToUnit(UnitStateInfo info, ClientUnit unit)
        {
            var animator = unit.animator;
            animator.SetFloat(AnimatorHashes.hashSpeed, info.speed);

            bool[] bits = ByteHelper.GetBooleansFromByte(info.bitArray_0);
            // 0 - isPositiveHorizontal | 1 - isNegativeHorizontal
            animator.SetFloat(AnimatorHashes.hashHorizontal, bits[0] ? 1f : bits[1] ? -1f : 0f);
            // 2 - isPositiveVertical | 3 - isNegativeVertical
            animator.SetFloat(AnimatorHashes.hashVertical, bits[2] ? 1f : bits[3] ? -1f : 0f);
            
            animator.SetBool(AnimatorHashes.hashGrounded, bits[4]); // 4 - isGrounded
            animator.SetBool(AnimatorHashes.hashJump, bits[5]); // 5 - isJumping
            animator.SetBool(AnimatorHashes.hashAim, bits[6]); // 6 - isAiming
            animator.SetBool(AnimatorHashes.hashFly, bits[7]); // 7 - isFlying
            
        }

        #endregion
        
        
    }
    
}
