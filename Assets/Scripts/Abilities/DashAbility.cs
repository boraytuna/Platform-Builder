using UnityEngine;

namespace Abilities
{
    public class DashAbility : Ability
    {
        private float dashSpeed;
        private float dashDuration;

        public DashAbility(float speed, float duration)
        {
            dashSpeed = speed;
            dashDuration = duration;
        }

        protected override void OnUnlock()
        {
            // Initialization logic when dash is unlocked
            Debug.Log("DashAbility unlocked");
        }

        public override void Activate()
        {
            // Logic to start the dash
        }

        public override void Deactivate()
        {
            // Logic to end the dash
        }

        public float DashSpeed => dashSpeed;
        public float DashDuration => dashDuration;
    }
}