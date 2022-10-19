using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Scripts.UserInputSystem
{
    public abstract class UserInput : MonoBehaviour
    {
        protected Vector3 Direction;
        protected Vector2 InputVector;

        private PlayerMovement _characterMovement;

        public abstract bool NeedActivate { get; }

        protected virtual float Horizontal => InputVector.x;
        protected virtual float Vertical => InputVector.y;

        protected virtual void Start()
        {
            Game.Instance.SetInputSystem(this);
        }

        public virtual void Init(PlayerMovement character)
        {
            _characterMovement = character;
        }

        private void Update()
        {
            if (_characterMovement == null)
                return;

            GetInputVector();
            SetPlayermoveDirection();
        }

        protected abstract void GetInputVector();

        private void SetPlayermoveDirection()
        {
            Direction = new Vector3(Horizontal, 0, Vertical);
            _characterMovement.SetInputDirection(Direction);
        }
    }
}