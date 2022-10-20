using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.UserInputSystem
{
    public class KeyboardInput : UserInput
    {
        private PlayerInput _input;

        public override bool NeedActivate => SystemInfo.deviceType == DeviceType.Desktop;

        private void OnEnable()
        {
            _input?.Enable();
        }

        private void OnDisable()
        {
            _input?.Disable();
        }

        public override void Init(PlayerMovement character)
        {
            _input = new PlayerInput();
            _input.Enable();
            base.Init(character);
        }

        protected override void GetInputVector()
        {
            InputVector = _input.Player.Move.ReadValue<Vector2>();
        }
    }
}
