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

        private Sequence _coloreChanging;
        private bool _died;


        private void Awake()
        {
            _unit = GetComponentInParent<Damageable>();

            _materials = GetComponent<Renderer>().materials.ToHashSet();

            foreach (var material in _materials)
                _baseColors.Add(material, material.color);

            _unit.Died += OnUnitDied;
            _unit.HealthChanged += OnUnitDamaged;
        }

        private void Start()
        {
            Game.Restarted += OnRestart;
        }

        private void OnDestroy()
        {
            Game.Restarted -= OnRestart;
            _unit.Died -= OnUnitDied;
            _unit.HealthChanged -= OnUnitDamaged;
        }

        public void OnRestart()
        {
            if (_coloreChanging != null)
                _coloreChanging.Kill();

            _died = false;

            SetColorToBase();
        }

        private void OnUnitDied(Damageable damageable)
        {
            if (_died)
                return;

            _died = true;

            SetMaterialColorGray();
        }

        private void OnUnitDamaged()
        {
            if (gameObject == null || _died)
                return;

            List<Tween> tweens = new();

            foreach (var material in _materials)
                tweens.Add(SetColorToDamaged(material));

            StartNewColorChanging(tweens, () => SetColorToBase());
        }

        private Tween SetColorToDamaged(Material material)
        {
            return SetColor(material, _damagedColor, SetColorDuration);
        }

        private void SetColorToBase()
        {
            if (gameObject == null || _died)
                return;

            List<Tween> tweens = new();

            foreach (var material in _materials)
            {
                Color baseColor = _baseColors[material];
                tweens.Add(SetColor(material, baseColor, SetColorDuration));
            }

            StartNewColorChanging(tweens);
        }

        private void SetMaterialColorGray()
        {
            List<Tween> tweens = new();

            foreach (var material in _materials)
            {
                Color baseColor = _baseColors[material];
                Color color = Color.white * baseColor.grayscale;
                color.a = 1;

                tweens.Add(SetColor(material, color, SetColorDuration));
            }

            StartNewColorChanging(tweens);
        }

        private Tween SetColor(Material material, Color color, float duration) => material.DOColor(color, duration);

        private void StartNewColorChanging(List<Tween> tweens, Action onComplete = null)
        {
            if (gameObject == null)
                return;

            if (_coloreChanging != null && _coloreChanging.active == true)
                _coloreChanging.Kill();

            _coloreChanging = DOTween.Sequence().SetEase(Ease.Linear).SetLink(gameObject).SetUpdate(true).OnComplete(() =>
                    {
                        if (onComplete != null)
                            onComplete?.Invoke();
                    });

            _coloreChanging.Append(tweens[0]);

            for (int i = 1; i < tweens.Count; i++)
                _coloreChanging.Insert(0, tweens[i]);

            _coloreChanging.Play();
        }
    }
}
