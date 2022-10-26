using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class EndLevelCheckpoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Tank tank))
                tank.OnComplete();
        }
    }
}
