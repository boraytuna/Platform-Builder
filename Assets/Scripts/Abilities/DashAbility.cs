using UnityEngine;

namespace Abilities
{
    [System.Serializable]
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

        public float DashSpeed => dashSpeed;
        public float DashDuration => dashDuration;
    }
}