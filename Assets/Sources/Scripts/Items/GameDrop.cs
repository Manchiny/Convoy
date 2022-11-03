using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class GameDrop : MonoBehaviour
    {
        [SerializeField] private GameDropChecker _playerChecker;

        private List<ItemCount> _items;
        private bool _needWathRewardedVideoToGet;

        private void Start()
        {
            _playerChecker.PlayerEntered += OnPlyareEnter;
        }

        private void OnDisable()
        {
            _playerChecker.PlayerEntered -= OnPlyareEnter;
        }

        public void Init(List<ItemCount> items, bool needWathRewardedVideoToGet)
        {
            _items = items;
            _needWathRewardedVideoToGet = needWathRewardedVideoToGet;
        }

        private void OnPlyareEnter()
        {
            if(_needWathRewardedVideoToGet)
            {

            }
            else
            {
                Debug.Log("Player enter in drop zone");
                Game.Instance.AddItems(_items);
            }
        }
    }
}
