using Agava.YandexGames;
using System;
using UnityEngine;

namespace Assets.Scripts.Social.Adverts
{
    public class YandexAdvertisingAdapter
    {
        private int _showInterstitialAfterLevelCounter;
        private YandexSocialAdapter _socialAdapter;

        private event Action Rewarded;
        private event Action RevardedAdsOpened;
        private event Action RewardedAdsClosed;
        private event Action RewardAdsError;

        private event Action OnInterstitialClosed;

        public bool NeedShowInterstitial => true;// _showInterstitialAfterLevelCounter <= 0;

        public string Tag => "YandexAdverts";

        protected void ShowInterstitial(Action onOpen, Action<bool> onClose, Action<string> onError, Action onOffline)
        {
            InterstitialAd.Show(onOpen, onClose, onError, onOffline);
        }

        protected void ShowRewarded(Action onOpen, Action onRewarded, Action onClose, Action<string> onError)
        {
            VideoAd.Show(onOpen, onRewarded, onClose, onError);
        }

        public void Init(YandexSocialAdapter socialAdapter)
        {
            _socialAdapter = socialAdapter;
            // _showInterstitialAfterLevelCounter = GameConstants.LevelsCountBetweenInterstitialShow;
        }

        public void TryShowInterstitial(Action onClose)
        {
            Debug.Log($"[{Tag}] Try show Interstitial... ");
            OnInterstitialClosed = onClose;

            if (_socialAdapter == null || _socialAdapter.IsInited == false || NeedShowInterstitial == false)
            {
                Debug.Log($"[{Tag}] Interstitial rejected: social adapetr inited = {_socialAdapter?.IsInited}, need show interstitial = {NeedShowInterstitial}, show interstitial counter = {_showInterstitialAfterLevelCounter}");

                onClose?.Invoke();
                return;
            }

            Debug.Log($"[{Tag}] start show Interstitial... ");
            ShowInterstitial(OnInterstitialOpen, OnInterstitilaClose, OnInterstitiaError, OnIntersitialOffline);
        }

        public bool TryShowRewardedVideo(Action onOpen,Action onRewarded, Action onClose, Action onError)
        {
            if (_socialAdapter == null || _socialAdapter.IsInited == false)
            {
                Debug.Log($"{Tag}: social adapter is not inited");
                return false;
            }

            RevardedAdsOpened = onOpen;
            Rewarded = onRewarded;
            RewardedAdsClosed = onClose;
            RewardAdsError = onError;

            ShowRewarded(OnRewardedOpen, OnRewarded, OnRewardedClose, OnRewardedError);

            return true;
        }

        private void OnInterstitilaClose(bool close)
        {
            if (OnInterstitialClosed != null)
            {
                OnInterstitialClosed?.Invoke();
                OnInterstitialClosed = null;
            }
        }

        private void OnInterstitiaError(string errorMessage)
        {
            OnInterstitilaClose(false);
            Debug.LogError($"[{Tag}]: {errorMessage}");
        }

        private void OnInterstitialOpen()
        {
            // _showInterstitialAfterLevelCounter = GameConstants.LevelsCountBetweenInterstitialShow;
            Debug.Log($"[{Tag}]: opened interstitial video");
        }

        private void OnIntersitialOffline()
        {
            Debug.LogError($"[{Tag}]: offline");
        }

        protected void OnRewardedOpen()
        {
            Debug.Log($"[{Tag}]: opened rewarded video");

            if (RevardedAdsOpened != null)
            {
                RevardedAdsOpened?.Invoke();
                RevardedAdsOpened = null;
            }
        }

        protected void OnRewarded()
        {
            Debug.Log($"[{Tag}]: rewarded!");

            if (Rewarded != null)
            {
                Rewarded?.Invoke();
                Rewarded = null;
            }
        }

        private void OnRewardedClose()
        {
            if (RewardedAdsClosed != null)
            {
                RewardedAdsClosed?.Invoke();
                RewardedAdsClosed = null;
            }
        }

        private void OnRewardedError(string errorMessage)
        {
            Debug.LogError($"[{Tag}]: {errorMessage}");

            if (RewardAdsError != null)
            {
                RewardAdsError?.Invoke();
                RewardAdsError = null;
            }
        }

        private void OnLevelComplete()
        {
            _showInterstitialAfterLevelCounter--;
        }
    }
}
