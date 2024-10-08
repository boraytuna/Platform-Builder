using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton for easy access
    
    [Header("Player Spawn References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab != null && spawnPoint != null)
        {
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
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