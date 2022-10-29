using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Items.Item;

namespace Assets.Scripts.UI
{
    public class ItemUseButton : BasicButton
    {
        [SerializeField] private ItemName _itemName;

        private Item _item;
        private bool _hasCount;

        private bool _locked;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _item = ItemsLibrary.GetItem(_itemName);
            SetOnClick(TryUse);
        }

        private void TryUse()
        {
            if( Game.Instance.TryUseItem(_item, OnEffectEnded))
            {
                Debug.Log($"Boost use: {_item.Name}");
            }

            Debug.Log($"Boost use failed: {_item.Name}!");
        }

        private void OnEffectEnded()
        {
            Debug.Log("Boost effect ended! ");
        }
    }
}
