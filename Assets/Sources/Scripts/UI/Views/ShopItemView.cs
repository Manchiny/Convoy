using Assets.Scripts.Items;
using Assets.Scripts.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ShopItemView : MonoBehaviour
    {
        [Header("Will be ignore if null")]
        [SerializeField] private ItemView _itemViewPrefab;
        [SerializeField] private RectTransform _itemViewContainer;
        [Space]
        [SerializeField] private BasicButton _buyButton;
        [SerializeField] private TextMeshProUGUI _adsText;
        [SerializeField] private Image _moneyImage;

        private const string FreeLocalizationKey = "free";
        private const string AdsLockalizationKey = "ad";

        protected ShopItem ShopItem { get; set; }
        protected Unit Unit { get; private set; }
        protected BasicButton BuyButton => _buyButton;

        private void OnDestroy()
        {
            Game.Localization.LanguageChanged -= SetText;
        }

        public virtual void Init(ShopItem shopItem, Unit unit)
        {
            if (shopItem != null)
                ShopItem = shopItem;

            Unit = unit;

            if (ShopItem.MoneyType == ShopItem.MoneyTypes.Ads)
            {
                Game.Localization.LanguageChanged += SetText;
                _moneyImage.gameObject.SetActive(false);
            }
            else if (ShopItem.MoneyType == ShopItem.MoneyTypes.Soft)
            {
                _moneyImage.gameObject.SetActive(true);
                _adsText.gameObject.SetActive(false);
            }

            if(_itemViewContainer != null && _itemViewPrefab != null)
            {
                for (int i = 0; i < 1; i++)
                {
                    ItemCount item = shopItem.Items[i];
                    var itemView = Instantiate(_itemViewPrefab, _itemViewContainer);
                    itemView.Init(item);
                }
            }

            SetText();

            _buyButton.SetOnClick(OnButtonBuyClicked);
        }

        protected void OnButtonBuyClicked()
        {
            Debug.Log($"[Item] {ShopItem.Name} button buy clicked;");
            _buyButton.SetLock(true);

            Game.Shop.TryBuyItem(ShopItem, OnSuccesBuy, OnBuyFail);
        }

        protected virtual void OnSuccesBuy()
        {
            _buyButton.SetLock(false);
            Debug.Log($"[Item] {ShopItem.Name} on succes buy");
        }

        protected virtual void OnBuyFail(string reason)
        {
            _buyButton.SetLock(false);
            Debug.Log($"[Item] {ShopItem.Name} on buy fail: " + reason);

            var buttonRect = _buyButton.transform as RectTransform;
            Game.Windows.ShowFloatingText(reason, buttonRect.position);
        }

        private void SetText()
        {
            if (ShopItem.MoneyType == ShopItem.MoneyTypes.Ads)
            {
                _buyButton.Text = FreeLocalizationKey.Localize();
                _adsText.text = AdsLockalizationKey.Localize();
            }
            else
            {
                _buyButton.Text = ShopItem.Cost.ToString();
            }
        }
    }
}
