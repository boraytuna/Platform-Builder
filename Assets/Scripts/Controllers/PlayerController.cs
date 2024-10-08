using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        // Movement variables
        [Header("Movement Variables")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float fallFastVelocity = 5f;
        private Rigidbody2D _rb2D;
        private Vector2 _inputDirection;

        // Ground detection variables
        [Header("Jump Variables")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        private bool _isGrounded;
        
        // Camera variables
        [FormerlySerializedAs("_camera")]
        [Header("Camera Variables")]
        [SerializeField] private new Camera camera;

        // Jumping variables
        private bool _canDoubleJump;

        // Screen wrapping variables
        private float _screenHalfWidthInWorldUnits;
        private float _playerHalfWidth;

        private void OnEnable()
        {
            InputController.OnMove += HandleMove;
            InputController.OnJump += HandleJump;
            InputController.OnFallFast += HandleFallFast;
        }

        private void OnDisable()
        {
            InputController.OnMove -= HandleMove;
            InputController.OnJump -= HandleJump;
            InputController.OnFallFast -= HandleFallFast;
        }

        private void Start()
        {
            // Get Rigidbody2D component
            _rb2D = GetComponent<Rigidbody2D>();
            if (_rb2D == null)
            {
                Debug.LogError("Rigidbody2D component is missing from the player GameObject.");
            }

            // Find camera at the start
            camera = Camera.main;
            if (camera == null)
            {
                Debug.LogError("Main Camera not found. Ensure the camera is tagged as 'MainCamera'.");
            }

            // Calculate the half-width of the screen in world units
            if (camera != null)
            {
                _screenHalfWidthInWorldUnits = camera.orthographicSize * camera.aspect;
            }

            // Calculate the half-width of the player sprite
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                _playerHalfWidth = spriteRenderer.bounds.extents.x;
            }
            else
            {
                Debug.LogError("SpriteRenderer component is missing from the player GameObject.");
            }
        }

        private void FixedUpdate()
        {
            // Check if the player is grounded
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // Apply horizontal movement
            float horizontalVelocity = _inputDirection.x * moveSpeed;
            _rb2D.velocity = new Vector2(horizontalVelocity, _rb2D.velocity.y);

            // Handle falling faster if velocity is negative (falling)
            if (_rb2D.velocity.y < 0)
            {
                _rb2D.velocity += Vector2.up * (Physics2D.gravity.y * (2.5f - 1) * Time.fixedDeltaTime);
            }

            // Wrap the player's horizontal position
            WrapAround();
        }

        // Handle movement input
        private void HandleMove(Vector2 direction)
        {
            _inputDirection = direction;

            // Flip player sprite depending on movement direction
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        // Handle jump input
        private void HandleJump()
        {
            if (_isGrounded)
            {
                Jump();
            }
            else if (_canDoubleJump)
            {
                DoubleJump();
            }
        }

        // Handle fast fall input
        private void HandleFallFast()
        {
            if (!_isGrounded)
            {
                _rb2D.velocity += Vector2.down * fallFastVelocity;  // Increase downward velocity for fast fall
            }
        }

        // Modular jump method
        private void Jump()
        {
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, jumpForce);
            _canDoubleJump = true;  // Allow double jump after the initial jump
        }

        // Modular double jump method
        private void DoubleJump()
        {
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, jumpForce);
            _canDoubleJump = false;  // Consume the double jump
        }

        // Wrap the player's horizontal position
        private void WrapAround()
        {
            if (camera == null)
            {
                Debug.LogError("Camera reference is missing.");
                return;
            }

            float cameraX = camera.transform.position.x;

            // Calculate the left and right edges relative to the camera's position
            float leftEdge = cameraX - _screenHalfWidthInWorldUnits - _playerHalfWidth;
            float rightEdge = cameraX + _screenHalfWidthInWorldUnits + _playerHalfWidth;

            Vector3 newPosition = transform.position;

            if (transform.position.x < leftEdge)
            {
                newPosition.x = rightEdge;
            }
            else if (transform.position.x > rightEdge)
            {
                newPosition.x = leftEdge;
            }

            transform.position = newPosition;
        }

        // Draw ground check gizmo in the editor for debugging
        private void OnDrawGizmosSelected()
        {
            if (groundCheck == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}