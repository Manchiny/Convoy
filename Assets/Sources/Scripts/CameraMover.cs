using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [Space]
        [SerializeField] private Vector3 _offset = new Vector3(0, 4, -5.5f);
        [SerializeField] private float _moveSpeed = 10f;

        private Vector3 _targetPosition;
        private Vector3 _smoothedPosition;

        private void LateUpdate()
        {
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition()
        {
            _targetPosition = _offset + _target.position;
          //  _targetPosition.y = _handsMover.GetAverageValue - _offset.y;

            _smoothedPosition = Vector3.Lerp(transform.position, _targetPosition, _moveSpeed * Time.fixedDeltaTime);
            transform.position = _smoothedPosition;
        }
    }
}
