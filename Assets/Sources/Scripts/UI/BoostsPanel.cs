using UnityEngine;

namespace Assets.Scripts.UI
{

    public class BoostsPanel : MonoBehaviour
    {
        [SerializeField] private BasicButton _openBoostsButton;
        [SerializeField] private DropDawnPanel _boosts;

        public void Init(UserData userData)
        {
            _openBoostsButton.AddListener(ShowHideBoosts);

            var boostButtons = _boosts.GetComponentsInChildren<ItemUseButton>();

            foreach (var button in boostButtons)
                button.Init(userData);
        }

        private void ShowHideBoosts()
        {
            if (_boosts.IsActive)
                _boosts.Hide();
            else
                _boosts.Show();
        }
    }
}
