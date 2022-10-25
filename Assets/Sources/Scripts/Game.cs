using Assets.Scripts.Levels;
using Assets.Scripts.Saves;
using Assets.Scripts.Social;
using Assets.Scripts.Social.Adverts;
using Assets.Scripts.UI;
using Assets.Scripts.Units;
using Assets.Scripts.UserInputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Tank _tank;
        [Space]
        [SerializeField] private List<Level> _levels;
        [Space]
        [SerializeField] private WindowsController _windowsController;
        [Space]
        [SerializeField] private Transform _sceneGarbageHolder;
        [Space]
        [SerializeField] private YandexSocialAdapter _yandexAdapter;
        [SerializeField] private TankPropertiesDatabase _tankPropertiesDatabase;

        private YandexAdvertisingAdapter _adverts;

        private UserInput _input;
        private GameMode _currentMode;
        private Saver _saver;
        private UserData _userData;

        public event Action<GameMode> GameModeChanged;
        public static Action Inited;

        public enum GameMode
        {
            Game,
            TankUpgrade,
        }

        public Level CurrentLevel { get; private set; }
        public static Game Instance { get; private set; }

        public static YandexAdvertisingAdapter Adverts => Instance._adverts;
        public static YandexSocialAdapter SocialAdapter => Instance._yandexAdapter;
        public static Player Player => Instance._player;
        public static TankPropertiesDatabase TankProperies => Instance._tankPropertiesDatabase;
        public static WindowsController Windows => Instance._windowsController;
        public static Transform GarbageHolder => Instance._sceneGarbageHolder;
        public static GameMode CurrentMode => Instance._currentMode;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Windows.Loader.gameObject.SetActive(true);

                _tankPropertiesDatabase.Init();

                DontDestroyOnLoad(this);
                return;
            }

            Destroy(this);
        }

        private IEnumerator Start()
        {
#if UNITY_WEBGL && YANDEX_GAMES && !UNITY_EDITOR
            yield return StartCoroutine(_yandexAdapter.Init());

            _adverts = new YandexAdvertisingAdapter();
            _adverts.Init(_yandexAdapter);
#endif
            yield return null;

            if (_yandexAdapter.IsInited)
                _saver = new YandexSaver();
            else
                _saver = new LocalJSONSaver();

            _saver.LoadUserData(InitGame);
        }


        public void SetInputSystem(UserInput input)
        {
            Debug.Log($"Device type = {SystemInfo.deviceType}");

            if (input.NeedActivate() == false)
            {
                input.gameObject.SetActive(false);
                Debug.Log($"{input.GetType()} disbled");
                return;
            }

            Debug.Log($"{input.GetType()} init");
            _input = input;
            _input.Init(_player.Movement);
        }

        public void OnDamageableHited(Damageable damageable, int damage)
        {
            damageable.GetDamage(damage);
        }

        public void SetMode(GameMode mode)
        {
            switch(mode)
            {
                case GameMode.Game:
                    Unpause();
                    break;
                case GameMode.TankUpgrade:
                    Pause();
                    break;
            }

            GameModeChanged?.Invoke(mode);
        }

        public void Save()
        {
            _userData.TankData = _tank.GetData;
            _saver.Save(_userData);
        }

        private void InitGame(UserData userData)
        {
            _userData = userData;

            Windows.Loader.gameObject.SetActive(false);

            StartLevel(_levels[0]);
            _currentMode = GameMode.Game;

            _tank.Init(_userData.TankData);

            Inited?.Invoke();
        }

        private void Pause()
        {
            Time.timeScale = 0;
        }

        private void Unpause()
        {
            Time.timeScale = 1;
        }

        private void StartLevel(Level level)
        {
            if (CurrentLevel != null)
                Destroy(CurrentLevel.gameObject);

            CurrentLevel = level;
            _tank.InitLevelProperties(level.SpawnPoint.position, level.Waypoints);
        }
    }
}
