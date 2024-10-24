namespace Abilities
{
    [System.Serializable]
    public abstract class Ability : IAbility
    {
        public bool isUnlocked { get; private set; }

        public void Unlock()
        {
            isUnlocked = true;
            OnUnlock();
        }

        protected virtual void OnUnlock() { }
    }
}