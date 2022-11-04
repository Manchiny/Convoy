using Assets.Scripts.Units;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private RectTransform _mainFiller;

        private const float MainFillerOffset = 0.01f;
        private const float AnimationDurationPerUnit = 0.15f;

        protected Damageable Damageable;

        protected bool NeedLog;
        protected bool NeedHideOnDie = true;

        private int _maxHealth;
        private int _lastHealth;

        private Tween _animation;
        private Vector3 _scale = Vector3.one;

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

            float targetValue = _scale.x - MainFillerOffset;

            if (targetValue < 0)
                targetValue = 0;

            float duration = (_lastHealth - Damageable.CurrentHealth) * AnimationDurationPerUnit;

            //if (NeedLog)
            //    Debug.Log($"{Damageable.gameObject.name} [1]: HP {Damageable.CurrentHealth}/{_maxHealth}, lastHealth {_lastHealth}, scale.x = {_scale.x}, targetValue {targetValue}, duration {duration}; slider {_slider.value}");

            _mainFiller.localScale = _scale;

            _lastHealth = Damageable.CurrentHealth;

            if (_animation != null)
            {
                float currentValue = _slider.value;

                float timeRamin = currentValue - targetValue * _animation.position;
                duration += timeRamin;

                _animation.Kill();
                _slider.value = currentValue;
            }

            if (duration <= 0)
                duration = 0.1f;

            //if (NeedLog)
            //    Debug.Log($"{Damageable.gameObject.name} [2]: HP {Damageable.CurrentHealth}/{_maxHealth}, lastHealth {_lastHealth}, scale.x = {_scale.x}, targetValue {targetValue}, duration {duration}; slider {_slider.value}");

            _animation = _slider.DOValue(targetValue, duration).SetLink(gameObject).SetEase(Ease.Linear).SetUpdate(true);
        }

        private void OnDied(Damageable damageable)
        {
            if(NeedHideOnDie)
                gameObject.SetActive(false);
        }

        private void OnRestart()
        {
            gameObject.SetActive(true);

            if (_animation != null)
                _animation.Kill();

            _slider.value = _slider.maxValue;
            _scale.x = 1;
            _mainFiller.localScale = _scale;

            _maxHealth = Damageable.MaxHealth;
            _lastHealth = _maxHealth;
        }
    }
}
