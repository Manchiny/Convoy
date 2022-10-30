using Assets.Scripts.UI;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Units
{
    public class UpgradeTankZone : MonoBehaviour
    {
        [SerializeField] private RectTransform _openingIndicator;
        [SerializeField] private Image _filler;
        [SerializeField] private TextMeshProUGUI _upgrageTankText;

        private const string UpgradeTankLocalizationKey = "upgrade_tank";
        private const float DelayTime = 2f;

        private Tween _fillerAnimation;

        private void Awake()
        {
            Game.Inited += Init;
        }

        private void Start()
        {
            _openingIndicator.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player) == true)
                ShowIndicator();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player) == true)
                StopWaiting();
        }

        private void OnDestroy()
        {
            Game.Inited -= Init;
        }

        private void Init()
        {
            Game.Inited -= Init;
            _upgrageTankText.text = UpgradeTankLocalizationKey.Localize();
        }

        private void ShowIndicator()
        {
            if (_fillerAnimation != null && _fillerAnimation.IsActive())
                return;

            _filler.fillAmount = 0;
            _openingIndicator.gameObject.SetActive(true);

            _fillerAnimation = _filler.DOFillAmount(1, DelayTime).SetLink(_openingIndicator.gameObject).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    UpgradeTankWindow.Show();
                    StopWaiting();
                });
        }

        private void StopWaiting()
        {
            if(_fillerAnimation != null)
            {
                _fillerAnimation.Kill();
                _fillerAnimation = null;
            }

            _openingIndicator.gameObject.SetActive(false);
        }
    }
}
