using Agava.WebUtility;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    public class GameSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _defaultAudioSource;
        [SerializeField] private AudioSource _backgrounsAudioSource;
        [Space]
        [SerializeField] private AudioClip _levelCompleteSound;
        [SerializeField] private AudioClip _failSound;
        [Space]
        [SerializeField] private AudioClip _buttonClick;
        [Space]
        [SerializeField] private AudioClip _backgroundSound;

        private bool _backgroundPlaying;

        public bool Enabled { get; private set; }

        private void OnEnable()
        {
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        }

        private void OnDisable()
        {
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
        }

        public void Init()
        {
            SetSoundEnebled(Game.User.NeedSound);

            Game.Loosed += PlayLooseSound;
            Game.LevelCompleted += PlayCongratsSound;

            PlayBackgroundSound();
        }

        public void SetSoundEnebled(bool enabled)
        {
            Enabled = enabled;

            AudioListener.pause = !enabled;
            AudioListener.volume = enabled ? 1 : 0;
        }

        public void PlayBasicButtonClick()
        {
            PlaySound(_buttonClick, 0.75f);
        }

        public void PlayShoottingSound(AudioClip audioClip, float volume, AudioSource source)
        {
            PlaySound(audioClip, volume, source);
        }

        public void PlaySound(AudioClip clip, float volume, AudioSource source = null)
        {
            if (clip != null)
            {
                if (source == null)
                    source = _defaultAudioSource;

                source.volume = volume;
                source.clip = clip;
                source.Play();
            }
        }

        private void PlayCongratsSound()
        {
            PlaySound(_levelCompleteSound, 1);
        }

        private void PlayLooseSound()
        {
            PlaySound(_failSound, 1f, _defaultAudioSource);
        }

        private void PlayBackgroundSound()
        {
            if (_backgroundPlaying)
                return;

            _backgroundPlaying = true;

            PlaySound(_backgroundSound, 0.5f, _backgrounsAudioSource);
        }

        private void OnInBackgroundChange(bool inBackground)
        {
            if (inBackground == false)
                SetSoundEnebled(Game.User.NeedSound);
            else
                SetSoundEnebled(false);
        }
    }
}
