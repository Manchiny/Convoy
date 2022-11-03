using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public abstract class RewardedWindow : AbstractWindow
    {
        [SerializeField] private ItemView _itemViewPrefab;
        [SerializeField] private RectTransform _viewsContainer;

        protected List<ItemCount> Items;
        protected Action RewardGot;

        public override string LockKey => "RewardedWindow";
        public override bool AnimatedClose => true;
        public override bool NeedHideHudOnShow => true;

        protected virtual void Init(List<ItemCount> items, Action onGetReward)
        {
            RewardGot = onGetReward;
            Items = items;

            Game.Instance.SetMode(Game.GameMode.Pause);

            foreach (var item in items)
            {
                ItemView view = Instantiate(_itemViewPrefab, _viewsContainer);
                view.Init(item);
            }
        }

        protected override void SetText() { }
    }
}
