using Assets.Scripts.Levels;
using Assets.Scripts.Units;
using Assets.Scripts.UserInputSystem;
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

        private UserInput _input;
        private Level _currentLevel;

        public static Game Instance { get; private set; }

        public static Player Player => Instance._player;

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

        private void StartLevel(Level level)
        {
            if (_currentLevel != null)
                Destroy(_currentLevel.gameObject);

            _currentLevel = level;
            _tank.Init(level.SpawnPoint.position, level.Waypoints);
        }
    }
}
