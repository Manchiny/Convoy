using UnityEngine;

namespace Assets.Scripts.Units
{
    public class TankEnemyChecker : EnemyChecker
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (other.TryGetComponent(out Player player))
                player.OnTankZoneEntered();
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            if (other.TryGetComponent(out Player player))
                player.OnTankZoneLeave();
        }
    }
}
