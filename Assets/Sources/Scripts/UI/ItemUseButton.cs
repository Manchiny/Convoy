using Assets.Scripts.Items;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ItemUseButton : BasicButton
    {
        [SerializeField] private ItemName _itemName;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private Image _coodawnFiller;

        private UserData _user;

        private Item _item;
        private bool _hasCount;

        private Tween _fillerAnimation;
        private bool _locked;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _user.ItemsChanged -= UpdateCountText;
        }

        public void Init(UserData data)
        {
            _item = Game.Shop.ItemsDatabase.GetItem(_itemName);
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
            if (Game.IsAllAlive == false)
                return;

            if(Game.Instance.TryUseItem(_item, OnEffectEnded, null))
            {
                if(_item.IsTemporary)
                {
                    Disable(true);
                    ShowCooldawnAnimation(_item.BoostSeconds);
                }

                Debug.Log($"Item used: {_item.Name}");
                return;
            }

            Debug.Log($"Item use failed: {_item.Name}!");
        }

        private void OnEffectEnded()
        {
            Debug.Log("Boost effect ended! ");
            Disable(false);
        }

        private void Disable(bool value)
        {
            SetLock(value);
        }

        private void ShowCooldawnAnimation(float cooldawn)
        {
            if (_fillerAnimation != null && _fillerAnimation.IsActive())
                _fillerAnimation.Kill();

            _coodawnFiller.fillAmount = 0;

            _fillerAnimation = _coodawnFiller.DOFillAmount(1, cooldawn).SetLink(gameObject).SetEase(Ease.Linear);
        }
    }
}
