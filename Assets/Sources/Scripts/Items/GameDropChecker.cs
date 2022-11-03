using Assets.Scripts.Units;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameDropChecker : MonoBehaviour
    {
        public event Action PlayerEntered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                PlayerEntered?.Invoke();
            }
        }
    }
}
