using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public class Abilities 
    {
        private Dictionary<Type, Ability> abilities = new Dictionary<Type, Ability>();

        public void AddAbility(Ability ability)
        {
            Type type = ability.GetType();
            if (!abilities.ContainsKey(type))
            {
                abilities[type] = ability;
            }
        }

        public void UnlockAbility(Type abilityType)
        {
            if (abilities.TryGetValue(abilityType, out Ability ability))
            {
                ability.Unlock();
                Debug.Log(abilityType.Name + " unlocked: " + ability.isUnlocked);
            }
            else
            {
                Debug.Log("Ability not found: " + abilityType.Name);
            }
        }

        public bool IsAbilityUnlocked<T>() where T : Ability
        {
            Type type = typeof(T);
            return abilities.TryGetValue(type, out Ability ability) && ability.isUnlocked;
        }
    }
}