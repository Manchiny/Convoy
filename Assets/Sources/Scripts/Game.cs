using Assets.Scripts.Levels;
using Assets.Scripts.Saves;
using Assets.Scripts.Social;
using Assets.Scripts.Social.Adverts;
using Assets.Scripts.UI;
using Assets.Scripts.Units;
using Assets.Scripts.UserInputSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Tank _tank;
        [Space]
        [SerializeField] private LevelLoader _levelLoader;
        [Space]
        [SerializeField] private WindowsController _windowsController;
        [Space]
        [SerializeField] private Transform _sceneGarbageHolder;
        [Space]
        [SerializeField] private YandexSocialAdapter _yandexAdapter;
        [SerializeField] private UnitPropertiesDatabase _tankPropertiesDatabase;
        [SerializeField] private UnitPropertiesDatabase _playerCharacterPropertiesDatabase;

        private YandexAdvertisingAdapter _adverts;

        private UserData _userData;
        private Saver _saver;

        private GameMode _currentMode;

        private UserInput _input;

        private bool _winLooseProcess;

        public static event Action Inited;
        public static event Action Restarted;

        public event Action<GameMode> GameModeChanged;

        public enum GameMode
        {
            Game,
            TankUpgrade
        }

        public static Game Instance { get; private set; }

        public int CurrentLevelId => _userData.LevelId;
        public static Player Player => Instance._player;

        public static WindowsController Windows => Instance._windowsController;

        public static YandexAdvertisingAdapter Adverts => Instance._adverts;
        public static YandexSocialAdapter SocialAdapter => Instance._yandexAdapter;

        public static UnitPropertiesDatabase TankProperies => Instance._tankPropertiesDatabase;
        public static UnitPropertiesDatabase CharacterProperties => Instance._playerCharacterPropertiesDatabase;

        public static Transform GarbageHolder => Instance._sceneGarbageHolder;
        public static GameMode CurrentMode => Instance._currentMode;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Windows.Loader.gameObject.SetActive(true);

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

            _tankPropertiesDatabase.Init();
            _playerCharacterPropertiesDatabase.Init();

            if (_yandexAdapter.IsInited)
                _saver = new YandexSaver();
            else
                _saver = new LocalJSONSaver();

            _saver.LoadUserData(InitGame);
        }

        private void OnDestroy()
        {
            _tank.Completed -= OnLevelComplete;
            _tank.Died -= OnAnyPlayerUnitDied;
            _player.Died -= OnAnyPlayerUnitDied;
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
            switch (mode)
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
            _userData.PlayerCharacterData = _player.GetData;

            _saver.Save(_userData);
        }

        private void InitGame(UserData userData)
        {
            _userData = userData;

            Windows.Loader.gameObject.SetActive(false);

            StartLevel(_userData.LevelId);
            _currentMode = GameMode.Game;

            _tank.Init(_userData.TankData, _tankPropertiesDatabase);
            _player.Init(_userData.PlayerCharacterData, _playerCharacterPropertiesDatabase);

            _tank.Completed += OnLevelComplete;
            _tank.Died += OnAnyPlayerUnitDied;
            _player.Died += OnAnyPlayerUnitDied;

            Inited?.Invoke();
        }

        private void StartLevel(int levelId, bool restart = false)
        {
            if (restart == false)
                _levelLoader.LoadLevel(levelId);

            _tank.OnLevelStarted(_levelLoader.TankSpawnPoint.position, _levelLoader.Waypoints);
            _player.OnLevelStarted(_levelLoader.PlayerSpawnPoint.position);

            Debug.Log($"Level {levelId + 1} started");

            _winLooseProcess = false;
        }

        private void OnAnyPlayerUnitDied(Damageable unit)
        {
            if (_winLooseProcess)
                return;

            _winLooseProcess = true;

            Debug.Log($"Level {CurrentLevelId + 1} loosed!");
            RestartLevel();
        }

        private void OnLevelComplete()
        {
            if (_winLooseProcess)
                return;

            _winLooseProcess = true;

            Debug.Log($"Level {CurrentLevelId + 1} completed!");

            _userData.LevelId++;
            Save();

            StartLevel(CurrentLevelId);
        }

        private void RestartLevel()
        {
            StartLevel(CurrentLevelId, true);
            Restarted?.Invoke();
        }

        private void Pause()
        {
            Time.timeScale = 0;
        }

        private void Unpause()
        {
            Time.timeScale = 1;
        }
    }
}
