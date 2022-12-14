using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Destroyable
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Restartable))]
    public class DestroyablePart : MonoBehaviour, IRestartable
    {
        private const float LiveTime = 5f;
        private const float ExplosionForce = 500f; 

        private Rigidbody _rigidbody;
        private Coroutine _disabling;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Crush(Vector3 explosionPosition)
        {
            _rigidbody.isKinematic = false;

            _rigidbody.AddExplosionForce(ExplosionForce, explosionPosition, 30f);
             _disabling =  StartCoroutine(WaitAndDestroy());
        }

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(LiveTime);
            gameObject.SetActive(false);
        }

        public void OnRestart()
        {
            if(_disabling !=null)
            {
                StopCoroutine(_disabling);
                _disabling = null;
            }

            _rigidbody.isKinematic = true;
        }
    }
}
