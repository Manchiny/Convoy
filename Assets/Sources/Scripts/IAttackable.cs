using static Assets.Scripts.Damageable;

namespace Assets.Scripts
{
    public interface IAttackable
    {
        public void AddFindedEnemy(Damageable enemy);
        public void RemoveFromEnemies(Damageable enemy);
        public Team TeamId { get; }
    }
}
