using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class DropController : MonoBehaviour
    {
        [SerializeField] private DropItemView _dropItemViewPrefab;

        private const float MaxOffset = 60f;

        public void Drop(ItemCount itemCount)
        {
            CreateDropView(itemCount);
        }

        private void CreateDropView(ItemCount itemCount)
        {
            var view = Instantiate(_dropItemViewPrefab, Game.Windows.TopLayer);
            view.Init(itemCount);

            float xPosition = Random.Range(-MaxOffset, MaxOffset);
            float yPosition = Random.Range(-MaxOffset, MaxOffset);

            RectTransform rect = view.transform as RectTransform;
            rect.anchoredPosition = new Vector2(xPosition, yPosition);
            view.Show();
        }
    }
}
