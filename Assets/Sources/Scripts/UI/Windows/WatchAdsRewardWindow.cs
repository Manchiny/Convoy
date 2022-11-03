using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class WatchAdsRewardWindow : RewardedWindow
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private BasicButton _adsButton;
        [SerializeField] private TextMeshProUGUI _adsText;
        [SerializeField] private BasicButton _getButton;

        private const string TitleLocalizationKey = "air_drop";
        private const string AdsLocalizationKey = "ad";
        private const string ContinueLocalizationKey = "continue";
        private const string GetLocalizationKey = "get";

        private bool _rewarded = false;
        public override bool NeedHideHudOnShow => true;

        public static WatchAdsRewardWindow Show(List<ItemCount> items, Action onGetReward) =>
               Game.Windows.ScreenChange<WatchAdsRewardWindow>(false, w => w.Init(items, onGetReward));

        protected override void Init(List<ItemCount> items, Action onGetReward)
        {
            base.Init(items, onGetReward);

            _adsButton.SetOnClick(OnAdsButtonClick);
            _getButton.SetOnClick(OnGetButtonClick);

            SetText();

            _adsButton.gameObject.SetActive(!_rewarded);
            _getButton.gameObject.SetActive(_rewarded);

        }

        protected override void OnClose()
        {
            if (_rewarded)
            {
                Game.Instance.AddItems(Items);
                RewardGot?.Invoke();
            }
        }

        protected override void SetText()
        {
            _titleText.text = TitleLocalizationKey.Localize();

            _adsText.text = AdsLocalizationKey.Localize();
            _adsButton.Text = GetLocalizationKey.Localize();

            _getButton.Text = ContinueLocalizationKey.Localize();
        }

        private void OnGetButtonClick()
        {
            Close();
        }

        private void OnAdsButtonClick()
        {

#if UNITY_EDITOR
            _rewarded = true;
            OnAdsClose();
            return;
#endif
            Game.Adverts.TryShowRewardedVideo(OnAdsOpen, OnAdsRewarded, OnAdsClose, OnAdsError);

            void OnAdsOpen()
            {
                _adsButton.SetLock(true);
            }

            void OnAdsRewarded()
            {
                _rewarded = true;
            }

            void OnAdsError()
            {
                _adsButton.SetLock(false);
            }

            void OnAdsClose()
            {
                if (_rewarded)
                {
                    _adsButton.gameObject.SetActive(false);
                    _getButton.gameObject.SetActive(true);
                }
                else
                    _adsButton.SetLock(false);
            }
        }
    }
}
