using System.Collections;
using UnityEngine;
using InventorySystem;
using UnityEngine.InputSystem;

namespace Tools
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(LineRenderer))]
    public class Grappler : Tool
    {
        // References
        private Inventory _inventory;
        private Rigidbody2D _rb;
        private LineRenderer _lineRenderer;

        // Grappling settings
        [Header("Grappler Settings")]
        [Tooltip("Layers that can be grappled (e.g., Ground, Wall).")]
        public LayerMask grappleLayers;

        [Tooltip("Maximum distance for grappling.")]
        public float maxGrappleDistance = 15f;

        [Tooltip("Force applied towards the grapple point.")]
        public float grappleForce = 20f;

        [Tooltip("Distance threshold to consider the player has reached the grapple point.")]
        public float reachThreshold = 1f;

        // State flags
        private bool _isGrappling;
        private Vector2 _grapplePoint;

        // Cooldown to prevent rapid grappling
        [Header("Cooldown Settings")]
        [Tooltip("Cooldown duration in seconds between grapples.")]
        public float grappleCooldown = 1f;
        private bool _canGrapple = true;

        // Camera reference
        private Camera _mainCamera;

        // Coroutine reference
        private Coroutine _pullCoroutine;

        private void Awake()
        {
            toolName = "Grappler";

            // Get components
            _rb = GetComponent<Rigidbody2D>();
            _lineRenderer = GetComponent<LineRenderer>();

            if (_rb == null)
            {
                Debug.LogError($"{toolName}: Rigidbody2D component is missing.");
            }

            if (_lineRenderer == null)
            {
                Debug.LogError($"{toolName}: LineRenderer component is missing.");
            }
            else
            {
                // Configure LineRenderer
                _lineRenderer.positionCount = 2;
                _lineRenderer.enabled = false;
                _lineRenderer.startWidth = 0.05f;
                _lineRenderer.endWidth = 0.05f;
                _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                _lineRenderer.startColor = Color.white;
                _lineRenderer.endColor = Color.white;
            }

            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _inventory = GetComponent<Inventory>();

            if (_inventory == null)
            {
                Debug.LogError($"{toolName}: Inventory component is missing.");
            }
        }

        private void Update()
        {
            if (_isGrappling)
            {
                UpdateGrappleLine();
                // Distance check handled in coroutine
            }
        }

        /// <summary>
        /// Defines the action when the grappler is used.
        /// </summary>
        public override void Use()
        {
            if (!_canGrapple)
            {
                Debug.Log("Grappler is on cooldown.");
                return;
            }

            if (_isGrappling)
            {
                // Optionally, cancel grapple if already grappling
                CancelGrapple();
                return;
            }

            // Perform grappling action
            PerformGrapple();
        }

        /// <summary>
        /// Initiates the grappling process.
        /// </summary>
        private void PerformGrapple()
        {
            // Get mouse position in world space
            Vector2 mouseWorldPos = GetMouseWorldPosition();

            // Check if the mouse is over a valid grapple target
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, grappleLayers);

            if (hit.collider != null)
            {
                _grapplePoint = hit.point;

                float distance = Vector2.Distance(_rb.position, _grapplePoint);

                if (distance > maxGrappleDistance)
                {
                    Debug.Log("Grapple target is too far.");
                    return;
                }

                // Check if the player has grappling tool (if necessary)
                if (!_inventory.HasTool<Grappler>())
                {
                    Debug.LogWarning("No Grappler available in inventory.");
                    return;
                }

                // Consume one Grappler from inventory
                _inventory.ConsumeTool<Grappler>();

                _isGrappling = true;

                // Enable LineRenderer
                if (_lineRenderer != null)
                {
                    _lineRenderer.enabled = true;
                    UpdateGrappleLine();
                }

                Debug.Log($"Grapple initiated towards {_grapplePoint}.");

                // Start pulling coroutine
                _pullCoroutine = StartCoroutine(PullTowardsGrapplePoint());

                // Start cooldown
                StartCoroutine(GrappleCooldown());
            }
            else
            {
                Debug.Log("No valid grapple target found.");
            }
        }

        /// <summary>
        /// Updates the LineRenderer positions.
        /// </summary>
        private void UpdateGrappleLine()
        {
            if (_lineRenderer != null && _isGrappling)
            {
                _lineRenderer.SetPosition(0, _rb.position);
                _lineRenderer.SetPosition(1, _grapplePoint);
            }
        }

        /// <summary>
        /// Cancels the current grapple.
        /// </summary>
        private void CancelGrapple()
        {
            if (_pullCoroutine != null)
            {
                StopCoroutine(_pullCoroutine);
                _pullCoroutine = null;
            }

            _isGrappling = false;

            if (_lineRenderer != null)
            {
                _lineRenderer.enabled = false;
            }

            Debug.Log("Grapple canceled.");
        }

        /// <summary>
        /// Retrieves the mouse position in world coordinates.
        /// </summary>
        /// <returns>Mouse position in world space.</returns>
        private Vector2 GetMouseWorldPosition()
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos3D = _mainCamera.ScreenToWorldPoint(mouseScreenPos);
            return new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);
        }

        /// <summary>
        /// Manages the cooldown period between grapples.
        /// </summary>
        /// <returns>IEnumerator for coroutine.</returns>
        private IEnumerator GrappleCooldown()
        {
            _canGrapple = false;
            yield return new WaitForSeconds(grappleCooldown);
            _canGrapple = true;
        }

        /// <summary>
        /// Coroutine to pull the player towards the grapple point.
        /// </summary>
        /// <returns>IEnumerator for coroutine.</returns>
        private IEnumerator PullTowardsGrapplePoint()
        {
            while (_isGrappling)
            {
                Vector2 direction = (_grapplePoint - _rb.position).normalized;
                _rb.AddForce(direction * (grappleForce * Time.fixedDeltaTime), ForceMode2D.Force);

                float distance = Vector2.Distance(_rb.position, _grapplePoint);
                if (distance <= reachThreshold)
                {
                    Debug.Log("Reached grapple point.");
                    CancelGrapple();
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}