namespace Abilities
{
    public interface IAbility
    {
        bool isUnlocked { get; }
        void Unlock();
    }
}