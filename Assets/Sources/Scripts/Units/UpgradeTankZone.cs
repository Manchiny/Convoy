using Assets.Scripts.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class UpgradeTankZone : MonoBehaviour
    {
        private const float DelayTime = 2f;

        private WaitForSeconds WaitSeconds = new WaitForSeconds(DelayTime);
        private Coroutine _waitAndShow;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player) == true && _waitAndShow == null)
                _waitAndShow = StartCoroutine(WaiteAndShowWindow());
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player) == true && _waitAndShow != null)
                StopWaiting();
        }

        private IEnumerator WaiteAndShowWindow()
        {
            yield return WaitSeconds;
            UpgradeTankWindow.Show();
        }

        private void StopWaiting()
        {
            if (_waitAndShow != null)
            {
                StopCoroutine(_waitAndShow);
                _waitAndShow = null;
            }
        }
    }
}
