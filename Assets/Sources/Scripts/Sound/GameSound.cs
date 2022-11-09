using Agava.WebUtility;
using UnityEngine;
using UnityEngine.Audio;

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
        [SerializeField] private AudioClip _buySound;
        [SerializeField] private AudioClip _boostUse;
        [Space]
        [SerializeField] private AudioMixer _mixer;

        private const float NormalVolumeLevel = 0f;
        private const float MinVolumeLevel = -100f;

        private const string MasterVolumeKey = "MasterVolume";
        private const string GameSoundsVolumeKey = "GameSoundsVolume";
        private const string MusicVolumeKey = "MusicVolume";

        private bool _backgroundPlaying;

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
            SetGameSoundsEnabled(Game.User.NeedSound);
            SetMusicEnabled(Game.User.NeedMusic);

            Game.Loosed += OnGameLoosed;
            Game.LevelCompleted += PlayCongratsSound;
            Game.LevelStarted += PlayBackgroundSound;
            Game.LevelCompleted += OnLevelComplete;

            PlayBackgroundSound();
        }

        private void OnDestroy()
        {
            Game.Loosed -= OnGameLoosed;
            Game.LevelCompleted -= PlayCongratsSound;
            Game.LevelStarted -= PlayBackgroundSound;
            Game.LevelCompleted -= OnLevelComplete;
        }

        public void SetAllSoundEnabled(bool enabled)
        {
            _mixer.SetFloat(MasterVolumeKey, enabled ? NormalVolumeLevel : MinVolumeLevel);

            AudioListener.pause = !enabled;
            AudioListener.volume = enabled ? 1 : 0;
        }

        public void SetGameSoundsEnabled(bool enabled)
        {
            _mixer.SetFloat(GameSoundsVolumeKey, enabled ? NormalVolumeLevel : MinVolumeLevel);
        }

        public void SetMusicEnabled(bool enabled)
        {
            _mixer.SetFloat(MusicVolumeKey, enabled ? NormalVolumeLevel : MinVolumeLevel);
        }

        public void PlayBasicButtonClick()
        {
            PlaySound(_buttonClick, 0.75f);
        }

        public void PlayPurchaseSound()
        {
            PlaySound(_buySound, 1);
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

        private void OnGameLoosed()
        {
            _backgrounsAudioSource.Stop();
            PlayLooseSound();
        }

        private void OnLevelComplete()
        {
            _backgrounsAudioSource.Stop();
            PlayCongratsSound();
        }

        private void PlayCongratsSound()
        {
            PlaySound(_levelCompleteSound, 0.8f);
        }

        private void PlayLooseSound()
        {
            PlaySound(_failSound, 0.8f);
        }

        public void PlayBoostUseSound()
        {
            PlaySound(_boostUse, 1);
        }

        private void PlayBackgroundSound()
        {
            if (_backgroundPlaying)
            {
                _backgrounsAudioSource.Play();
                return;
            }

            _backgroundPlaying = true;
            PlaySound(_backgroundSound, 0.4f, _backgrounsAudioSource);
        }

        private void OnInBackgroundChange(bool inBackground)
        {
            if (inBackground == false)
                SetAllSoundEnabled(true);
            else
                SetAllSoundEnabled(false);
        }
    }
}
