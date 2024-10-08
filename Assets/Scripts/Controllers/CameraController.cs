using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        private Transform _playerTransform;

        [SerializeField] private float verticalOffset;

        private void OnEnable()
        {
            Managers.GameManager.OnPlayerSpawned += SetPlayerTransform;
        }

        private void OnDisable()
        {
            Managers.GameManager.OnPlayerSpawned -= SetPlayerTransform;
        }

        // Method to set the player transform when the event is invoked
        private void SetPlayerTransform(Transform player)
        {
            _playerTransform = player;
        }

        private void LateUpdate()
        {
            if (_playerTransform == null) return;

            // Set the camera's Y position to match the player's Y position plus offset
            float newY = _playerTransform.position.y + verticalOffset;

            // Update the camera's position, keeping X and Z the same
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
