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

        private ShopItem _shopItem;

        private void OnDestroy()
        {
            Game.Localization.LanguageChanged -= SetText;
        }

        private void Start()
        {
            _shopItem = ShopItemsLibrary.GetShopItemByName(_shopItemName);
        }

        public void Init()
        {
            if (_shopItem.MoneyType == ShopItem.MoneyTypes.Ads)
            {
                Game.Localization.LanguageChanged += SetText;
                _moneyImage.gameObject.SetActive(false);
            }
            else if (_shopItem.MoneyType == ShopItem.MoneyTypes.Soft)
            {
                _moneyImage.gameObject.SetActive(true);
                _adsText.gameObject.SetActive(false);
            }

            SetText();

            _buyButton.SetOnClick(OnButtonBuyClicked);
        }

        protected void OnButtonBuyClicked()
        {
            Game.Shop.TryBuyItem(_shopItem, OnSuccesBuy, OnBuyFail);
            _buyButton.SetLock(true);
        }

        protected virtual void OnSuccesBuy()
        {
            _buyButton.SetLock(true);
        }

        protected virtual void OnBuyFail()
        {
            _buyButton.SetLock(true);
        }

        private void SetText()
        {
            if (_shopItem.MoneyType == ShopItem.MoneyTypes.Ads)
            {
                _buyButton.Text = FreeLocalizationKey.Localize();
                _adsText.text = AdsLockalizationKey.Localize();
            }
            else
            {
                _buyButton.Text = _shopItem.Cost.ToString();
            }
        }
    }
}
