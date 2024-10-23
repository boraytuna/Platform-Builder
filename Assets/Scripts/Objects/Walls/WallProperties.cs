using UnityEngine;

namespace Walls
{
    public class WallProperties : MonoBehaviour
    {
        [Header("Wall Slide Variables")]
        [SerializeField] private float wallStickTime; // Time to stick to the wall
        [SerializeField] private float wallSlideSpeed; // Speed to slide down the wall
        [SerializeField] private float wallFastSlideSpeed; // Speed when sliding down faster
        [SerializeField] private float wallClimbSpeed; // Speed when climbing up
        [SerializeField] private float wallFlySpeedLimit; // Limit to player from flying too much

        // Properties to allow access from the player controller
        public float WallStickTime => wallStickTime;
        public float WallSlideSpeed => wallSlideSpeed;
        public float WallFastSlideSpeed => wallFastSlideSpeed;
        public float WallClimbSpeed => wallClimbSpeed;
        
        public float WallFlySpeedLimit => wallFlySpeedLimit;
    }
}