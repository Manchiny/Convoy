using Assets.Scripts.Items;
using Assets.Scripts.Levels;
using Assets.Scripts.Localization;
using Assets.Scripts.Saves;
using Assets.Scripts.Social;
using Assets.Scripts.Social.Adverts;
using Assets.Scripts.Sound;
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
        [Space]
        [SerializeField] private UserInput _joistickInput;
        [SerializeField] private UserInput _keyboardInput;
        [Space]
        [SerializeField] private GameSound _gameSound;

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
        public static event Action LevelStarted;
        public static event Action Loosed;
        public static event Action LevelCompleted;

        public event Action<GameMode> GameModeChanged;

        public enum GameMode
        {
            Game,
            Pause,
            PuaseTankView
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
        public static bool IsAllAlive => Player.IsAlive && Tank.IsAlive;

        public static WindowsController Windows => Instance._windowsController;

        public static YandexAdvertisingAdapter Adverts => Instance._adverts;
        public static YandexSocialAdapter SocialAdapter => Instance._yandexAdapter;

        public static UnitPropertiesDatabase TankProperies => Instance._tankPropertiesDatabase;
        public static UnitPropertiesDatabase CharacterProperties => Instance._playerCharacterPropertiesDatabase;

        public static GameLocalization Localization => Instance?._gameLocalization;
        public static GameConfiguration Configuration => Instance?._gameConfiguration;
        public static GameSound Sound => Instance._gameSound;

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
                _saver = new YandexSaver();
            else
                _saver = new LocalJSONSaver();

            yield return null;

            _saver.LoadUserData(data => StartCoroutine(Init(data)));
        }

        private void OnDestroy()
        {
            _tank.Completed -= OnLevelComplete;
            _tank.Died -= OnAnyPlayerUnitDied;
            _player.Died -= OnAnyPlayerUnitDied;
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
                case GameMode.Pause:
                    Pause();
                    break;
                case GameMode.PuaseTankView:
                    Pause();
                    break;
            }

            _currentMode = mode;
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

                if (onUse != null)
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

                if (item.Item.IsPropertyPoint == false)
                    Windows.Drops.Drop(item);
            }

            Save();
        }

        public void AddItems(ShopItem shopItem)
        {
            foreach (var item in shopItem.Items)
            {
                User.AddItemCount(item);

                if (item.Item.IsPropertyPoint == false)
                    Windows.Drops.Drop(item);
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

        public void SetSaver(Saver saver)
        {
            if (saver == null || saver.GetType() == _saver.GetType())
                return;

            _saver = saver;
            Save();
        }

        public void SetSound(bool needOn)
        {
            User.NeedSound = needOn;
            _gameSound.SetSoundEnebled(needOn);
            Save();
        }

        private IEnumerator Init(UserData userData)
        {
            _userData = userData;

            _player.InitData(_userData.PlayerCharacterData, _playerCharacterPropertiesDatabase);
            _tank.InitData(_userData.TankData, _tankPropertiesDatabase);

            yield return StartCoroutine(LoadGameConfiguration());
            yield return StartCoroutine(InitGame());
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

        }

        private IEnumerator InitGame()
        {
            InitLocalization();

            if (_yandexAdapter != null && _yandexAdapter.IsInited)
            {
                _adverts = new YandexAdvertisingAdapter();
                _adverts.Init(_yandexAdapter);
            }

            _levelLoader.InitData(_gameConfiguration.LevelsDatabaseData);

            _gameSound.Init();
            _shop.Init(_userData);
            Windows.HUD.Init(_userData);

            InitInputSystem();

            yield return StartCoroutine(StartLevel(CurrentLevelId));

            _currentMode = GameMode.Game;

            _tank.Completed += OnLevelComplete;
            _tank.Died += OnAnyPlayerUnitDied;
            _player.Died += OnAnyPlayerUnitDied;

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

        private void InitInputSystem()
        {
            Debug.Log($"Device type = {SystemInfo.deviceType}");

            Init(_joistickInput);
            Init(_keyboardInput);

            void Init(UserInput input)
            {
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
        }

        private IEnumerator StartLevel(int levelId, bool restart = false)
        {
            _startLevelTime = Time.time;
            bool levelBuilded = false;

            Windows.Loader.gameObject.SetActive(true);

            yield return new WaitForEndOfFrame();

            if (restart == false)
            {
                _levelLoader.LevelBuilded += OnLevelBuilded;
                yield return (levelBuilded == true);

                _levelLoader.LoadLevel(levelId);

                List<ItemCount> levelDropItems = Shop.ItemsDatabase.GetRandomBoosts(2);
                _levelLoader.CreateAirDrop(levelDropItems);

                LevelStarted?.Invoke();
#if GAME_ANALYTICS
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_start", levelId);
#endif
            }

            _tank.OnLevelStarted(_levelLoader.TankSpawnPoint.position, _levelLoader.Waypoints);
            _player.OnLevelStarted(_levelLoader.PlayerSpawnPoint.position);

            Debug.Log($"Level {levelId + 1} started");

            _input.UnFreeze();

            _winLooseProcess = false;

            Windows.Loader.gameObject.SetActive(false);
            SetMode(GameMode.PuaseTankView);

            yield return new WaitForEndOfFrame();

            StartLevelWindow.Show(() => SetMode(GameMode.Game));

            void OnLevelBuilded()
            {
                _levelLoader.LevelBuilded -= OnLevelBuilded;
                levelBuilded = true;
            }
        }

        private void OnAnyPlayerUnitDied(Damageable unit)
        {
            if (_winLooseProcess)
                return;

            _winLooseProcess = true;
            Loosed?.Invoke();
            _input.Freeze();

            float currentTime = Time.time;
            int elapsedTime = (int)(currentTime - _startLevelTime);
            LoosReason reason = unit is Tank ? LoosReason.Tank_died : LoosReason.Player_died;

#if GAME_ANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, CurrentLevelId.ToString(), reason.ToString(), elapsedTime);
#endif
            Debug.Log($"Level {CurrentLevelId + 1} loosed!");
            LevelFailedWindow.Show(OnContinue);

            void OnContinue()
            {
                if (_adverts != null && _adverts.NeedShowInterstitialAfterFail)
                    _adverts.TryShowInterstitial(() => StartCoroutine(RestartLevel()));
                else
                    StartCoroutine(RestartLevel());
            }
        }

        private void OnLevelComplete()
        {
            if (_winLooseProcess)
                return;

            _winLooseProcess = true;
            _input.Freeze();

#if GAME_ANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_complete", CurrentLevelId);
#endif
            LevelCompleteWindow.Show(CurrentLevelId, Player.Badges, OnContinue);
            _userData.LevelId++;

            if (SocialAdapter != null && SocialAdapter.IsInited)
                SocialAdapter.SetLeaderboardValue(YandexSocialAdapter.DefaultLeaderBoardName, CurrentLevelId + 1);

            LevelCompleted?.Invoke();

            Save();

            void OnContinue()
            {
                if (_adverts != null && _adverts.NeedShowInterstitialAfterLevel)
                    _adverts.TryShowInterstitial(() => StartCoroutine(StartLevel(CurrentLevelId)));
                else
                    StartCoroutine(StartLevel(CurrentLevelId));
            }
        }

        private IEnumerator RestartLevel()
        {
#if GAME_ANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "restart", CurrentLevelId);
#endif
            yield return StartCoroutine(StartLevel(CurrentLevelId, true));
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
