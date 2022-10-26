using UnityEngine;

namespace Assets.Scripts
{
    public class Restartable : MonoBehaviour
    {
        private IRestartable _restartable;

        private Transform _parent;

        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private Vector3 _startScale;

        private void Start()
        {
            _restartable = GetComponent<IRestartable>();
            Game.Restarted += Restart;

            WriteStartData();
        }

        private void OnDestroy()
        {
            Game.Restarted -= Restart;
        }

        public void Restart()
        {
            ResetTransforms();

            if(_restartable != null)
                _restartable.OnRestart();

            gameObject.SetActive(true);
        }

        public void WriteStartData()
        {
            _parent = transform.parent;

            _startPosition = transform.position;
            _startRotation = transform.rotation;
            _startScale = transform.localScale;
        }

        protected void ResetTransforms()
        {
            transform.position = _startPosition;
            transform.rotation = _startRotation;
            transform.localScale = _startScale;

            transform.parent = _parent;
        }
    }
}
