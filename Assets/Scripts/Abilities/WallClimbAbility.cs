using UnityEngine;

namespace Abilities
{
    public class WallClimbAbility : Ability
    {
        protected override void OnUnlock()
        {
            // Initialization logic when wall climb is unlocked
            // You can access ClimbSpeed here if needed
            Debug.Log("WallClimbAbility unlocked");
        }
    }
}