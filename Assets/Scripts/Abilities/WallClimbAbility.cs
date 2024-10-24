using UnityEngine;

namespace Abilities
{
    [System.Serializable]
    public class WallClimbAbility : Ability
    {
        protected override void OnUnlock()
        {
            Debug.Log("WallClimbAbility unlocked");
        }
    }
}