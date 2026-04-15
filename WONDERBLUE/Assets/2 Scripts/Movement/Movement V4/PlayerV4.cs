using UnityEngine;

/// <summary>
/// Player V4 — Third-person controller inspired by Messenger (abeto.to)
/// 
/// Camera behaviour:
///   • Floats behind the player with heavy smoothing (lazy / drifting feel)
///   • Auto-centers softly when the player starts moving
///   • Mouse stays FREE — no camera orbit on mouse move
///   • No cursor lock
/// 
/// Movement:
///   • WASD moves relative to the camera's forward
///   • Left Shift → run
///   • Space     → jump (while grounded)
///   • Player body rotates to face movement direction (smooth turn)
/// 
/// Setup:
///   1. Attach this script to your Player GameObject (with Rigidbody + Collider).
///   2. Assign 'cameraTransform' to your scene Camera.
///   3. Assign 'groundCheck' to an empty child at the player's feet.
///   4. Set 'groundLayer' to the Ground layer mask.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerV4 : MonoBehaviour
{
    // ─── Inspector ────────────────────────────────────────────────────────────

    [Header("References")]
    public Transform cameraTransform;       // The scene camera
    public Transform groundCheck;           // Empty child at player feet
    public LayerMask groundLayer;

    [Header("Movement")]
    public float walkSpeed       = 3.5f;
    public float runSpeed        = 7f;
    public float turnSmoothTime  = 0.12f;   // How fast the body rotates to face direction

    [Header("Jump")]
    public float jumpForce         = 5f;
    public float groundCheckRadius = 0.22f;

    [Header("Camera – Lazy Follow")]
    public float cameraDistance    = 5f;    // How far behind the player
    public float cameraHeight      = 2.2f;  // Height offset above player pivot
    public float cameraFollowSpeed = 3f;    // Position lerp speed (low = lazy)
    public float cameraRotateSpeed = 2f;    // Rotation lerp speed (low = dreamy)
    public float autoCenterDelay   = 0.6f;  // Seconds of movement before camera re-centers
    public float autoCenterSpeed   = 1.8f;  // How fast it re-centers once triggered

    // ─── Private state ────────────────────────────────────────────────────────

    private Rigidbody rb;
    private float turnSmoothVelocity;
    private bool  isGrounded;

    // Camera
    private float  camYaw;              // Current horizontal angle of camera around player
    private float  camYawTarget;        // Where we want the camera angle to drift toward
    private float  movingTimer;         // Accumulated time the player has been moving
    private bool   autoCentering;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Initialise camera yaw to face the same way as the player
        camYaw       = transform.eulerAngles.y + 180f;
        camYawTarget = camYaw;

        // Unlock cursor — mouse stays free for world interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }

    void Update()
    {
        CheckGrounded();
        HandleJump();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void LateUpdate()
    {
        // Camera runs last so it reads the final player position this frame
        HandleCamera();
    }

    // ─── Ground ───────────────────────────────────────────────────────────────

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    // ─── Movement ─────────────────────────────────────────────────────────────

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0f, v);

        if (input.magnitude > 0.1f)
        {
            // Compute desired world-space direction relative to camera
            float cameraForwardYaw = camYaw - 180f; // Camera looks at player from camYaw
            Quaternion camRotation = Quaternion.Euler(0f, cameraForwardYaw, 0f);
            Vector3 moveDir = (camRotation * input.normalized);

            // Smoothly rotate player body to face movement direction
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref turnSmoothVelocity,
                turnSmoothTime
            );
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            // Apply velocity
            bool running = Input.GetKey(KeyCode.LeftShift);
            float speed  = running ? runSpeed : walkSpeed;
            Vector3 vel  = moveDir * speed;
            vel.y        = rb.velocity.y;
            rb.velocity = vel;

            // Accumulate moving time for auto-center trigger
            movingTimer += Time.fixedDeltaTime;
            if (movingTimer >= autoCenterDelay)
            {
                autoCentering  = true;
                // Target: camera yaw sits directly behind the player's facing direction
                camYawTarget   = transform.eulerAngles.y + 180f;
            }
        }
        else
        {
            // Decelerate
            rb.velocity = new Vector3(
                rb.velocity.x * 0.8f,
                rb.velocity.y,
                rb.velocity.z * 0.8f
            );
            movingTimer   = 0f;
            autoCentering = false;
        }
    }

    // ─── Jump ─────────────────────────────────────────────────────────────────

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // ─── Lazy Camera ──────────────────────────────────────────────────────────

    void HandleCamera()
    {
        if (cameraTransform == null) return;

        // Softly drift camYaw toward the auto-center target when walking
        if (autoCentering)
        {
            camYaw = Mathf.LerpAngle(camYaw, camYawTarget, autoCenterSpeed * Time.deltaTime);
        }

        // Desired camera position: orbit around player at camYaw angle
        Quaternion yawRot      = Quaternion.Euler(0f, camYaw, 0f);
        Vector3    orbitOffset = yawRot * new Vector3(0f, cameraHeight, cameraDistance);
        Vector3    desiredPos  = transform.position + orbitOffset;

        // Lazy lerp — position trails behind with inertia
        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position,
            desiredPos,
            cameraFollowSpeed * Time.deltaTime
        );

        // Softly look at a point slightly above the player's feet
        Vector3 lookTarget = transform.position + Vector3.up * 1.2f;
        Quaternion desiredRot = Quaternion.LookRotation(
            lookTarget - cameraTransform.position
        );
        cameraTransform.rotation = Quaternion.Slerp(
            cameraTransform.rotation,
            desiredRot,
            cameraRotateSpeed * Time.deltaTime
        );
    }

    // ─── Gizmo ────────────────────────────────────────────────────────────────

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}