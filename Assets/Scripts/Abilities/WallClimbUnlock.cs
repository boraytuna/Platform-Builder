using Managers;
using UnityEngine;

namespace Abilities
{
    public class WallClimbUnlocker : AbilityUnlocker
    {
        public override void TryUnlockAbility(Abilities playerAbilities)
        {
            Debug.Log("Unlocking Double Jump Ability!");

            // Invoke the ability unlocked event
            GamePlayEvents.instance.AbilityUnlocked(typeof(WallClimbAbility));
            
            // Destroy game object after unlocking
            Destroy(gameObject);
        }
    }
}