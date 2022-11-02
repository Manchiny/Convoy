using Assets.Scripts.Items;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ShopWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private ShopItemView _shopItemViewPrefab;
        [SerializeField] private RectTransform _viewsContainer;
        [SerializeField] private TextMeshProUGUI _badgesCount;

        private const string ShopLocalizationKey = "shop";

        public override string LockKey => "ShopWindow";
        public override bool AnimatedClose => true;

        public static ShopWindow Show() =>
                       Game.Windows.ScreenChange<ShopWindow>(false, w => w.Init());

        protected override void Awake()
        {
            base.Awake();
            Debug.LogWarning($"{LockKey}: on Awake!");
        }

        private void OnDestroy()
        {
            Game.User.BadgesChaged -= OnBadgesCountChanged;
        }

        private void Init()
        {
            List<ShopItem> shopItems = Game.Shop.ShopItemsDatabase.GetItemsForShop();

            foreach (var item in shopItems)
            {
                var view = Instantiate(_shopItemViewPrefab, _viewsContainer);
                view.Init(item, null);
            }

            SetText();

            OnBadgesCountChanged(Game.User.Badges);
            Game.User.BadgesChaged += OnBadgesCountChanged;
        }

        protected override void SetText()
        {
            _titleText.text = ShopLocalizationKey.Localize();
        }

        private void OnBadgesCountChanged(int count)
        {
            _badgesCount.text = count.ToString();
        }

    }
}
