using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu]
    public class ItemsDatabase : ScriptableObject
    {
        [SerializeField] private List<Item> _items;

        private const float BoostEffectTime = 30f;

        public Item GetItem(ItemName name)
        {
            Item item = _items.Where(item => item.Name == name).FirstOrDefault();

            if(item == null)
                Debug.Log("Items library don't content item " + name);

            return item;
        }

        public List<ItemCount> GetRandomBoosts(int count)
        {
            List<ItemCount> items = new();

            List<Item> boosts = _items.Where(item => item.IsTemporary).ToList();

            int random = 0;

            for (int i = 0; i < count; i++)
            {
                random = Random.Range(0, boosts.Count);
                Item item = boosts[random];
                AddItemInCollection(items, item);
            }

            return items;
        }

        private void AddItemInCollection(List<ItemCount> items, Item item)
        {
            ItemCount itemCount = items.Where(i => i.Name == item.Name).FirstOrDefault();

            if (itemCount == null)
                items.Add(new ItemCount(item.Name, 1));
            else
                itemCount.Count++;
        }
    }
}
