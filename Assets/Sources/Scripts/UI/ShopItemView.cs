using Assets.Scripts.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private ShopItemName _shopItemName;
        [SerializeField] private BasicButton _buyButton;
        [SerializeField] private TextMeshProUGUI _adsText;
        [SerializeField] private Image _moneyImage;

        private const string FreeLocalizationKey = "free";
        private const string AdsLockalizationKey = "ad";

        protected BasicButton BuyButton => _buyButton;
        protected ShopItem ShopItem { get; private set; }

        private void OnDestroy()
        {
            Game.Localization.LanguageChanged -= SetText;
        }

        private void Awake()
        {
            ShopItem = ShopItemsLibrary.GetShopItemByName(_shopItemName);
        }

        public virtual void Init()
        {
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

            SetText();

            _buyButton.SetOnClick(OnButtonBuyClicked);
        }

        protected void OnButtonBuyClicked()
        {
            _buyButton.SetLock(true);
            Game.Shop.TryBuyItem(ShopItem, OnSuccesBuy, OnBuyFail);
        }

        protected virtual void OnSuccesBuy()
        {
            _buyButton.SetLock(false);
        }

        protected virtual void OnBuyFail()
        {
            _buyButton.SetLock(false);
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
