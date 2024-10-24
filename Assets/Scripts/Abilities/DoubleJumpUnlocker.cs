using Managers;
using UnityEngine;

namespace Abilities
{
    public class DoubleJumpUnlocker : AbilityUnlocker
    {
        public override void TryUnlockAbility(Abilities playerAbilities)
        {
            Debug.Log("Unlocking Double Jump Ability!");

            // Invoke the ability unlocked event
            GamePlayEvents.instance.AbilityUnlocked(typeof(DoubleJumpAbility));
            
            // Destroy game object after unlocking
            Destroy(gameObject);
        }
    }
}