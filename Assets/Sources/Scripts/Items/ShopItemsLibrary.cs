using System.Collections.Generic;
using System.Linq;
using static Assets.Scripts.Items.ShopItem;

namespace Assets.Scripts.Items
{
    public class ShopItemsLibrary
    {
        private const int PlayerPropertyCostSoft = 50;

        private static readonly List<ShopItem> ShopItems = new List<ShopItem>
        {
#region Player Property points by ads
            {new ShopItem(ShopItemName.PlayerArmorPropertyPointAds, 0, MoneyTypes.Ads,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.PlayerPropertyPointArmor), 1)}
                    )},

            {new ShopItem(ShopItemName.PlayerMaxHealthPropertyPointAds, 0, MoneyTypes.Ads,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.PlayerPropertyPointMaxHealth), 1)}
                    )},

             {new ShopItem(ShopItemName.PlayerDamagePropertyPointAds, 0, MoneyTypes.Ads,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.PlayerPropertyPointDamage), 1)}
                    )},

            {new ShopItem(ShopItemName.PlyaerShootingSpeedPropertyAds, 0, MoneyTypes.Ads,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.PlayerPropertyPointShootingSpeed), 1)}
                    )},
#endregion

#region Player Property points by badges
            {new ShopItem(ShopItemName.PlayerArmorPropertyPointSoft, PlayerPropertyCostSoft, MoneyTypes.Soft,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.PlayerPropertyPointArmor), 1)}
                    )},

            {new ShopItem(ShopItemName.PlayerMaxHealthPropertyPointSoft, PlayerPropertyCostSoft, MoneyTypes.Soft,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.PlayerPropertyPointMaxHealth), 1)}
                    )},

             {new ShopItem(ShopItemName.PlayerDamagePropertyPointSoft, PlayerPropertyCostSoft, MoneyTypes.Soft,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.PlayerPropertyPointDamage), 1)}
                    )},

            {new ShopItem(ShopItemName.PlyaerShootingSpeedPropertySoft, PlayerPropertyCostSoft, MoneyTypes.Soft,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.PlayerPropertyPointShootingSpeed), 1)}
                    )},
#endregion

#region Tank Property points by ads
            {new ShopItem(ShopItemName.TankArmorPropertyPointAds, 0, MoneyTypes.Ads,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.TankPropertyPointArmor), 1)}
                    )},

            {new ShopItem(ShopItemName.TankMaxHealthPropertyPointAds, 0, MoneyTypes.Ads,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.TankPropertyPointMaxHealth), 1)}
                    )},

             {new ShopItem(ShopItemName.TankDamagePropertyPointAds, 0, MoneyTypes.Ads,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.TankPropertyPointDamage), 1)}
                    )},

            {new ShopItem(ShopItemName.TankShootingSpeedPropertyAds, 0, MoneyTypes.Ads,
                new ItemCount[]
                    {new ItemCount(ItemsLibrary.GetItem(ItemName.TankPropertyPointShootingSpeed), 1)}
                    )},
#endregion

        };

        public static ShopItem GetShopItemByName(ShopItemName name)
        {
            return ShopItems.Where(item => item.Name == name).FirstOrDefault();
        }
    }
}