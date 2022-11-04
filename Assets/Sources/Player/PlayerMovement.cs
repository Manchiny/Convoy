using System;
using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : MonoBehaviour
    {
        private const float MaxAngleForAnimationTransition = 90f;

        private const int AnimatorHandsLayerId = 1;
        private const int AnimatorFullBodyLayerId = 2;

        private const string AnimatorSpeedKey = "Speed";
        private const string AnimatorSideSpeedKey = "SideSpeed";
        private const string AnimatorDiedKey = "Died";

        private Player _player;

        private float _speed = 5f;
        private float _rotationSpeed = 15f;

        private Animator _animator;
        private Rigidbody _rigidbody;

        private float _speedForward;
        private float _speedSide;

        private bool _died;

        private Vector3 _inputDirection;

        public event Action OnMovementStarted;
        public event Action OnMovementStoped;
        public bool IsStoped { get; private set; }

        private float SpeedForward
        {
            get => _speedForward;
            set
            {
                _speedForward = value;
                _animator.SetFloat(AnimatorSpeedKey, value);
            }
        }

        private float SpeedSide
        {
            get => _speedSide;
            set
            {
                _speedSide = value;
                _animator.SetFloat(AnimatorSideSpeedKey, value);
            }
        }

        private void Awake()
        {
            _player = GetComponent<Player>();
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();

            _player.Died += OnPlayerDied;
            Game.LevelStarted += OnLevelStarted;
            Game.Restarted += OnLevelStarted;
        }

        private void FixedUpdate()
        {
            if (_player.IsAlive || _died == false)
            {
                Move();
                Rotate();
            }
        }

        private void OnDestroy()
        {
            _player.Died -= OnPlayerDied;
        }

        public void SetInputDirection(Vector3 inputDirection)
        {
            if (_player.IsAlive == false || _died)
                return;

            if (IsStoped == false && inputDirection == Vector3.zero)
            {
                IsStoped = true;
                OnMovementStoped?.Invoke();
            }
            else if (IsStoped == true && inputDirection != Vector3.zero)
            {
                IsStoped = false;
                OnMovementStarted?.Invoke();
            }

            _inputDirection = inputDirection;
        }

        private void Move()
        {
            Vector3 direction = _inputDirection * _speed;
            direction.y = 0;
            _rigidbody.velocity = direction;

            if (_inputDirection == Vector3.zero)
            {
                SpeedForward = 0;
                SpeedSide = 0;

                return;
            }

            float forwardAngle = Vector3.Angle(_inputDirection.normalized, transform.forward);
            float sideAngle = Vector3.Angle(_inputDirection.normalized, transform.right);

            float speedForward = 1 - (forwardAngle / MaxAngleForAnimationTransition);
            SpeedForward = speedForward;

            float speedSide = 1 - (sideAngle / MaxAngleForAnimationTransition);
            SpeedSide = speedSide;
        }

        private void Rotate()
        {
            Vector3 lookPos = Vector3.zero;

            if (_player.Target != null)
                lookPos = _player.Target.transform.position - transform.position;
            else if (_inputDirection != Vector3.zero)
                lookPos = _inputDirection.normalized;
            else
                return;

            lookPos.y = 0;

            Quaternion playerRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, _rotationSpeed * Time.fixedDeltaTime);
        }

        private void OnPlayerDied(Damageable unit)
        {
            _died = true;
            Debug.Log("PlayerDied");

            SpeedForward = 0;
            SpeedSide = 0;

            _animator.SetLayerWeight(AnimatorHandsLayerId, 0);
            _animator.SetLayerWeight(AnimatorFullBodyLayerId, 1);

            _animator.SetBool(AnimatorDiedKey, true);
        }

        private void OnLevelStarted() 
        {
            _died = false;
            Debug.Log("OnLevelRestarted");

            _animator.SetLayerWeight(AnimatorHandsLayerId, 1);
            _animator.SetLayerWeight(AnimatorFullBodyLayerId, 0);

            _animator.SetBool(AnimatorDiedKey, false);

            SpeedForward = 0;
            SpeedSide = 0;

        }
    }
}
