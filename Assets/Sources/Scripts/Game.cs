using Assets.Scripts.Units;
using Assets.Scripts.UserInputSystem;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Player _player;

        private UserInput _input;
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
    }
}
