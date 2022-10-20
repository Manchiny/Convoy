namespace Assets.Scripts.Guns
{
    public class TankTower : Gun
    {
        protected override int Damage => 100;
        protected override float CooldawnSeconds => 2f;
        protected override int PoolCount => 20;
    }
}
