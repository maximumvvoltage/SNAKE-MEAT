using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerV4 : MonoBehaviour
{

    [Header("References")]
    public Transform cameraTransform;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float walkSpeed = 3.5f;
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
    public float autoCenterDelay   = 0.6f;
    public float autoCenterSpeed   = 1.8f;

    private Rigidbody rb;
    private float turnSmoothVelocity;
    private bool  isGrounded;
    
    private float camYaw;//horizontal angle of camera around player
    private float camYawTarget;//where we want cammy to point
    private float movingTimer;//how long its been since the player started moving
    private bool autoCentering; // self explanatory </3

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        camYaw = transform.eulerAngles.y + 180f; // start yaw facing the same direction as shum
        camYawTarget = camYaw;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        CheckGrounded();
        HandleJump();
        HandleCamera();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void LateUpdate()
    {
        // camera runs last so it reads the final player position this frame
        
    }

    // ─── GROUND CHECK  ───────────────────────────────────────────────────────────────

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // ─── BASIC MOVEMENT ─────────────────────────────────────────────────────────────

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0f, v);

        if (input.magnitude > 0.1f)
        {
            float cameraForwardYaw = camYaw - 180f; //no matter where the camera is pointed, its rotation will always be flipped, making it always behind uou 
            Quaternion camRotation = Quaternion.Euler(0f, cameraForwardYaw, 0f);
            Vector3 moveDir = (camRotation * input.normalized);

            // smoothly rotates shum;s body to face the cam direction
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            
            bool running = Input.GetKey(KeyCode.LeftShift);
            float speed = running ? runSpeed : walkSpeed;
            Vector3 vel = moveDir * speed; 
            vel.y = rb.linearVelocity.y;
            rb.linearVelocity = vel;
            
            // accumulates time depending on how long you've been walking for. then, once the timer spills over the delay time, it begins to center itself
            movingTimer += Time.fixedDeltaTime;
            if (movingTimer >= autoCenterDelay)
            {
                autoCentering = true;
                // with this, camera yaw target now becomes the new camera target, making it shift behind the playe
                camYawTarget = transform.eulerAngles.y + 180f;
            }
        }
        else
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x * 0.8f, rb.linearVelocity.y, rb.linearVelocity.z * 0.8f);
            movingTimer = 0f;
            autoCentering = false;
        }
    }

    // ─── JUMPING ─────────────────────────────────────────────────────────────────

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // ─── LAZY CAM ──────────────────────────────────────────────────────────

    void HandleCamera()
    {
        if (cameraTransform == null) return;
        
        if (autoCentering)
        { // this turns the cam yaw to face its direction at the centering speed that we like
            camYaw = Mathf.LerpAngle(camYaw, camYawTarget, autoCenterSpeed * Time.deltaTime);
        }


        Quaternion yawRot = Quaternion.Euler(0f, camYaw, 0f);
        Vector3 orbitOffset = yawRot * new Vector3(0f, cameraHeight, cameraDistance);
        Vector3 desiredPos = transform.position + orbitOffset;
        
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPos, cameraFollowSpeed * Time.deltaTime);
        
        Vector3 lookTarget = transform.position + Vector3.up * 1.2f;
        Quaternion desiredRot = Quaternion.LookRotation(lookTarget - cameraTransform.position);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, desiredRot, cameraRotateSpeed * Time.deltaTime);
    }

    // ─── gizmo ball at shum's feet ────────────────────────────────────────────────────────────────

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}