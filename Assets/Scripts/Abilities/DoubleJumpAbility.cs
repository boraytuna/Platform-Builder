using UnityEngine;

namespace Abilities
{
    public class DoubleJumpAbility : Ability
    {
        protected override void OnUnlock()
        {
            // Any initialization logic when double jump is unlocked
            Debug.Log("DoubleJumpAbility unlocked");
        }
    }
}