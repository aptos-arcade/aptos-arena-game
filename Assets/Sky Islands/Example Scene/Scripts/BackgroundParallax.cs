using Gameplay;
using UnityEngine;

namespace Sky_Islands.Example_Scene.Scripts
{
	public class BackgroundParallax : MonoBehaviour
	{
		public Transform[] backgrounds;				// Array of all the backgrounds to be parallaxed.
		public float parallaxScale;					// The proportion of the camera's movement to move the backgrounds by.
		public float parallaxReductionFactor;		// How much less each successive layer should parallax.
		public float smoothing;						// How smooth the parallax effect should be.
	
		private Vector3 previousCamPos;				// The postion of the camera in the previous frame.

		private void Start()
		{
			previousCamPos = MatchManager.Instance.SceneCamera.transform.position;
		}

		private void Update ()
		{
			// The parallax is the opposite of the camera movement since the previous frame multiplied by the scale.
			var parallax = (previousCamPos.x - MatchManager.Instance.SceneCamera.transform.position.x) * parallaxScale;

			// For each successive background...
			for(var i = 0; i < backgrounds.Length; i++)
			{
				// ... set a target x position which is their current position plus the parallax multiplied by the reduction.
				var backgroundTargetPosX = backgrounds[i].position.x + parallax * (i * parallaxReductionFactor + 1);

				// Create a target position which is the background's current position but with it's target x position.
				var backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

				// Lerp the background's position between itself and it's target position.
				backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
			}

			// Set the previousCamPos to the camera's position at the end of this frame.
			previousCamPos = MatchManager.Instance.SceneCamera.transform.position;
		}
	}
}
