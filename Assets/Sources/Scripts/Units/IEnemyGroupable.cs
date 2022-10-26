namespace Assets.Scripts.Units
{
    public interface IEnemyGroupable
    {
        public EnemyGroup Group { get; }
        public void SetGroup(EnemyGroup group);
    }
}
