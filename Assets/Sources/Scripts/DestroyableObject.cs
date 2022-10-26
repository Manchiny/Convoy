using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Destroyable
{
    public class DestroyableObject : MonoBehaviour
    {
        [SerializeField] private Transform _explosionPoint;
        private List<DestroyablePart> _parts = new();

        private void Awake()
        {
            _parts = GetComponentsInChildren<DestroyablePart>().ToList();
        }

        public void DestroyObject()
        {
            _parts.ForEach(part =>
            {
                part.Crush(_explosionPoint.position);
            });
        }
    }
}
