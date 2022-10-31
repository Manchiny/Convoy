using Assets.Scripts.Items;
using Assets.Scripts.Levels;
using Assets.Scripts.Localization;
using Assets.Scripts.Saves;
using Assets.Scripts.Social;
using Assets.Scripts.Social.Adverts;
using Assets.Scripts.UI;
using Assets.Scripts.Units;
using Assets.Scripts.UserInputSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

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

        private YandexSocialAdapter _yandexAdapter;
        private YandexAdvertisingAdapter _adverts;

        private GameConfiguration _gameConfiguration;

        private UserData _userData;
        private Saver _saver;
        private GameLocalization _gameLocalization;

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
        public static Tank Tank => Instance._tank;

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
            _yandexAdapter = FindObjectOfType<YandexSocialAdapter>();

            if(_yandexAdapter != null && _yandexAdapter.IsInited)
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
                RestartLevel();

            if (Input.GetKeyDown(KeyCode.Q) == true)
                AddItemToUser();
        }

        private void AddItemToUser()
        {
            Item item = ItemsLibrary.GetItem(ItemName.PlayerDoubleArmorBoost);
            _userData.AddItem(item);
            Save();
        }

#endif
        private void OnDestroy()
        {
            _tank.Completed -= OnLevelComplete;
            _tank.Died -= OnAnyPlayerUnitDied;
            _player.Died -= OnAnyPlayerUnitDied;
        }

        public static string Localize(string key, params string[] parameters) => Localization?.Localize(key, parameters) ?? key;

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

        public bool TryUseItem(Item item, Action onEffectEnded)
        {
            if(_userData.TryUseItem(item, onEffectEnded))
            {
                ItemsUseHandler.UseItem(item);
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

        public void ChangeLocale(string local)
        {
            if (local == GameLocalization.CurrentLocale)
                return;

            _gameLocalization.LoadKeys(local, _localizationDatabase.LocalizationKeys);
            _userData.SavedLocale = GameLocalization.CurrentLocale;
        }

        private void InitData(UserData userData)
        {
            _userData = userData;

            _tank.InitData(_userData.TankData, _tankPropertiesDatabase);
            _player.InitData(_userData.PlayerCharacterData, _playerCharacterPropertiesDatabase);

            StartCoroutine(LoadGameConfiguration());
        }

        private IEnumerator LoadGameConfiguration()
        {
            yield return StartCoroutine(LoadTextFromServer(GameConstants.GameConfigURL, (result) => ParseToDataOrCreateNew(result, out _gameConfiguration)));
            yield return _gameConfiguration != null;

            if (_gameConfiguration.NeedUpdatedLevels(_levelLoader.DatabaseVesrsion))
            {
                yield return StartCoroutine(LoadTextFromServer(GameConstants.LevelsDataURL, (result) => ParseToDataOrCreateNew(result, out _gameConfiguration.LevelsDatabaseData)));
                yield return _gameConfiguration.LevelsDatabaseData != null;
            }

            if (_gameConfiguration.NeedUpdateLocalizations(_localizationDatabase.Version))
            {
                yield return StartCoroutine(LoadTextFromServer(GameConstants.LocalizationsURL, (result) => ParseToDataOrCreateNew(result, out _gameConfiguration.LocalizationData)));
                yield return _gameConfiguration.LocalizationData != null;
            }

            InitGame();
        }

        private void InitGame()
        {
            InitLocalization();
            _levelLoader.InitData(_gameConfiguration.LevelsDatabaseData);

            Windows.HUD.Init(_userData);

            StartLevel(_userData.LevelId);
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

        private void ParseToDataOrCreateNew<T>(string parseText, out T resultOut) where T : class, new()
        {
            try
            {
                T result = JsonUtility.FromJson<T>(parseText);
                resultOut = result;
            }
            catch
            {
                Debug.Log($"Error parse data to {typeof(T)}");
                resultOut = new T();
            }
        }

        private IEnumerator LoadTextFromServer(string url, Action<string> onComplete)
        {
            UnityWebRequest request = null;

            try
            {
                request = UnityWebRequest.Get(url);
            }
            catch
            {
                Debug.LogError("[Game] Error connection!");
                onComplete?.Invoke(null);

                yield break;
            }

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.DataProcessingError || request.result != UnityWebRequest.Result.ProtocolError && request.result != UnityWebRequest.Result.ConnectionError)
            {
                onComplete?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat($"[Game] error request {url}, { request.error}");
                onComplete?.Invoke(null);
            }

            request.Dispose();
        }
    }
}
