using Assets.Scripts.Items;
using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UnitBoostsStateView : MonoBehaviour
    {
        [SerializeField] private Image _armorBoost;
        [SerializeField] private Image _damageBoost;
        [SerializeField] private Image _shootingSpeedBoost;

        private IBoostable _unit;

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            _unit.BoostAdded -= OnAnyBoostAdded;
            _unit.BoostRemoved -= OnAnyBoostRemoved;
        }

        private void Init()
        {
            _unit = GetComponentInParent<IBoostable>();

            OnAnyBoostRemoved(ItemType.ArmorMultyplier);
            OnAnyBoostRemoved(ItemType.DamageMultyplier);
            OnAnyBoostRemoved(ItemType.ShootingDelayDivider);

            _unit.BoostAdded += OnAnyBoostAdded;
            _unit.BoostRemoved += OnAnyBoostRemoved;
        }

        private void OnAnyBoostAdded(ItemType type)
        {
            switch(type)
            {
                case ItemType.ArmorMultyplier:
                    _armorBoost.gameObject.SetActive(true);
                    break;
                case ItemType.DamageMultyplier:
                    _damageBoost.gameObject.SetActive(true);
                    break;
                case ItemType.ShootingDelayDivider:
                    _shootingSpeedBoost.gameObject.SetActive(true);
                    break;
            }
        }

        private void OnAnyBoostRemoved(ItemType type)
        {
            switch (type)
            {
                case ItemType.ArmorMultyplier:
                    _armorBoost.gameObject.SetActive(false);
                    break;
                case ItemType.DamageMultyplier:
                    _damageBoost.gameObject.SetActive(false);
                    break;
                case ItemType.ShootingDelayDivider:
                    _shootingSpeedBoost.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
