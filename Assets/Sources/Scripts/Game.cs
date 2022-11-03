using Assets.Scripts.Items;
using Assets.Scripts.Levels;
using Assets.Scripts.Localization;
using Assets.Scripts.Saves;
using Assets.Scripts.Social;
using Assets.Scripts.Social.Adverts;
using Assets.Scripts.UI;
using Assets.Scripts.Units;
using Assets.Scripts.UserInputSystem;
using GameAnalyticsSDK;
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
        [SerializeField] private LevelLoader _levelLoader;
        [Space]
        [SerializeField] private WindowsController _windowsController;
        [Space]
        [SerializeField] private Transform _sceneGarbageHolder;
        [Space]
        [SerializeField] private UnitPropertiesDatabase _tankPropertiesDatabase;
        [SerializeField] private UnitPropertiesDatabase _playerCharacterPropertiesDatabase;
        [Space]
        [SerializeField] private LocalizationDatabase _localizationDatabase;
        [SerializeField] private Shop _shop;

        private YandexSocialAdapter _yandexAdapter;
        private YandexAdvertisingAdapter _adverts;

        private GameConfiguration _gameConfiguration;

        private UserData _userData;
        private Saver _saver;
        private GameLocalization _gameLocalization;

        private float _startLevelTime;

        private GameMode _currentMode;

        private UserInput _input;

        private bool _winLooseProcess;

        public static event Action Inited;
        public static event Action Restarted;

        public event Action<GameMode> GameModeChanged;

        public enum GameMode
        {
            Game,
            Puase
        }

        private enum LoosReason
        {
            Player_died,
            Tank_died
        }

        public static Game Instance { get; private set; }

        public int CurrentLevelId => _userData.LevelId;
        public static Player Player => Instance._player;
        public static Tank Tank => Instance._tank;
        public static Shop Shop => Instance._shop;
        public static UserData User => Instance._userData; 

        public static WindowsController Windows => Instance._windowsController;

        public static YandexAdvertisingAdapter Adverts => Instance._adverts;
        public static YandexSocialAdapter SocialAdapter => Instance._yandexAdapter;

        public static UnitPropertiesDatabase TankProperies => Instance._tankPropertiesDatabase;
        public static UnitPropertiesDatabase CharacterProperties => Instance._playerCharacterPropertiesDatabase;

        public static GameLocalization Localization => Instance?._gameLocalization;
        public static GameConfiguration Configuration => Instance?._gameConfiguration;

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
#if GAME_ANALYTICS
            GameAnalytics.Initialize();
            GameAnalytics.NewDesignEvent("game_start");
#endif
            _yandexAdapter = FindObjectOfType<YandexSocialAdapter>();

            if (_yandexAdapter != null && _yandexAdapter.IsInited)
            {
                _adverts = new YandexAdvertisingAdapter();
                _adverts.Init(_yandexAdapter);
                _saver = new YandexSaver();
            }
            else
                _saver = new LocalJSONSaver();

            yield return null;

            _saver.LoadUserData(InitData);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) == true)
                Debug.Log("Time: " + Time.time);
        }
