using Assets.Scripts.Sound;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Destroyable
{
    [RequireComponent(typeof(DestroyableSound))]
    public class DestroyableObject : MonoBehaviour
    {
        [SerializeField] private Transform _explosionPoint;

        private List<DestroyablePart> _parts = new();
        private DestroyableSound _sound;

        private void Awake()
        {
            _parts = GetComponentsInChildren<DestroyablePart>().ToList();
            _sound = GetComponent<DestroyableSound>();
        }

        public void DestroyObject()
        {
            _sound.PlayDyingSound();

            _parts.ForEach(part =>
            {
                part.Crush(_explosionPoint.position);
            });
        }
    }
}
