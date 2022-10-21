using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{

    public class Tower : Unit
    {
        public override int MaxHealth => 100;

        public override Team TeamId => Team.Enemy;

        protected override void Die()
        {
            throw new System.NotImplementedException();
        }

        protected override void OneEnenmyMissed(Damageable enemy)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnEnemyFinded(Damageable enemy)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnGetDamage()
        {
            throw new System.NotImplementedException();
        }
    }
}
