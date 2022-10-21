using UnityEngine;

namespace Assets.Scripts
{
	[RequireComponent(typeof(LineRenderer))]
    public class EnemyCheckerRenderer : MonoBehaviour
    {
		private const int LinesCount = 64;
		private float _radius = 10f;

		private LineRenderer _circleRenderer;

		private void Awake() { _circleRenderer = GetComponent<LineRenderer>(); }


        private void Update() { DrawCircle(LinesCount, _radius); }

		public void SetRadius(float radius)
        {
			_radius = radius;
        }

		private void DrawCircle(int steps, float radius)
		{
			_circleRenderer.positionCount = steps;

			for (int i = 0; i < steps; i++)
			{
				float circumferenceProgress = (float)i / steps;
				float currentRadian = circumferenceProgress * 2f * Mathf.PI;

				float xScaled = Mathf.Cos(currentRadian);
				float yScaled = Mathf.Sin(currentRadian);

				float x = xScaled * radius;
				float y = yScaled * radius;

				Vector3 currentPosition = new Vector3(x + transform.localPosition.x, 0.5f, y + transform.localPosition.z);
				_circleRenderer.SetPosition(i, currentPosition);
			}
		}
	}
}
