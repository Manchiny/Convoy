namespace Assets.Scripts.Units
{
    public class RoadShelterEnemy : MovableEnemy
    {
        protected override float AttackDistance => 24f;

        protected override void OnFixedUpdate()
        {
            if (NeedAttack)
                Attack();
        }
    }
}
