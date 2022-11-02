using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LevelFailedWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _failedText;
        [Space]
        [SerializeField] private BasicButton _continueButton;

        private const string LevelLocalizationKey = "level";
        private const string FailedLocalizationKey = "failed";
        private const string ContinueLocalizationKey = "continue";

        private event Action _onContinueCallback;

        public override string LockKey => "LevelFailedWindow";
        public override bool AnimatedClose => true;

        public static LevelFailedWindow Show(Action onContinue) =>
                      Game.Windows.ScreenChange<LevelFailedWindow>(true, w => w.Init(onContinue));

        protected override void SetText()
        {
            _levelText.text = LevelLocalizationKey.Localize() + $" {Game.Instance.CurrentLevelId + 1}";
            _failedText.text = FailedLocalizationKey.Localize() + "!";
            _continueButton.Text = ContinueLocalizationKey.Localize();
        }

        private void Init(Action onContinue)
        {
            _onContinueCallback = onContinue;
            SetText();
            _continueButton.SetOnClick(OnContinueButtonClick);
        }

        private void OnContinueButtonClick()
        {
            Close();

            if (_onContinueCallback != null)
                _onContinueCallback?.Invoke();
        }
    }
}