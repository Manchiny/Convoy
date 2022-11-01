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
    }
}
