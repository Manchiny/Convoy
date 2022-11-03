using Assets.Scripts.Items;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class AirPlane : MonoBehaviour
    {
        [SerializeField] private AirDrop _airDropPrefab;

        private const float FlyDuration = 4f;
        private const float EndPositiontX = 50f;
        private const float DurationToEndPointFly = 8f;

        private Tween _moveAnimation;

        private Vector3 _dropCreatePoisition = new Vector3(0, 0, -50f);

        private List<ItemCount> _items;
        private Vector3 _dropPosition;
        private bool _needWaitRewardedVideoToGet;

        private AirDrop _drop;

        public AirDrop DeliveDrop(Vector3 dropPosition, List<ItemCount> items, bool needWaitRewardedVideoToGet)
        {
            if (_moveAnimation != null)
                _moveAnimation.Kill();

            if (_drop != null)
                Destroy(_drop.gameObject);

            _drop = Instantiate(_airDropPrefab, _dropCreatePoisition, Quaternion.identity);
            _drop.gameObject.SetActive(false);

            _items = items;
            _dropPosition = dropPosition;
            _needWaitRewardedVideoToGet = needWaitRewardedVideoToGet;

            MoveAtPoint(dropPosition, FlyDuration, OnDropAirPointReached);

            return _drop;
        }

        private void MoveAtPoint(Vector3 position, float duration, Action onComplete)
        {
            position.y = transform.position.y;
            _moveAnimation = transform.DOMove(position, duration).SetLink(gameObject).SetEase(Ease.Linear).OnComplete(() => onComplete?.Invoke());
            transform.DOLookAt(position, duration / 2f);
        }

        private void OnDropAirPointReached()
        {
            _drop.transform.position = transform.position;
            _drop.gameObject.SetActive(true);

            _drop.Init(_items, _needWaitRewardedVideoToGet);
            _drop.MoveToDropPoint(_dropPosition);

            Vector3 endPosition = transform.position;
            endPosition.x = EndPositiontX;

            MoveAtPoint(endPosition, DurationToEndPointFly, () => gameObject.SetActive(false));
        }
    }
}
