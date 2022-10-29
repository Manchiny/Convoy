using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HudHealthBar : HealthBar
    {
        [SerializeField] private Damageable _unit;

        protected override void OnInit()
        {
            Damageable = _unit;
        }
    }
}