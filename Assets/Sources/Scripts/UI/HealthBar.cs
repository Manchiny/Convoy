using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private RectTransform _mainFiller;

        private const float MainFillerOffse = 0.01f;
        private const float AnimationDurationPerUnit = 0.15f;

        protected Damageable Damageable;

        private int _maxHealth;
        private int _lastHealth;

        private Tween _animation;

        Vector3 _scale = Vector3.one;

        private void Awake()
        {
            OnInit();

            _slider.minValue = 0;
            _slider.maxValue = 1;

            Damageable.HealthChanged += OnHealthChanged;
            Damageable.Died += OnDied;
            Game.Restarted += OnRestart;

            OnRestart();
        }

        private void OnDestroy()
        {
            Damageable.HealthChanged -= OnHealthChanged;
            Damageable.Died -= OnDied;
            Game.Restarted -= OnRestart;
        }

        protected virtual void OnInit()
        {
            Damageable = GetComponentInParent<Damageable>();
        }

        private void OnHealthChanged()
        {
            if (_maxHealth == 0)
            {
                if (Damageable.MaxHealth == 0)
                    return;
                else
                {
                    _maxHealth = Damageable.MaxHealth;
                    _lastHealth = _maxHealth;
                }
            }

            _scale.x = Damageable.CurrentHealth / (float)_maxHealth;
            
            float targetValue = _scale.x - MainFillerOffse;

            if (targetValue < 0)
                targetValue = 0;

            float duration = (_lastHealth - Damageable.CurrentHealth) * AnimationDurationPerUnit;

            _mainFiller.localScale = _scale;

            _lastHealth = Damageable.CurrentHealth;

            if (_animation != null && _animation.IsActive())
            {
                float timeRamin = _slider.value - targetValue * _animation.position;
                duration += timeRamin;

                _animation.Kill();
            }

            _animation = _slider.DOValue(targetValue, duration).SetLink(gameObject).SetEase(Ease.Linear);
        }

        private void OnDied(Damageable damageable)
        {
            gameObject.SetActive(false);
        }

        private void OnRestart()
        {
            gameObject.SetActive(true);

            _slider.value = _slider.maxValue;
            _scale.x = 1;
            _mainFiller.localScale = _scale;

            _maxHealth = Damageable.MaxHealth;
            _lastHealth = _maxHealth;
        }
    }
}
