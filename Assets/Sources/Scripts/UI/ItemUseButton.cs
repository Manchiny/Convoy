using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ItemUseButton : BasicButton
    {
        [SerializeField] private ItemName _itemName;
        [SerializeField] private TextMeshProUGUI _countText;

        private UserData _user;

        private Item _item;
        private bool _hasCount;

        private bool _locked;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _user.ItemsChanged -= UpdateCountText;
        }

        public void Init(UserData data)
        {
            _item = ItemsLibrary.GetItem(_itemName);
            SetOnClick(TryUse);

            _user = data;

            _user.ItemsChanged += UpdateCountText;

            UpdateCountText(_itemName);
        }

        private void UpdateCountText(ItemName name)
        {
            if (name != _itemName)
                return;

            int count = 0;

            if (_user.HasItem(_itemName, out ItemCount item))
                count = item.Count;

            _countText.text = count.ToString();
        }

        private void TryUse()
        {
            if(Game.Instance.TryUseItem(_item, OnEffectEnded))
            {
                Debug.Log($"Boost use: {_item.Name}");
            }

            Debug.Log($"Boost use failed: {_item.Name}!");
        }

        private void OnEffectEnded()
        {
            Debug.Log("Boost effect ended! ");
        }
    }
}
