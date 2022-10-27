using UnityEngine;

namespace Assets.Scripts.UI
{
    public class Billboard : MonoBehaviour
    {
        private Transform _camera;

        private void Awake()
        {
            _camera = Camera.main.gameObject.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.forward);
        }
    }
}
