using UnityEngine;

namespace Assets.Scripts.Levels
{
    [CreateAssetMenu]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int _roadPartCount;


        public int RoadPartsCount => _roadPartCount;
    }
}
