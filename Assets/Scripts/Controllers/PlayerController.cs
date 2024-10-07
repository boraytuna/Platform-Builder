using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        // Movement variables
        public float moveSpeed = 5f;
        public float jumpForce = 5f;
        private Rigidbody2D _rb2D;
        private Vector2 _inputDirection;

        // Ground detection variables
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheck;  // Position at the player's feet for checking ground contact
        [SerializeField] private float groundCheckRadius = 0.2f;  // Radius of ground check
        private bool _isGrounded;

        // Jumping variables
        private bool _canDoubleJump;  // To track if double jump is allowed

        private void OnEnable()
        {
            // Subscribe to input events from InputController
            InputController.OnMove += HandleMove;
            InputController.OnJump += HandleJump;
        }

        private void OnDisable()
        {
            // Unsubscribe to avoid memory leaks
            InputController.OnMove -= HandleMove;
            InputController.OnJump -= HandleJump;
        }

        private void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate()
        {
            // Check if the player is grounded
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // Manually stop movement when no input is detected
            if (_inputDirection.x == 0)
            {
                _rb2D.velocity = new Vector2(0, _rb2D.velocity.y);
            }
            else
            {
                // Apply movement when input is detected
                _rb2D.velocity = new Vector2(_inputDirection.x * moveSpeed, _rb2D.velocity.y);
            }

            // Reset double jump if player is grounded
            // if (_isGrounded)
            // {
            //     _canDoubleJump = true;
            // }
        }

        // Handle movement input
        private void HandleMove(Vector2 direction)
        {
            _inputDirection = direction;

            // Flip player sprite depending on movement direction
            if (direction.x < 0)
            {
                // Moving left: flip the sprite to face left
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (direction.x > 0)
            {
                // Moving right: ensure the sprite is facing right
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        // Handle jump input
        private void HandleJump()
        {
            if (_isGrounded)
            {
                // Normal jump when grounded
                Jump();
            }
            else if (_canDoubleJump)
            {
                // Double jump if allowed
                DoubleJump();
            }
        }

        // Modular jump method
        private void Jump()
        {
            Debug.Log("Jump executed");
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, jumpForce);
        }

        // Modular double jump method
        private void DoubleJump()
        {
            Debug.Log("Double jump executed");
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, jumpForce);
            _canDoubleJump = false;  // Disable double jump until grounded again
        }

        // Draw ground check gizmo in the editor for debugging
        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
}
 