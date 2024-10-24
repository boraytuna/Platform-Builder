using UnityEngine;

namespace Abilities
{
    [System.Serializable]
    public class DoubleJumpAbility : Ability
    {
        protected override void OnUnlock()
        {
            Debug.Log("Double jump unlocked!");
            // Activate double jump logic here
        }
    }
}