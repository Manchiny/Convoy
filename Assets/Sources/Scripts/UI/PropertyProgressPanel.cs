using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PropertyProgressPanel : MonoBehaviour
    {
        [SerializeField] private Color _baseIndicatorColor;
        [SerializeField] private Color _onUpgradedIndicatorColor;
        [SerializeField] private List<PropertyPointIndicator> _indicators;

        private void Awake()
        {
            foreach (var item in _indicators)
            {
                item.Init(_baseIndicatorColor, _onUpgradedIndicatorColor);
            }
        }

        public void SetProgress(int currentPropertyPoints)
        {
            int counter = 0;

            for (int i = 0; i < currentPropertyPoints; i++)
            {
                _indicators[i].SetUpgraded();
                counter++;
            }

            if(counter < _indicators.Count)
            {
                for (int i = counter; i < _indicators.Count; i++)
                {
                    _indicators[i].Reset();
                }
            }
        }

        public void SetMax()
        {
            _indicators.ForEach(indicator => indicator.SetUpgraded());
        }
    }
}
