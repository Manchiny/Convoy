using Assets.Scripts.Social;
using GameAnalyticsSDK;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private YandexSocialAdapter _yandexAdapter;

        private IEnumerator Start()
        {
#if UNITY_WEBGL
            GameAnalytics.Initialize();
#if YANDEX_GAMES && !UNITY_EDITOR
            _yandexAdapter = new YandexSocialAdapter();
            yield return StartCoroutine(_yandexAdapter.Init());
#endif
#endif
            yield return null;
            SceneManager.LoadScene(1);
        }
    }
}
