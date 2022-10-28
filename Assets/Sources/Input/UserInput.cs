using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.UserInputSystem
{
    public abstract class UserInput : MonoBehaviour
    {
        protected Vector3 Direction;
        protected Vector2 InputVector;

        private PlayerMovement _characterMovement;

        protected virtual float Horizontal => InputVector.x;
        protected virtual float Vertical  => InputVector.y;

        private void Awake()
        {
            Game.Inited += OnGameInited;
        }

        private void OnDestroy()
        {
            Game.Inited -= OnGameInited;
        }

        public void OnGameInited()
        {
            Game.Inited -= OnGameInited;
            Game.Instance.SetInputSystem(this);
        }

        public virtual void Init(PlayerMovement character)
        {
            _characterMovement = character;
        }

        public abstract bool NeedActivate();

        private void Update()
        {
            if (_characterMovement == null)
                return;

            GetInputVector();
            SetPlayerMoveDirection();
        }

        protected abstract void GetInputVector();

        private void SetPlayerMoveDirection()
        {
            Direction = new Vector3(Horizontal, 0, Vertical);
            _characterMovement.SetInputDirection(Direction);
        }
    }
}