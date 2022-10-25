using static Assets.Scripts.Damageable;

namespace Assets.Scripts
{
    public interface IAttackable
    {
        public void AddFindedEnemy(Damageable enemy);
        public void RemoveFromEnemies(Damageable enemy);
        public int Damage { get; }
        float ShootDelay { get; }
        public Team TeamId { get; }
    }
}
