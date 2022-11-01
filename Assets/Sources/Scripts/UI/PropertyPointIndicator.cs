using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Image))]
    public class PropertyPointIndicator : MonoBehaviour
    {
        private Image _image;

        private Color _baseColor;
        private Color _upgradedColor;

        public bool IsUpgraded { get; private set; }

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Init(Color baseColor, Color onUpgradedColor)
        {
            _baseColor = baseColor;
            _upgradedColor = onUpgradedColor;

            Reset();
        }

        public void SetUpgraded()
        {
            _image.color = _upgradedColor;
            IsUpgraded = true;
        }

        public void Reset()
        {
            if(_image == null)
                _image = GetComponent<Image>();

            _image.color = _baseColor;
            IsUpgraded = false;
        }
    }
}
