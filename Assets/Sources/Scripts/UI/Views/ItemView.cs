using Assets.Scripts.Items;
using Assets.Scripts.Units;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Image _shopItemIcon;
        [Space]
        [SerializeField] private RectTransform _timePanel;
        [SerializeField] private TextMeshProUGUI _timeText;
        [Space]
        [SerializeField] private RectTransform _tankOwnerLable;
        [SerializeField] private RectTransform _playerOwnerLable;
        [Space]
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private TextMeshProUGUI _valueText;

        private const string SecondsLocalizationKey = "sec";

        private Item _item;

        public virtual void Init(ItemCount itemCount)
        {
            _item = itemCount.Item;

            if (_shopItemIcon != null)
            {
               if (_item.Icon != null)
                    _shopItemIcon.sprite = _item.Icon;
            }

            if (_item.IsTemporary)
                _timeText.text = $"{_item.BoostSeconds} " + SecondsLocalizationKey.Localize();
            else
                _timePanel.gameObject.SetActive(false);

            _tankOwnerLable.gameObject.SetActive(_item.Owner == ItemOwner.Tank);
            _playerOwnerLable.gameObject.SetActive(_item.Owner == ItemOwner.Player);

            if (itemCount.Count > 1)
                _countText.text = itemCount.Count.ToString();
            else
                _countText.gameObject.SetActive(false);

            _valueText.text = _item.GetValueFormatedString();
        }
    }
}
