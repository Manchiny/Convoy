using Assets.Scripts.Levels;
using Assets.Scripts.UI;
using Assets.Scripts.Units;
using Assets.Scripts.UserInputSystem;
using System;
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

        private UserInput _input;
        private GameMode _currentMode;

        public event Action<GameMode> GameModeChanged;

        public enum GameMode
        {
            Game,
            TankUpgrade,
        }

        public Level CurrentLevel { get; private set; }
        public static Game Instance { get; private set; }

        public static Player Player => Instance._player;
        public static WindowsController Windows => Instance._windowsController;
        public static Transform GarbageHolder => Instance._sceneGarbageHolder;
        public static GameMode CurrentMode => Instance._currentMode;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(this);
                return;
            }

            Destroy(this);
        }

        private void Start()
        {
            StartLevel(_levels[0]);
            _currentMode = GameMode.Game;
        }

        public void SetInputSystem(UserInput input)
        {
            Debug.Log($"Device type = {SystemInfo.deviceType}");
            if (input.NeedActivate == false)
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
            _tank.Init(level.SpawnPoint.position, level.Waypoints);
        }
    }
}
