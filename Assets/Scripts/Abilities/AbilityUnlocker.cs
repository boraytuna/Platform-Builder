using UnityEngine;

namespace Abilities
{
    public abstract class AbilityUnlocker : MonoBehaviour
    {
        public abstract void TryUnlockAbility(Abilities playerAbilities);
    }
}