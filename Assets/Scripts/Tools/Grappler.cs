using InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools
{
    public class Grappler : Tool
    {
        [Header("Grappler Settings")]
        public float stoppingDistance = 2f;     // Distance at which grappling stops
        public float grapplingSpeed = 20f;      // Speed of the player when grappling
        public LayerMask grappleLayerMask;      // Layers that can be grappled onto (e.g., ground and wall)

        private bool _isGrappling;
        private Vector2 _targetPoint;
        private LineRenderer _lineRenderer;
        private Rigidbody2D _playerRigidbody;
        private Camera _camera;
        private float _originalGravityScale;
        
        private Inventory _inventory;
        private bool _hasGrappled;

        void Start()
        {
            _inventory = GetComponent<Inventory>();
            
            // Get the Rigidbody2D component for physics calculations
            _playerRigidbody = GetComponent<Rigidbody2D>();
            if (_playerRigidbody == null)
            {
                Debug.LogError("Grappler requires a Rigidbody2D component on the player.");
            }
            else
            {
                _originalGravityScale = _playerRigidbody.gravityScale;
            }

            // Initialize the LineRenderer for the grappling line visualization
            _lineRenderer = gameObject.GetComponent<LineRenderer>();
            if (_lineRenderer == null)
            {
                Debug.LogError("Grappler requires a LineRenderer component on the player.");
            }
            else
            { 
                _lineRenderer.enabled = false;
            }

            _camera = Camera.main;
        }

        public override void Use()
        {
            if (_inventory != null && _playerRigidbody != null)
            {
                // Activate the grappler
                ActivateGrappler();
            }
            else
            {
                Debug.LogWarning($"{toolName}: Cannot activate Grappler. Inventory or Rigidbody2D is missing.");
            } 
        }

        private void ActivateGrappler()
        {
            Vector2 mouseWorldPos = GetMouseWorldPosition();
            Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, grappleLayerMask);

            if (hit.collider != null)
            {
                _targetPoint = hit.point;
                _isGrappling = true;

                // Disable gravity
                _playerRigidbody.gravityScale = 0;

                // Enable and set positions for the LineRenderer
                _lineRenderer.enabled = true;
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, _targetPoint);

                Debug.Log($"{toolName} used. Grappling to {hit.collider.name} at {_targetPoint}.");
            }
            else
            {
                Debug.Log($"{toolName} failed to find a valid grapple point.");
            } 
        }

        void FixedUpdate()
        {
            UpdateTool();
        }

        public override void UpdateTool()
        {
            if (_isGrappling)
            {
                // Calculate direction towards the target point
                Vector2 direction = (_targetPoint - (Vector2)transform.position).normalized;

                // Set the player's velocity towards the target point
                _playerRigidbody.velocity = direction * grapplingSpeed;

                // Update the LineRenderer positions
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, _targetPoint);

                // Check if the player is within the stopping distance
                if (Vector2.Distance(transform.position, _targetPoint) <= stoppingDistance)
                {
                    CancelGrapple();
                }
            }
        }

        private void CancelGrapple()
        {
            _isGrappling = false;
            _lineRenderer.enabled = false;

            // Reset gravityScale
            _playerRigidbody.gravityScale = _originalGravityScale;

            // Stop the player's movement
            _playerRigidbody.velocity = Vector2.zero;
            
            _hasGrappled = true;

            Debug.Log($"{toolName}  canceled.");
        }

        private Vector2 GetMouseWorldPosition()
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos3D = _camera.ScreenToWorldPoint(mouseScreenPos);
            return new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);
        }

        public override void OnDeselect()
        {
            base.OnDeselect();
            CancelGrapple();
        }
    }
}