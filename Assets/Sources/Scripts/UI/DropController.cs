using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class DropController : MonoBehaviour
    {
        [SerializeField] private DropItemView _dropItemViewPrefab;

        private const float MaxOffset = 60f;

        public void Drop(ItemCount itemCount, RectTransform rectFrom)
        {
            CreateDropView(itemCount, rectFrom);
        }

        private void CreateDropView(ItemCount itemCount, RectTransform rectFrom)
        {
            DropItemView view = null;
            if(rectFrom == null)
                view = Instantiate(_dropItemViewPrefab, Game.Windows.TopLayer);
            else
                view = Instantiate(_dropItemViewPrefab, rectFrom.position, Quaternion.identity, Game.Windows.TopLayer);

            view.Init(itemCount);

            float xOffset = Random.Range(-MaxOffset, MaxOffset);
            float yOffset = Random.Range(-MaxOffset, MaxOffset);

            RectTransform rect = view.transform as RectTransform;
            Vector2 position = rect.anchoredPosition + new Vector2(xOffset, yOffset);
            rect.anchoredPosition = position;
            view.Show();
        }
    }
}
