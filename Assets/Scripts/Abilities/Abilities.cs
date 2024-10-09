// Abilities.cs
using System;
using System.Collections.Generic;

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

        public void UnlockAbility<T>() where T : Ability
        {
            Type type = typeof(T);
            if (abilities.TryGetValue(type, out Ability ability))
            {
                ability.Unlock();
            }
        }

        public bool IsAbilityUnlocked<T>() where T : Ability
        {
            Type type = typeof(T);
            return abilities.TryGetValue(type, out Ability ability) && ability.IsUnlocked;
        }
    }
}