namespace Assets.Scripts.Units
{
    public class EnemyGroup
    {
        public Damageable Target { get; private set; }

        public EnemyGroup()
        {
            Target = null;
        }

        public void OnAnyFindTarget(Damageable damageable)
        {
            if (Target == null)
                Target = damageable;

            else if (damageable is Tank && Target is not Tank)
                Target = damageable;
        }

        public void ResetTarget()
        {
            Target = null;
        }
    }
}
