//----------------------------------------------------------
// This script is responsible for player movement.
// Use A or D to move horizontally,
// Use space to jump,
// Use S to fall faster while in the air,
// Use W or S to climb up or down on a wall,
// Double tap on A or D to dash on that direction,
//
// For prototyping use 1,2,3 to unlock the abilities.
//
// Author: Boray Tuna Goren
// Date: 10/10/2024
//---------------------------------------------------------
using UnityEngine;
using System.Collections;
using Abilities;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        // Movement variables
        [Header("Movement Variables")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float fallFastMultiplier = 4f; // Multiplier for fast falling
        private Rigidbody2D _rb2D;
        private Vector2 _inputDirection;

        // Double jumping variables
        private bool _canDoubleJump;

        // Dash variables
        [Header("Dash Variables")]
        [SerializeField] private float dashSpeed = 4f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 4f;
        private bool _isDashing;
        private bool _canDash = true;

        // Double-tap dash variables
        private float _lastTapTimeLeft;
        private float _lastTapTimeRight;
        private const float DoubleTapThreshold = 0.3f;

        // Wall interaction variables
        [Header("Wall Interaction Variables")]
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private float wallCheckDistance = 1f;
        private bool _isTouchingWall;
        private GameObject _currentWall;
        private float _climbInput;

        // Wall slide variables
        [Header("Wall Slide Variables")]
        [SerializeField] private float wallStickTime = 0.4f; // Time to stick to the wall
        [SerializeField] private float wallSlideSpeed = 10f; // Speed to slide down the wall
        [SerializeField] private float wallFastSlideSpeed = 16f; // Speed when sliding down faster
        [SerializeField] private float wallClimbSpeed = 14f; // Speed when climbing up
        private float _wallContactTime;
        private bool _isWallSliding;

        // Wall jump variables
        [Header("Wall Jump Variables")]
        [SerializeField] private float wallJumpHorizontalForce = 12f;
        [SerializeField] private float wallJumpVerticalForce = 16f;

        // Ground detection variables
        [Header("Ground Detection")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.4f;
        private bool _isGrounded;

        // Camera variables for wrapping around
        [Header("Camera Variables")]
        private Camera _camera;

        // Screen wrapping variables
        private float _screenHalfWidthInWorldUnits;
        private float _playerHalfWidth;

        // Ability system
        private readonly Abilities.Abilities _abilities = new Abilities.Abilities();

        private void OnEnable()
        {
            InputController.OnMove += HandleMove;
            InputController.OnJump += HandleJump;
            InputController.OnClimb += HandleClimb;
        }

        private void OnDisable()
        {
            InputController.OnMove -= HandleMove;
            InputController.OnJump -= HandleJump;
            InputController.OnClimb -= HandleClimb;
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
            _camera = Camera.main;
            if (_camera == null)
            {
                Debug.LogError("Main Camera not found. Ensure the camera is tagged as 'MainCamera'.");
            }

            // Calculate the half-width of the screen in world units
            if (_camera != null)
            {
                _screenHalfWidthInWorldUnits = _camera.orthographicSize * _camera.aspect;
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

            // Initialize abilities
            _abilities.AddAbility(new DoubleJumpAbility());
            _abilities.AddAbility(new DashAbility(dashSpeed, dashDuration));
            _abilities.AddAbility(new WallClimbAbility());
        }

        private void Update()
        {
            // Handle test unlocking of abilities (for prototype/testing)
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _abilities.UnlockAbility<DoubleJumpAbility>();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _abilities.UnlockAbility<DashAbility>();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _abilities.UnlockAbility<WallClimbAbility>();
            }
        }

        private void FixedUpdate()
        {
            // Check if the player is grounded
            bool wasGrounded = _isGrounded;
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // Reset double jump when grounded
            if (_isGrounded && !wasGrounded)
            {
                if (_abilities.IsAbilityUnlocked<DoubleJumpAbility>())
                {
                    _canDoubleJump = true;
                    Debug.Log("Player grounded. Double jump reset.");
                }
            }

            // Detect walls for sliding and climbing
            DetectWall();

            // Handle wall sliding
            if (_isTouchingWall && !_isGrounded)
            {
                HandleWallSlide();
            }
            else if (_isWallSliding)
            {
                // Stop wall sliding
                _isWallSliding = false;
                _rb2D.gravityScale = 1; // Reset gravity
                Debug.Log("Stopped wall sliding.");
            }

            // Apply horizontal movement if not dashing
            if (!_isDashing)
            {
                float horizontalVelocity = _inputDirection.x * moveSpeed;
                _rb2D.velocity = new Vector2(horizontalVelocity, _rb2D.velocity.y);
            }

            // Ensure gravity is reset when not sliding
            if (!_isWallSliding && _rb2D.gravityScale == 0)
            {
                _rb2D.gravityScale = 1;
                Debug.Log("Gravity reset to normal.");
            }

            // Handle fast falling when not on wall
            if (!_isGrounded && !_isWallSliding && _climbInput < 0f)
            {
                HandleFallFast();
            }

            // Wrap the player's horizontal position
            WrapAround();
        }

        // Handle movement input
        private void HandleMove(Vector2 direction)
        {
            _inputDirection = direction;

            DetectDoubleTap(direction.x);   // Check for double-tap input

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

        // Handle climb input
        private void HandleClimb(float value)
        {
            _climbInput = value;
            Debug.Log($"Climb Input: {_climbInput}");
        }

        // Detect double-tap for dash
        private void DetectDoubleTap(float horizontalInput)
        {
            if (horizontalInput > 0)
            {
                if (Time.time - _lastTapTimeRight < DoubleTapThreshold)
                {
                    // Double-tap detected to the right
                    AttemptDash(1);
                    _lastTapTimeRight = 0f; // Reset tap time
                }
                else
                {
                    _lastTapTimeRight = Time.time;
                }
                _lastTapTimeLeft = 0f; // Reset opposite direction
            }
            else if (horizontalInput < 0)
            {
                if (Time.time - _lastTapTimeLeft < DoubleTapThreshold)
                {
                    // Double-tap detected to the left
                    AttemptDash(-1);
                    _lastTapTimeLeft = 0f; // Reset tap time
                }
                else
                {
                    _lastTapTimeLeft = Time.time;
                }
                _lastTapTimeRight = 0f; // Reset opposite direction
            }
        }

        // Attempt to dash in a direction
        private void AttemptDash(int direction)
        {
            if (_abilities.IsAbilityUnlocked<DashAbility>() && !_isDashing && _canDash)
            {
                // Prevent dashing towards the wall when on the wall
                bool isDashingTowardsWall = _isWallSliding && ((direction > 0 && transform.localScale.x > 0) || (direction < 0 && transform.localScale.x < 0));

                if (!isDashingTowardsWall)
                {
                    StartCoroutine(DashCoroutine(direction));
                }
                else
                {
                    Debug.Log("Cannot dash towards the wall while sliding.");
                }
            }
            else if (!_canDash)
            {
                Debug.Log("Dash is on cooldown.");
            }
        }

        private IEnumerator DashCoroutine(int direction)
        {
            _isDashing = true;
            _canDash = false; // Start cooldown

            // Temporarily disable gravity
            float originalGravity = _rb2D.gravityScale;
            _rb2D.gravityScale = 0;

            // Apply dash velocity
            _rb2D.velocity = new Vector2(direction * dashSpeed * moveSpeed, 0);
            Debug.Log($"Dashing with velocity: {_rb2D.velocity}");

            yield return new WaitForSeconds(dashDuration);

            // Restore gravity
            _rb2D.gravityScale = originalGravity;

            _isDashing = false;

            // Start cooldown timer
            StartCoroutine(DashCooldownCoroutine());
        }

        private IEnumerator DashCooldownCoroutine()
        {
            yield return new WaitForSeconds(dashCooldown);
            _canDash = true; // Cooldown finished
            Debug.Log("Dash cooldown finished.");
        }

        // Handle jump input
        private void HandleJump()
        {
            if (_isGrounded)
            {
                Jump();
                Debug.Log("Jumped from ground.");
            }
            else if (_isWallSliding && _abilities.IsAbilityUnlocked<WallClimbAbility>())
            {
                WallJump();
                _canDoubleJump = true;
                Debug.Log("Performed wall jump.");
            }
            else if (_canDoubleJump)
            {
                DoubleJump();
                _canDoubleJump = false;
                Debug.Log("Performed double jump.");
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
        }

        // Wall jump method
        private void WallJump()
        {
            float jumpDirection = -transform.localScale.x;
            Vector2 wallJumpForce = new Vector2(jumpDirection * wallJumpHorizontalForce, wallJumpVerticalForce);
            _rb2D.velocity = Vector2.zero;
            _rb2D.AddForce(wallJumpForce, ForceMode2D.Impulse);

            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            _isWallSliding = false;
            _isTouchingWall = false;
            _rb2D.gravityScale = 1;
        }

        // Handle fast fall
        private void HandleFallFast()
        {
            _rb2D.velocity += Vector2.up * (Physics2D.gravity.y * (fallFastMultiplier - 1) * Time.fixedDeltaTime);
        }

        // Wrap the player's horizontal position
        private void WrapAround()
        {
            if (_camera == null)
            {
                Debug.LogError("Camera reference is missing.");
                return;
            }

            float cameraX = _camera.transform.position.x;

            // Calculate the left and right edges relative to the camera's position
            float leftEdge = cameraX - _screenHalfWidthInWorldUnits - _playerHalfWidth;
            float rightEdge = cameraX + _screenHalfWidthInWorldUnits + _playerHalfWidth;

            Vector3 newPosition = transform.position;

            if (transform.position.x < leftEdge)
            {
                newPosition.x = rightEdge;
                Debug.Log("Player wrapped to right edge.");
            }
            else if (transform.position.x > rightEdge)
            {
                newPosition.x = leftEdge;
                Debug.Log("Player wrapped to left edge.");
            }

            transform.position = newPosition;
        }

        // Detect walls for sliding and climbing
        private void DetectWall()
        {
            float direction = transform.localScale.x;
            Vector2 rayOrigin = wallCheck.position;
            Vector2 rayDirection = Vector2.right * direction;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, wallCheckDistance, wallLayer);

            Debug.DrawRay(rayOrigin, rayDirection * wallCheckDistance, Color.red);
            
            if (hit.collider != null)
            {
                if (!_isTouchingWall)
                {
                    _wallContactTime = Time.time;
                    _isWallSliding = true;
                    Debug.Log("Just touched wall. Start wall sliding.");
                }
                _isTouchingWall = true;
                _currentWall = hit.collider.gameObject;

                // Reset double jump when touching the wall, only if wall climbing ability is unlocked
                if (_abilities.IsAbilityUnlocked<WallClimbAbility>() && _abilities.IsAbilityUnlocked<DoubleJumpAbility>())
                {
                    _canDoubleJump = true;
                    Debug.Log("Double jump reset on wall touch (wall climbing ability unlocked).");
                }
            }
            else
            {
                if (_isTouchingWall)
                {
                    Debug.Log("No longer touching wall.");
                }
                _isTouchingWall = false;
                _currentWall = null;
                _isWallSliding = false;
            }
        }

        // Handle wall sliding
        private void HandleWallSlide()
        {
            if (!_isWallSliding)
            {
                _isWallSliding = true;
                _wallContactTime = Time.time;
                Debug.Log("Started wall sliding.");
            }

            _rb2D.gravityScale = 0; // Disable gravity during wall sliding

            if (!_abilities.IsAbilityUnlocked<WallClimbAbility>())
            {
                // Without wall climbing ability, player slides down at a fixed speed
                _rb2D.velocity = new Vector2(_rb2D.velocity.x, -wallSlideSpeed);
                Debug.Log("Sliding down wall (no wall climbing ability).");
            }
            else
            {
                float timeSinceWallContact = Time.time - _wallContactTime;

                if (timeSinceWallContact < wallStickTime)
                {
                    // Stick to the wall for wallStickTime seconds
                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, 0);
                    Debug.Log("Sticking to wall.");
                }
                else
                {
                    Debug.Log($"Wall Slide - Climb Input: {_climbInput}");

                    float verticalVelocity;

                    if (_climbInput > 0f)
                    {
                        // Climb up
                        verticalVelocity = wallClimbSpeed;
                        Debug.Log("Climbing up the wall.");
                    }
                    else if (_climbInput < 0f)
                    {
                        // Slide down faster
                        verticalVelocity = -wallFastSlideSpeed;
                        Debug.Log("Sliding down faster.");
                    }
                    else
                    {
                        // Regular wall sliding down
                        verticalVelocity = -wallSlideSpeed;
                        Debug.Log("Sliding down.");
                    }

                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, verticalVelocity);
                }
            }
        }
    }
}