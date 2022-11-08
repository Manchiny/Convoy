using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Tank))]
    public class TankSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _movementSound;
        [SerializeField] private AudioClip _tankDestruction;

        private const float MovementAudioVolume = 0.7f;
        private const float DestructionVolume = 0.5f;

        private const float MovementPtichOnStop = 1f;
        private const float MovementPithOnMove = 1.2f;

        private AudioSource _audioSource;
        private Tank _tank;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _tank = GetComponent<Tank>();

            Game.LevelStarted += OnLevelStarted;
            Game.Restarted += OnLevelStarted;

            _tank.Stopped += OnTankStopped;
            _tank.Died += OnTankDied;
        }

        private void OnDestroy()
        {
            Game.LevelStarted -= OnLevelStarted;
            Game.Restarted -= OnLevelStarted;

            _tank.Stopped -= OnTankStopped;
            _tank.Died -= OnTankDied;
        }

        private void OnLevelStarted()
        {
            _audioSource.loop = true;
            _audioSource.pitch = MovementPtichOnStop;

            Game.Sound.PlaySound(_movementSound, MovementAudioVolume, _audioSource);
        }

        private void OnTankStopped(bool isStopped)
        {
            if (_tank.IsAlive == false)
                return;

            if(isStopped)
                _audioSource.pitch = MovementPtichOnStop;
            else
                _audioSource.pitch = MovementPithOnMove;
        }

        private void OnTankDied(Damageable tank)
        {
            _audioSource.Stop();
            _audioSource.loop = false;
            _audioSource.pitch = 1.4f;

            Game.Sound.PlaySound(_tankDestruction, DestructionVolume, _audioSource);
        }
    }
}