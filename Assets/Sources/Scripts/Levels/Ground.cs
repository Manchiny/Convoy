using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Levels
{
    public class Ground : MonoBehaviour
    {    
        [SerializeField] private NavMeshSurface _navMeshSurface;

        private const float ExtraLenght = 5f;
        private const float ScaleDivider = 10f;

        public void Resize(float roadSize)
        {
            Vector3 scale = transform.localScale;
            scale.z = ExtraLenght + (roadSize / ScaleDivider);

            Vector3 position = Vector3.zero;
            position.z = roadSize / 2f - RoadPart.Lenght / 2f;

            transform.localScale = scale;
            transform.position = position;

            _navMeshSurface.BuildNavMesh();
        }
    }
}