#endif

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
                case GameMode.Puase:
                    Pause();
                    break;
            }

            GameModeChanged?.Invoke(mode);
        }

        public void Save()
        {
            _userData.TankData = _tank.Data;
            _userData.PlayerCharacterData = _player.Data;

            _saver.Save(_userData);
        }

        public bool TryUseItem(Item item, Action onEffectEnded, Action onUse)
        {
            if (_userData.TryUseItem(item, onEffectEnded))
            {
                ItemsUseHandler.UseItem(item);

                if(onUse != null)
                    onUse?.Invoke();

                Save();
                return true;
            }

            return false;
        }

        public void AddBadges(int count)
        {
            _userData.AddBadges(count);
            Save();
        }

        public void AddItems(List<ItemCount> items)
        {
            foreach (var item in items)
            {
                User.AddItemCount(item);
            }

            Save();
        }

        public void ChangeLocale(string local)
        {
            if (local == GameLocalization.CurrentLocale)
                return;

            _gameLocalization.LoadKeys(local, _localizationDatabase.LocalizationKeys);
            _userData.SavedLocale = GameLocalization.CurrentLocale;
        }

        public static string Localize(string key, params string[] parameters) => Localization?.Localize(key, parameters) ?? key;

        private void InitData(UserData userData)
        {
            _userData = userData;

            _tank.InitData(_userData.TankData, _tankPropertiesDatabase);
            _player.InitData(_userData.PlayerCharacterData, _playerCharacterPropertiesDatabase);

            StartCoroutine(LoadGameConfiguration());
        }

        private IEnumerator LoadGameConfiguration()
        {
            yield return StartCoroutine(Utils.LoadTextFromServer(GameConstants.GameConfigURL, (result) => Utils.ParseToDataOrCreateNew(result, out _gameConfiguration)));
            yield return _gameConfiguration != null;

            if (_gameConfiguration.NeedUpdatedLevels(_levelLoader.DatabaseVesrsion))
            {
                yield return StartCoroutine(Utils.LoadTextFromServer(GameConstants.LevelsDataURL, (result) => Utils.ParseToDataOrCreateNew(result, out _gameConfiguration.LevelsDatabaseData)));
                yield return _gameConfiguration.LevelsDatabaseData != null;
            }

            if (_gameConfiguration.NeedUpdateLocalizations(_localizationDatabase.Version))
            {
                yield return StartCoroutine(Utils.LoadTextFromServer(GameConstants.LocalizationsURL, (result) => Utils.ParseToDataOrCreateNew(result, out _gameConfiguration.LocalizationData)));
                yield return _gameConfiguration.LocalizationData != null;
            }

            InitGame();
        }

        private void InitGame()
        {
            InitLocalization();
            _levelLoader.InitData(_gameConfiguration.LevelsDatabaseData);

            _shop.Init(_userData);

            Windows.HUD.Init(_userData);

            StartLevel(CurrentLevelId);
            _currentMode = GameMode.Game;

            _tank.Completed += OnLevelComplete;
            _tank.Died += OnAnyPlayerUnitDied;
            _player.Died += OnAnyPlayerUnitDied;

            Windows.Loader.gameObject.SetActive(false);
            Inited?.Invoke();
        }

        private void InitLocalization()
        {
            _localizationDatabase.InitData(_gameConfiguration.LocalizationData);
            _gameLocalization = new GameLocalization();

            string locale = _userData.SavedLocale;

            if (locale.IsNullOrEmpty())
            {
                locale = GameLocalization.GetSystemLocaleByCapabilities();
                _userData.SavedLocale = locale;
            }

            _gameLocalization.LoadKeys(locale, _localizationDatabase.LocalizationKeys);
        }

        private void StartLevel(int levelId, bool restart = false)
        {
            _startLevelTime = Time.time;

            if (restart == false)
            {
                _levelLoader.LoadLevel(levelId);

                List<ItemCount> levelDropItems = Shop.ItemsDatabase.GetRandomBoosts(2);
                _levelLoader.CreateAirDrop(levelDropItems);
#if GAME_ANALYTICS
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_start", levelId);
# endif
            }

            _tank.OnLevelStarted(_levelLoader.TankSpawnPoint.position, _levelLoader.Waypoints);
            _player.OnLevelStarted(_levelLoader.PlayerSpawnPoint.position);

            Debug.Log($"Level {levelId + 1} started");

            _winLooseProcess = false;

            SetMode(GameMode.Puase);
            StartLevelWindow.Show(_userData.Badges, () => SetMode(GameMode.Game));
        }

        private void OnAnyPlayerUnitDied(Damageable unit)
        {
            if (_winLooseProcess)
                return;

            _winLooseProcess = true;

            float currentTime = Time.time;
            int elapsedTime = (int)(currentTime - _startLevelTime);
            LoosReason reason = unit is Tank ? LoosReason.Tank_died : LoosReason.Player_died;

#if GAME_ANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, CurrentLevelId.ToString(), reason.ToString(), elapsedTime);
#endif
            Debug.Log($"Level {CurrentLevelId + 1} loosed!");
            LevelFailedWindow.Show(RestartLevel);
        }

        private void OnLevelComplete()
        {
            if (_winLooseProcess)
                return;

            _winLooseProcess = true;

#if GAME_ANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_complete", CurrentLevelId);
#endif
            LevelCompleteWindow.Show(CurrentLevelId, Player.Badges, () => StartLevel(CurrentLevelId));

            _userData.LevelId++;
            Save();
        }

        private void RestartLevel()
        {
#if GAME_ANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "restart", CurrentLevelId);
#endif
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
