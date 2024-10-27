using System.Collections;
using InventorySystem;
using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(Rigidbody2D))] // Ensures Rigidbody2D is present
    public class Jetpack : Tool
    {
        private Inventory _inventory;
        private Rigidbody2D _rb;

        [Header("Jetpack Settings")]
        [Tooltip("Force applied upwards when the jetpack is activated.")]
        public float jetpackForce = 25f;

        [Tooltip("Duration for which the jetpack provides continuous thrust.")]
        public float jetpackDuration = 2f;

        private bool _isJetpackActive;

        private void Awake()
        {
            toolName = "Jetpack";
            _rb = GetComponent<Rigidbody2D>();

            if (_rb == null)
            {
                Debug.LogError($"{toolName}: No Rigidbody2D found on the player. Please add a Rigidbody2D component.");
            }
        }

        private void Start()
        {
            _inventory = GetComponent<Inventory>();
        }

        /// <summary>
        /// Defines the action when the jetpack is used.
        /// </summary>
        public override void Use()
        {
            if (_inventory != null && _rb != null)
            {
                // Activate the jetpack
                ActivateJetpack();

                // Consume the jetpack after use
                _inventory.ConsumeTool<Jetpack>();
            }
            else
            {
                Debug.LogWarning($"{toolName}: Cannot activate jetpack. Inventory or Rigidbody2D is missing.");
            }
        }

        /// <summary>
        /// Activates the jetpack by applying an upward force.
        /// </summary>
        private void ActivateJetpack()
        {
            if (_isJetpackActive)
            {
                Debug.Log($"{toolName}: Jetpack is already active.");
                return;
            }

            Debug.Log($"{toolName}: Activated jetpack.");

            // Apply an instantaneous upward force
            _rb.AddForce(Vector2.up * jetpackForce, ForceMode2D.Impulse);

            _isJetpackActive = true;

            // Handle continuous thrust for a duration
            StartCoroutine(ContinuousThrust());
        }

        /// <summary>
        /// Applies continuous upward thrust for a set duration.
        /// </summary>
        private IEnumerator ContinuousThrust()
        {
            float elapsedTime = 0f;

            while (elapsedTime < jetpackDuration)
            {
                _rb.AddForce(Vector2.up * (jetpackForce * Time.deltaTime), ForceMode2D.Force);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _isJetpackActive = false;
            Debug.Log($"{toolName}: Jetpack thrust ended.");
        }
    }
}