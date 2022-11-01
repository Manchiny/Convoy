using System.Collections;
using UnityEngine;
using static Assets.Scripts.Game;

namespace Assets.Scripts
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _tank;
        [SerializeField] private Transform _tankUpgradePosition;
        [Space]
        [SerializeField] private Vector3 _baseOffset = new Vector3(0, 24, -10f);
        [SerializeField] private float _moveSpeedOnGame = 50f;
        [SerializeField] private float _moveSpeedOnShop = 1.5f;

        private Transform _target;
        private Transform _lookAtPoint;
        private float _currentSpeed;

        private Vector3 _offset;
        private Vector3 _smoothedPosition;

        private Coroutine _increasingSpeed;

        private void Start()
        {
            Instance.GameModeChanged += OnGameModeChanged;

            _currentSpeed = _moveSpeedOnGame;
            SetupOnGame();
        }

        private void LateUpdate()
        {
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition()
        {
            _smoothedPosition = Vector3.Lerp(transform.position, _target.position + _offset, _currentSpeed * Time.unscaledDeltaTime);
            transform.position = _smoothedPosition;
            transform.LookAt(_lookAtPoint);
        }

        private void OnGameModeChanged(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.Game:
                    SetupOnGame();
                    break;
                case GameMode.Puase:
                    SetupOnTankUpgrade();
                    break;
            }
        }

        private void SetupOnGame()
        {
            _target = _player;
            _offset = _baseOffset;
            _lookAtPoint = _player;

            if (_increasingSpeed != null)
                StopCoroutine(_increasingSpeed);

            StartCoroutine(IncreseSpeed(_moveSpeedOnGame));
        }

        private void SetupOnTankUpgrade()
        {
            if (_increasingSpeed != null)
                StopCoroutine(_increasingSpeed);

            _currentSpeed = _moveSpeedOnShop;
            _target = _tankUpgradePosition;
            _offset = Vector3.zero;
            _lookAtPoint = _tank;
        }

        private IEnumerator IncreseSpeed(float speed)
        {
            float step = 0.12f;

            while(_currentSpeed < speed)
            {
                _currentSpeed += step * Time.unscaledDeltaTime;
                yield return null;
            }

            _currentSpeed = speed;
        }


    }
}
