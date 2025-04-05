using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CollisionDetector2D))]
//[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Horizontal movement speed.")]
    [SerializeField]
    private float moveSpeed = 5f;

    [Tooltip("Jump force applied when jumping.")]
    [SerializeField]
    private float jumpForce = 10f;

    [Tooltip("Custom gravity applied when airborne.")]
    [SerializeField]
    private float gravity = -20f;

    [Tooltip("Maximum fall speed.")]
    [SerializeField]
    private float maxFallSpeed = -20f;

    [Tooltip("Maximum fall speed when hovering.")]
    [SerializeField]
    private float maxHoverFallSpeed = -10f;

    [Tooltip("A small distance to keep away from colliders to avoid penetration.")]
    [SerializeField]
    private float skinWidth = 0.01f;

    private bool isGrounded = false;
    private bool jumpConsumed = false;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private CollisionDetector2D collisionDetector;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction hoverAction;
    private InputAction dropAction;

    // Controlled velocity (manually simulated).
    private Vector2 velocity;
    private bool jumpPressed;
    private bool hoverPressed;
    private bool dropPressed;
    private bool leftPressed;
    private bool rightPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        collisionDetector = GetComponent<CollisionDetector2D>();

        // Set Rigidbody2D to kinematic for manual control.
        rb.bodyType = RigidbodyType2D.Kinematic;

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        hoverAction = playerInput.actions["Hover"];
        dropAction = playerInput.actions["Drop"];
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Update()
    {
        leftPressed = moveAction.ReadValue<Vector2>().x < 0;
        rightPressed = moveAction.ReadValue<Vector2>().x > 0;

        // Only allow jumpPressed to be set if the player is grounded
        if (isGrounded && !jumpConsumed && jumpAction.triggered)
        {
            jumpPressed = true;
        }

        // Handle hover and drop inputs
        hoverPressed = hoverAction.triggered;
        dropPressed = dropAction.triggered;
    }

    void FixedUpdate()
    {
        var wasGrounded = isGrounded;
        var isNowGrounded = collisionDetector.IsGrounded();

        if (!wasGrounded && isNowGrounded)
        {
            Debug.Log("Landed");
        }
        else if (wasGrounded && !isNowGrounded)
        {
            Debug.Log("Jumped");
        }

        velocity.x = leftPressed
            ? -moveSpeed
            : rightPressed
                ? moveSpeed
                : 0f;

        // Ground detection via our collision detector.
        if (isNowGrounded)
        {
            if (jumpPressed)
            {
                Debug.Log("Jump pressed");
                velocity.y = jumpForce;
                jumpPressed = false; // Reset jumpPressed after processing
                jumpConsumed = true; // Mark jump as consumed
            }
            else
            {
                // When grounded, reset vertical velocity.
                velocity.y = 0f;
            }
        }
        else
        {
            var maxSpeed = hoverPressed ? maxHoverFallSpeed : maxFallSpeed;
            // Apply custom gravity when airborne.
            velocity.y += gravity * Time.fixedDeltaTime;
            velocity.y = Mathf.Max(velocity.y, maxSpeed);
        }

        Vector2 deltaMovement = velocity * Time.fixedDeltaTime;

        if (deltaMovement != Vector2.zero)
        {
            //Debug.Log("Moving: " + deltaMovement);
        }

        // Move the player using the adjusted movement.
        transform.position = collisionDetector.GetNewPosition(deltaMovement);

        isGrounded = isNowGrounded;

        // Reset jumpConsumed when grounded
        if (isGrounded)
        {
            jumpConsumed = false;
        }
    }
}
