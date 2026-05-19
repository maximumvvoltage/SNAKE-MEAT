using UnityEngine;

public class PlayerV4 : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform pauseMenuTransform; // Assign in Inspector

    [Header("Movement")]
    public float walkSpeed = 3.5f;
    public float jogSpeed = 8f;
    public float runSpeed = 7f;
    public float turnSmoothTime = 0.12f; 

    [Header("Jump")]
    public float jumpForce = 5f;
    public float groundCheckRadius = 0.22f;

    [Header("Camera – Lazy Follow")]
    public float cameraDistance = 5f;
    public float cameraHeight = 2.2f;
    public float cameraFollowSpeed = 3f;
    public float cameraRotateSpeed = 2f;
    public float autoCenterDelay = 2f;
    public float autoCenterSpeed = 1.8f;

    [Header("Pause")]
    public float pauseCameraMoveSpeed = 3f;

    public GameObject pauseMenuObjects;

    private Rigidbody rb;
    private float turnSmoothVelocity;
    private bool isGrounded;
    public bool isTeleporting;
    
    private float camYaw;
    private float camYawTarget;
    private float idleTimer;
    private bool autoReturning;
    private Animator shumAnim;

    private bool isWalkToggled = false;
    private bool wasRunning = false;

    private bool isPaused = false;
    private Vector3 prePausePosition;
    private Quaternion prePauseRotation;

    void Start()
    {
        shumAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        camYaw = transform.eulerAngles.y + 180f;
        camYawTarget = camYaw;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        HandlePause();

        if (!isPaused)
        {
            CheckGrounded();
            HandleJump();
            HandleCamera();
            HandleWalkToggle();
            HandleMovement();
        }
        else
        {
            SlideCameraTo(pauseMenuTransform.position, pauseMenuTransform.rotation);
        }
    }

    // ─── PAUSE ───────────────────────────────────────────────────────────────────────

    void HandlePause()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        pauseMenuObjects.SetActive(true);

        isPaused = !isPaused;

        if (isPaused)
        {
            prePausePosition = cameraTransform.position; //prepausee saves the cam's current transform position so that it
            prePauseRotation = cameraTransform.rotation; // can move back into place upon unpausing
            pauseMenuObjects.SetActive(true);
        }
        if (!isPaused)
            pauseMenuObjects.SetActive(false);
    }

    void SlideCameraTo(Vector3 targetPos, Quaternion targetRot)
    {
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, pauseCameraMoveSpeed * Time.deltaTime);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRot, pauseCameraMoveSpeed * Time.deltaTime);
    }

    // ───── GROUND CHECK ────────────────────────────────────────────────────────────────

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // ─── WALK TOGGLE ─────────────────────────────────────────────────────────────────

    void HandleWalkToggle()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isWalkToggled = !isWalkToggled;
    }

    // ─────── BASIC MOVEMENT ──────────────────────────────────────────────────────────────

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0f, v);
        bool isMoving = input.magnitude > 0.1f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        if (wasRunning && !isRunning)
            isWalkToggled = false;
        wasRunning = isRunning;

        bool isWalking = isMoving && isWalkToggled && !isRunning;

        shumAnim.SetBool("isWalking", isWalking);
        shumAnim.SetBool("isJogging", isMoving && !isWalking && !isRunning);
        shumAnim.SetBool("isRunning", isRunning);

        if (isMoving)
        {
            float cameraForwardYaw = camYaw - 180f;
            Quaternion camRotation = Quaternion.Euler(0f, cameraForwardYaw, 0f);
            Vector3 moveDir = camRotation * input.normalized;

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            float speed = isRunning ? runSpeed : isWalking ? walkSpeed : jogSpeed;
            Vector3 vel = moveDir * speed;
            vel.y = rb.linearVelocity.y;
            rb.linearVelocity = vel;

        }
        else
        {
            rb.linearVelocity =
                new Vector3(rb.linearVelocity.x * 0.8f, rb.linearVelocity.y, rb.linearVelocity.z * 0.8f);
        }
    }

    // ───── JUMPING ────────────────────────

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // ─── LAZY CAM ──────────────────────────────────────

    // creating a lazy camera was very complicated and used advanced Quarternions so I used Claude code.
    void HandleCamera()
    {
        if (cameraTransform == null) return;
        if (isTeleporting) return;
        

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool isMoving = new Vector3(h, 0f, v).magnitude > 0.1f;

        if (isMoving)
        {
            // Reset idle timer and stop auto-return while moving
            idleTimer = 0f;
            autoReturning = false;
        }
        else
        {
            // Count up idle time, then snap back behind player
            idleTimer += Time.deltaTime;
            if (idleTimer >= autoCenterDelay)
                autoReturning = true;
        }

        if (autoReturning)
        {
            camYawTarget = transform.eulerAngles.y + 180f;
            camYaw = Mathf.LerpAngle(camYaw, camYawTarget, autoCenterSpeed * Time.deltaTime);
        }

        // Position camera behind player using current camYaw
        Quaternion yawRot = Quaternion.Euler(0f, camYaw, 0f);
        Vector3 orbitOffset = yawRot * new Vector3(0f, cameraHeight, cameraDistance);
        Vector3 desiredPos = transform.position + orbitOffset;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPos, cameraFollowSpeed * Time.deltaTime);

        // Always look at the player
        Vector3 lookTarget = transform.position + Vector3.up * 1.2f;
        Quaternion desiredRot = Quaternion.LookRotation(lookTarget - cameraTransform.position);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, desiredRot, cameraRotateSpeed * Time.deltaTime);
    }

    // ─── GIZMO ───────────────────────────────────────────────────────────────────────

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}