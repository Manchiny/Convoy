using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class DamageColorChanger : MonoBehaviour
    {
        private const float SetColorDuration = 0.15f;

        private Color _damagedColor = Color.white;

        private Damageable _unit;

        private Dictionary<Material, Color> _baseColors = new();
        private HashSet<Material> _materials;

        private bool _died;

        private void Awake()
        {
            _unit = GetComponentInParent<Damageable>();

            _materials = GetComponent<Renderer>().materials.ToHashSet();

            foreach (var material in _materials)
                _baseColors.Add(material, material.color);

            _unit.Died += OnUnitDied;
            _unit.Damaged += OnUnitDamaged;
        }

        private void OnUnitDied(Damageable damageable)
        {
            if (_died)
                return;

            _died = true;

            foreach (var material in _materials)
                SetMaterialColorGray(material);
        }
        
        private void OnUnitDamaged()
        {
            if (_died)
                return;

            foreach (var material in _materials)
                SetColorToDamaged(material);
        }

        private void SetColorToDamaged(Material material)
        {
            SetColor(material, _damagedColor, SetColorDuration, () => SetColorToBase(material));
        }

        private void SetColorToBase(Material material)
        {
            if (_died)
                return;

            Color baseColor = _baseColors[material];
            SetColor(material, baseColor, SetColorDuration);
        }

        private void SetMaterialColorGray(Material material)
        {
            Color baseColor = _baseColors[material];
            Color color = Color.white * baseColor.grayscale;
            color.a = 1;

            SetColor(material, color, SetColorDuration);
        }

        private void SetColor(Material material, Color color, float duration, Action onComplete = null)
        {
           material.DOColor(color, duration).SetEase(Ease.Linear).SetLink(gameObject)
                .OnComplete(() =>
                {
                    if (onComplete != null)
                        onComplete?.Invoke();
                });
        }

        private void OnDisable()
        {
            _unit.Died -= OnUnitDied;
            _unit.Damaged -= OnUnitDamaged;
        }
    }
}
