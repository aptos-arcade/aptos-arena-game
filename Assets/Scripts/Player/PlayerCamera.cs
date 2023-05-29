using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour {

        [SerializeField] private float speed = 25f;

        [SerializeField] private GameObject target;
        [SerializeField] private Vector3 offset;

        [SerializeField] private Vector2 minBoundary;
        [SerializeField] private Vector2 maxBoundary;

        private Vector3 targetPos;

        private void Start()
        {
            // start the camera at the players position
            targetPos = transform.position;
        }


        private void FixedUpdate()
        {
            if (!target) return;
            // remove the z component of transform
            var cameraPos = transform.position;
            var transformPosition = target.transform.position;

            // get difference between target's position and camera's current position
            var targetDirection = transformPosition - cameraPos;
            targetDirection.z = 0;
            
            // calculate target position as the camera's current position * velocity * time
            this.targetPos = cameraPos + (targetDirection * (speed * Time.deltaTime));

            transform.position = ClampCamera(Vector3.Lerp(cameraPos, this.targetPos + offset, 0.25f));
        }
    
        public IEnumerator Shake(float duration, float magnitude)
        {
            var elapsed = 0f;

            while (elapsed < duration)
            {
                var x = Random.Range(-1f, 1f) * magnitude;
                var y = Random.Range(-1f, 1f) * magnitude;

                transform.position += new Vector3(x, y, 0);
            
                elapsed += Time.deltaTime;
                yield return 0;
            }
            transform.position = ClampCamera(targetPos);
        }

        private Vector3 ClampCamera(Vector3 desiredPosition)
        {
            return new Vector3(
                Mathf.Clamp(desiredPosition.x, minBoundary.x, maxBoundary.x),
                Mathf.Clamp(desiredPosition.y, minBoundary.y, maxBoundary.y),
                desiredPosition.z
            );
        }
    }
}