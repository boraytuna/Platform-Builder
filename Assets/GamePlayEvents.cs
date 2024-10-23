using System;
using UnityEngine;

public class GamePlayEvents : MonoBehaviour
{
    public static GamePlayEvents Instance { get; private set; }

    // Events for gameplay actions
    public event Action<Transform> OnPlayerSpawned;
    public event Action<Vector2> OnMove;
    public event Action OnJump;
    public event Action OnBuildPlatform;
    public event Action<float> OnClimb;
    public event Action OnUseTool;
    public event Action OnInteract;

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

    // Methods to trigger the events (can be invoked by other scripts)
    public void PlayerSpawned(Transform playerTransform)
    {
        OnPlayerSpawned?.Invoke(playerTransform);
    }

    public void Move(Vector2 direction)
    {
        OnMove?.Invoke(direction);
    }

    public void Jump()
    {
        OnJump?.Invoke();
    }

    public void BuildPlatform()
    {
        OnBuildPlatform?.Invoke();
    }

    public void Climb(float value)
    {
        OnClimb?.Invoke(value);
    }

    public void UseTool()
    {
        OnUseTool?.Invoke();
    }

    public void Interact()
    {
        OnInteract?.Invoke();
    }
}