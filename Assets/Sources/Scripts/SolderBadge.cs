using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class SolderBadge : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void AddDropForce()
        {
            _rigidbody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            _rigidbody.AddTorque(transform.up * 20f);
            _rigidbody.AddTorque(transform.forward * 50f);
        }
    }
}
