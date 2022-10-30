using UnityEngine;

namespace Assets.Scripts.UI
{

    public class BoostsPanel : MonoBehaviour
    {
        [SerializeField] private BasicButton _openBoostsButton;
        [SerializeField] private DropDawnPanel _boosts;

        private void Start()
        {
            _openBoostsButton.AddListener(ShowHideBoosts);
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
