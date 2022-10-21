using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Destroyable
{
    [RequireComponent(typeof(Rigidbody))]
    public class DestroyablePart : MonoBehaviour
    {
        private const float LiveTime = 5f;
        private const float ExplosionForce = 500f; 

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Crush(Vector3 explosionPosition)
        {
            _rigidbody.isKinematic = false;

            _rigidbody.AddExplosionForce(ExplosionForce, explosionPosition, 30f);
            StartCoroutine(WaitAndDestroy());
        }

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(LiveTime);
            gameObject.SetActive(false);
        }
    }
}
