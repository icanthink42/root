using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Input")]
    public PlayerActions playerActions;
    [SerializeField] private InputAction inputMove;
    [SerializeField] private InputAction inputJump;

    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private Vector2 vel;
    [SerializeField] private float accel;
    [SerializeField, Tooltip("This is the acceleration when an input is held.")] private float decel;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField, Tooltip("Upwards velocity before addition fall force is applied.")] private float jumpFalloff;
    [SerializeField] private float gravity;
    [SerializeField] private float fallModifier;
    [SerializeField] private float lastJumpTime;
    [SerializeField] private float jumpBuffer;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float timeLeftGround;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isDoubleJumping;
    private AnimationController _animationController;

    [Header("Ground Detection")]
    [SerializeField] private bool isColliding;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Vector2 groundColPos;
    [SerializeField] private float groundColRadius;
    [SerializeField] private float height;
    [SerializeField] private LayerMask groundMask;

    [Header("Wall Detection")]
    [SerializeField] private bool hitWall;
    [SerializeField] private Vector2 wallColPos;
    [SerializeField] private float wallColRadius;
    [SerializeField] private float width;
    [SerializeField] private LayerMask wallMask;
    public static bool frozen = false;
    private static bool spawned = false;

    bool timeLeftGroundChecked = false;

    // Handles new Input System
    private void Awake() {
        playerActions = new PlayerActions();
    }

    private void OnEnable() {
        inputMove = playerActions.PlayerControls.Movement;
        inputMove.Enable();
        inputJump = playerActions.PlayerControls.Jump;
        inputJump.Enable();
    }

    private void OnDisable() {
        inputMove.Disable();
        inputJump.Disable();
    }

    // Start is called before the first frame update
    void Start() {
        if (spawned)
        {
            Destroy(gameObject);
        }

        spawned = true;
        rb = GetComponent<Rigidbody2D>();
        isJumping = false;
        lastJumpTime = -jumpBuffer;
        isColliding = false;
        _animationController = GetComponent<AnimationController>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (frozen)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        // Get input axes
        float horizontalInput = Input.GetAxis("Horizontal");

        // Reset the horizontal speed if the player has stopped moving horizontally.
        horizontalSpeed = rb.velocity.x == 0 ? 0 : horizontalSpeed;

        // Get the position as a Vector2
        Vector2 position = transform.position;

        // Handle the horizontal input
        if (horizontalInput != 0) {
            horizontalSpeed += horizontalInput * accel * Time.deltaTime;
            horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);
            /*
            if (Physics2D.OverlapCircle(position + wallColPos, wallColRadius, wallMask) || Physics2D.OverlapCircle(position + new Vector2(-wallColPos.x, wallColPos.y), wallColRadius, wallMask)) {
                horizontalSpeed = 0;
            }
            */
            /*
            if (Physics2D.OverlapCircle(position + wallColPos, wallColRadius, wallMask) && isColliding && horizontalInput > 0) {
                horizontalSpeed = 0;
            }
            else if (Physics2D.OverlapCircle(position + new Vector2(-wallColPos.x, wallColPos.y), wallColRadius, wallMask) && isColliding && horizontalInput < 0) {
                horizontalSpeed = 0;
            }
            */
        }
        else {
            horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, 0, decel * Time.deltaTime);
        }

        // Check if wall is being collided with
        if (Physics2D.OverlapCircle(position + wallColPos, wallColRadius, wallMask) || Physics2D.OverlapCircle(position + new Vector2(-wallColPos.x, wallColPos.y), wallColRadius, wallMask)) {
            hitWall = true;
        }
        else {
            hitWall = false;
        }
        
        // Handle ground collisions
        isGrounded = Physics2D.OverlapCircle(position + groundColPos, groundColRadius, groundMask) && vel.y <= 0f && isColliding ? true : false;
        if (isGrounded) {
            isJumping = false;
            isDoubleJumping = false;
            timeLeftGroundChecked = false;
        }
        else {
            if (!timeLeftGroundChecked) {
                timeLeftGround = Time.time;
                timeLeftGroundChecked = true;
            }
        }

        // Handle jumping
        vel = rb.velocity;
        
        if (vel.x > 0 && _animationController.currentAnimation != "jump" && _animationController.currentAnimation != "rebound")
        {
            _animationController.PlayAnimation("left");
        }
        else if (vel.x < 0 && _animationController.currentAnimation != "jump" && _animationController.currentAnimation != "rebound")
        {
            _animationController.PlayAnimation("right");
        }
        vel.x = horizontalSpeed;
        if (Input.GetKeyDown(KeyCode.Space)) {
            lastJumpTime = Time.time;
        }

        if ((lastJumpTime + jumpBuffer > Time.time) && !isJumping && (isGrounded || timeLeftGround + coyoteTime > Time.time)) {
            Jump();
        }
        if ((lastJumpTime + jumpBuffer > Time.time) && !isDoubleJumping) {
            Jump();
            isDoubleJumping = true;
        }

        // Handle additional fall speed
        if (!isGrounded) {
            if (rb.velocity.y < jumpFalloff || rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space)) {
                if (vel.y > 0f) {
                    //vel.y = 0f;
                }
                vel.y += gravity * fallModifier * Time.deltaTime;

            }
            else {
                vel.y += gravity * Time.deltaTime;
            }
        }
        else {
            vel.y = vel.y < 0 ? 0 : vel.y;
        }

        rb.velocity = vel;
    }

    /// <summary>
    /// Jump script, resets velocity and adds jump force.
    /// </summary>
    void Jump() {
        _animationController.PlayAnimation("jump");
        lastJumpTime = -jumpBuffer;
        vel.y = 0f;
        vel.y += jumpForce;
        isJumping = true;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo) {
        isColliding = true;
        _animationController.PlayAnimation("rebound");
    }

    void OnCollisionExit2D(Collision2D collisionInfo) {
        isColliding = false;
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position + new Vector3(groundColPos.x, groundColPos.y, 0f), groundColRadius);
        Gizmos.DrawWireSphere(transform.position + new Vector3(wallColPos.x, wallColPos.y, 0f), wallColRadius);
        Gizmos.DrawWireSphere(transform.position + new Vector3(-wallColPos.x, wallColPos.y, 0f), wallColRadius);
    }
}
