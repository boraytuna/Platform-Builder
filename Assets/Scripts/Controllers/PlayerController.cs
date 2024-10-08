using UnityEngine;

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

        // Jumping variables
        private bool _canDoubleJump;

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
            _rb2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            // Check if the player is grounded
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // Apply horizontal movement if input is detected
            _rb2D.velocity = _inputDirection.x != 0 ? new Vector2(_inputDirection.x * moveSpeed, _rb2D.velocity.y) :
                // Stop horizontal movement when no input is detected
                new Vector2(0, _rb2D.velocity.y);
    
            // Handle falling faster if velocity is negative (falling)
            if (_rb2D.velocity.y < 0)
            {
                _rb2D.velocity += Vector2.up * (Physics2D.gravity.y * (2.5f - 1) * Time.fixedDeltaTime);
            }

            // Reset double jump if player is grounded
            // if (_isGrounded)
            // {
            //     canDoubleJump = true;
            // }
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
        }

        // Modular double jump method
        private void DoubleJump()
        {
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, jumpForce);
            _canDoubleJump = false;
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