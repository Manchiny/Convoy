using Assets.Scripts.Units;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class TankPointer : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private Tank _tank;
        [SerializeField] private Player _player;
        [SerializeField] private TextMeshProUGUI _distanceText;

        private float _speed = 2000f;
        private float _screenOffset = 100f;

        private bool _isShown = false;
        private RectTransform _rectTransform;

        private Vector3 _targetPosition;
        private bool NeedShow => _player.InTankZone == false;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _content.gameObject.SetActive(false);
            _isShown = false;
        }

        private void Update()
        {
            if (NeedShow)
            {
                if (_isShown == false)
                {
                    _content.gameObject.SetActive(true);
                    _isShown = true;
                    _rectTransform.anchoredPosition = Vector3.zero;
                }

                SetIconSmoothedPosition();
                Rotate();
                UpdateDistanceText();
            }
            else
            {
                if (_isShown == true)
                {
                    _isShown = false;
                    _content.gameObject.SetActive(false);
                }
            }
        }

        private void UpdateDistanceText()
        {
            int distance = (int)Vector3.Distance(_player.transform.position, _tank.transform.position);
            _distanceText.text = $"{distance}m";
        }

        private void Rotate()
        {
            Quaternion lookRotation = Quaternion.LookRotation(_tank.transform.position - _player.transform.position);
            Quaternion rotation = transform.rotation;

            rotation.z = - lookRotation.y;
            rotation.w = lookRotation.w;

            transform.rotation = rotation;
        }

        public void SetIconSmoothedPosition()
        {
            _targetPosition = Camera.main.WorldToScreenPoint(_tank.transform.position);

            if (_targetPosition.x < _screenOffset)
                _targetPosition.x = _screenOffset;
            else if (_targetPosition.x > Screen.width - _screenOffset)
                _targetPosition.x = Screen.width - _screenOffset;

            if (_targetPosition.y < _screenOffset)
                _targetPosition.y = _screenOffset;
            else if (_targetPosition.y > Screen.height - _screenOffset)
                _targetPosition.y = Screen.height - _screenOffset;

            var step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
        }
    }
}
