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
                onSucces?.Invoke();
            }
            else if (shopItem.MoneyType == ShopItem.MoneyTypes.Ads && Game.Adverts != null)
            {
                Game.Adverts.TryShowRewardedVideo(null, () => rewarded = true, TryAddRewardedItems, null);
            }

            void TryAddRewardedItems()
            {
                if (rewarded)
                {
                    AddItems(shopItem);
                    onSucces?.Invoke();
                }
                else
                {
                    Debug.Log("Rewarded video not watched");
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

            Game.Instance.Save();
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
