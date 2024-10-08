using System;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;  // Singleton for easy access

        [Header("Player Spawn References")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform spawnPoint;

        // Event that other scripts can subscribe to
        public static event Action<Transform> OnPlayerSpawned; // For camera controller script

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            if (playerPrefab != null && spawnPoint != null)
            {
                // Instantiate the player and store a reference to the instance
                GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

                // Invoke the event to notify subscribers that the player has been spawned
                OnPlayerSpawned?.Invoke(playerInstance.transform);
            }
            else
            {
                Debug.LogError("Player prefab or spawn point is not set.");
            }
        }

        public void EndGame()
        {
            Debug.Log("Game Over");
            // Logic to handle end game state, like returning to main menu
        }
    }
}
