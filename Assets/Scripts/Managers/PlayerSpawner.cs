    using UnityEngine;

namespace Managers
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform spawnPoint;

        private void Start()
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            if (playerPrefab != null && spawnPoint != null)
            {
                // Instantiate the player and store a reference to the instance
                GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

                // Trigger the event to notify that the player has been spawned
                GamePlayEvents.Instance.PlayerSpawned(playerInstance.transform);
            }
            else
            {
                Debug.LogError("Player prefab or spawn point is not set.");
            }
        }
    }
}