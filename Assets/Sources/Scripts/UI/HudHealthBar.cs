using UnityEngine;

namespace Assets.Scripts.UI
{
    public class HudHealthBar : HealthBar
    {
        [SerializeField] private Damageable _unit;

        protected override void OnInit()
        {
            Damageable = _unit;
            NeedLog = true;
            NeedHideOnDie = false;
        }
    }
}