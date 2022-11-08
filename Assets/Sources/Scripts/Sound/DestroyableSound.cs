using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class DestroyableSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _dyingAudio;
        [SerializeField] private float _dyingAudioVolume;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayDyingSound()
        {
            Game.Sound.PlaySound(_dyingAudio, _dyingAudioVolume, _audioSource);
        }
    }
}
