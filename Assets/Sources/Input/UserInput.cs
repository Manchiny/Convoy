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

        protected bool Freezed { get; private set; }

        public virtual void Init(PlayerMovement character)
        {
            _characterMovement = character;
            Freezed = false;
        }

        public abstract bool NeedActivate();

        private void Update()
        {
            if (_characterMovement == null)
                return;

            if(Freezed == false)
                GetInputVector();

            SetPlayerMoveDirection();
        }

        public void Freeze()
        {
            Freezed = true;
            InputVector = Vector2.zero;
        }

        public void UnFreeze()
        {
            Freezed = false;
        }


        protected abstract void GetInputVector();

        private void SetPlayerMoveDirection()
        {
            Direction = new Vector3(Horizontal, 0, Vertical);
            _characterMovement.SetInputDirection(Direction);
        }
    }
}