using UnityEngine;

namespace Assets.Scripts.Characters
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : MonoBehaviour
    {
        private const float MaxAngleForAnimationTransition = 90f;
        private Player _player;

        private float _speed = 3.5f;
        private float _rotationSpeed = 7.5f;

        private Animator _animator;
        private Rigidbody _rigidbody;

        private float _speedForward;
        private float _speedSide;

        private Vector3 _inputDirection;

        private float SpeedForward
        {
            get => _speedForward;
            set
            {
                _speedForward = value;
                _animator.SetFloat("Speed", value);
            }
        }

        private float SpeedSide
        {
            get => _speedSide;
            set
            {
                _speedSide = value;
                _animator.SetFloat("SideSpeed", value);
            }
        }

        private void Awake()
        {
            _player = GetComponent<Player>();
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
        }

        public void SetInputDirection(Vector3 inputDirection)
        {
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
    }
}
