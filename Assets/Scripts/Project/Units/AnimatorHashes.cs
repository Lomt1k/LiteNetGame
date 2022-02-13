using UnityEngine;

namespace Project.Units
{
    public static class AnimatorHashes
    {
        public static readonly int hashHorizontal = Animator.StringToHash("H");
        public static readonly int hashVertical = Animator.StringToHash("V");
        public static readonly int hashSpeed = Animator.StringToHash("Speed");
        public static readonly int hashGrounded = Animator.StringToHash("Grounded");
        public static readonly int hashJump = Animator.StringToHash("Jump");
        public static readonly int hashAim = Animator.StringToHash("Aim");
        public static readonly int hashFly = Animator.StringToHash("Fly");
    }
}

