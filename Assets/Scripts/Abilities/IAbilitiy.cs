namespace Abilities
{
    public interface IAbility
    {
        bool IsUnlocked { get; }
        void Unlock();
        void Activate();
        void Deactivate();
    }
}