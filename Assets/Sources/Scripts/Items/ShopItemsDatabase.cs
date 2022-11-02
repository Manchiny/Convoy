using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu]
    public class ShopItemsDatabase : ScriptableObject
    {
        [SerializeField] private List<ShopItem> _shopItems;
       
        public ShopItem GetShopItemByName(ShopItemName name)
        {
            return _shopItems.Where(item => item.Name == name).FirstOrDefault();
        }

        public List<ShopItem> GetItemsForShop() => _shopItems.Where(item => item.ShowInShop == true).ToList();
    }
}