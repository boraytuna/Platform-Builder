using Managers;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        private Transform _playerTransform;

        [SerializeField] private float verticalOffset;
        [SerializeField] private float initialCameraHeight; // Set this in the Inspector for how high the camera starts
        private bool _followPlayer;

        private void OnEnable()
        {
            if (GamePlayEvents.instance != null)
            {
                GamePlayEvents.instance.OnPlayerSpawned += SetPlayerTransform;
            }
            else
            {
                Debug.LogError("GamePlayEvents instance is not available in CameraController.");
            }
        }

        private void OnDisable()
        {
            if (GamePlayEvents.instance != null)
            {
                GamePlayEvents.instance.OnPlayerSpawned -= SetPlayerTransform;
            }
        }

        // Method to set the player transform when the event is invoked
        private void SetPlayerTransform(Transform player)
        {
            _playerTransform = player;

            // Start the camera higher to avoid showing the ground 
            transform.position = new Vector3(transform.position.x, initialCameraHeight, transform.position.z);
        }

        private void LateUpdate()
        {
            if (_playerTransform == null) return;

            // Only start following the player once they are above the camera's initial height
            if (_playerTransform.position.y > initialCameraHeight)
            {
                _followPlayer = true;
            }

            // Update the camera's position only if the player is above the initial height
            if (_followPlayer)
            {
                // Calculate the new Y position with the vertical offset
                float newY = _playerTransform.position.y + verticalOffset;

                // Make sure the camera never goes lower than the initial camera height
                if (newY < initialCameraHeight)
                {
                    newY = initialCameraHeight;
                }

                // Update the camera's position
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }
    }
}