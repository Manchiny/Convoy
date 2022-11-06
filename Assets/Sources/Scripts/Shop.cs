using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private ItemsDatabase _itemsDatabase;
        [SerializeField] private ShopItemsDatabase _shopItemsDatabase;

        private UserData _user;
        public ItemsDatabase ItemsDatabase => _itemsDatabase;
        public ShopItemsDatabase ShopItemsDatabase => _shopItemsDatabase;

        public void Init (UserData userData)
        {
            _user = userData;
        }

        public void TryBuyItem(ShopItem shopItem, Action onSucces = null, Action<string> onFail = null)
        {
            bool rewarded = false;

            if (shopItem.MoneyType == ShopItem.MoneyTypes.Soft)
            {
                if(_user.TryBuyItem(shopItem))
                {
                    Game.Instance.AddItems(shopItem);

                    if (onSucces != null)
                        onSucces?.Invoke();
                }
                else
                {
                    onFail.Invoke("not enough badges");
                }
            }
            else if (shopItem.MoneyType == ShopItem.MoneyTypes.Ads)
            {
#if UNITY_EDITOR
                rewarded = true;
                TryAddRewardedItems();
#else
                if (Game.Adverts != null)
                {
                     Game.Adverts.TryShowRewardedVideo(null, () => rewarded = true, TryAddRewardedItems, null);
                }
#endif
            }

            void TryAddRewardedItems()
            {
                Debug.Log("[Shop] Try add items. Rewarde = " + rewarded);

                if (rewarded)
                {
                    Game.Instance.AddItems(shopItem);

                    if (onSucces != null)
                        onSucces?.Invoke();
                }
                else
                {
                    Debug.Log("Rewarded video not watched");

                    if (onFail != null)
                        onFail?.Invoke("Rewarded video not watched");
                }
            }
        }
    }
}
