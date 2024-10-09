namespace Abilities
{
    public abstract class Ability : IAbility
    {
        public bool IsUnlocked { get; private set; }

        public void Unlock()
        {
            IsUnlocked = true;
            OnUnlock();
        }

        public virtual void Activate() { }
        public virtual void Deactivate() { }

        protected virtual void OnUnlock() { }
    }
}