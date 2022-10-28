using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private RectTransform _mainFiller;

        private const float MainFillerOffse = 0.1f;
        private const float AnimationDurationPerUnit = 0.1f;

        private Damageable _damageable;
        private int _maxHealth;
        private int _lastHealth;

        private Tween _animation;

        Vector3 _scale = Vector3.one;

        private void Awake()
        {
            _damageable = GetComponentInParent<Damageable>();
            _slider.minValue = 0;
            _slider.maxValue = 1;

            _damageable.Damaged += OnHealthChanged;
            _damageable.Died += OnDied;
            Game.Restarted += OnRestart;

            OnRestart();
        }

        private void OnDestroy()
        {
            _damageable.Damaged -= OnHealthChanged;
            _damageable.Died -= OnDied;
            Game.Restarted -= OnRestart;
        }

        private void OnHealthChanged()
        {
            if (_maxHealth == 0)
            {
                if (_damageable.MaxHealth == 0)
                    return;
                else
                {
                    _maxHealth = _damageable.MaxHealth;
                    _lastHealth = _maxHealth;
                }
            }

            float targetValue = _damageable.CurrentHealth / (float)_maxHealth;
            float duration = (_lastHealth - _damageable.CurrentHealth) * AnimationDurationPerUnit;
            float timeRamin = 0f;

            _scale.x = targetValue + MainFillerOffse;
            _mainFiller.localScale = _scale;

            _lastHealth = _damageable.CurrentHealth;

            if (_animation != null && _animation.IsActive())
            {
                timeRamin = _slider.value-targetValue * _animation.position;
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

            _maxHealth = _damageable.MaxHealth;
            _lastHealth = _maxHealth;
        }
    }
}
