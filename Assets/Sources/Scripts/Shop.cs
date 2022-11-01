using Assets.Scripts.Items;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Shop
    {
        private UserData _user;

        public Shop(UserData userData)
        {
            _user = userData;
        }

        public void TryBuyItem(ShopItem shopItem, Action onSucces = null, Action onFail = null)
        {
            bool rewarded = false;

            if (shopItem.MoneyType == ShopItem.MoneyTypes.Soft && _user.TryBuyItem(shopItem))
            {
                AddItems(shopItem);

                if(onSucces !=null)
                    onSucces?.Invoke();
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
                    AddItems(shopItem);

                    if (onSucces != null)
                        onSucces?.Invoke();
                }
                else
                {
                    Debug.Log("Rewarded video not watched");

                    if (onFail != null)
                        onFail?.Invoke();
                }
            }
        }

        private void AddItems(ShopItem shopItem)
        {
            foreach (var item in shopItem.Items)
            {
                _user.AddItemCount(item);
            }
        }

        //private void AddItemToUser()
        //{
        //    Item item = ItemsLibrary.GetItem(ItemName.PlayerDoubleArmorBoost);
        //    _userData.AddItem(item);
        //    Save();
        //}

        private void OnRewardeBuyFailed()
        {

        }
    }
}
