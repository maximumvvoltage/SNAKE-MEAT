using UnityEngine;

public class WaterOrbVolume : MonoBehaviour
{
    [Header("References")]
    public Transform centerPoint;

    [Header("Entry Float")]
    public float floatToCenterForce = 5f;
    public float dragForce = 4f;

    [Header("Idle Pull")]
    public float idleTimeBeforePull = 1.5f;
    public float idleVelocityThreshold = 0.05f;
    public float idlePullForce = 6f;

    [Header("Escape")]
    public float escapeForceMagnitude = 20f;

    private Rigidbody playerRb;
    private bool playerInside = false;
    private float idleTimer = 0f;

    private float originalDrag;
    private bool originalUseGravity;

    private Vector3 previousPlayerPosition;
    private bool hasEscaped = false;

    private Vector3 previousOrbPosition;
    private Vector3 estimatedOrbVelocity;

    private SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        previousOrbPosition = transform.position;
    }

    private void Update()
    {
        estimatedOrbVelocity = (transform.position - previousOrbPosition) / Time.deltaTime;
        previousOrbPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerRb = other.GetComponent<Rigidbody>();
        if (playerRb == null) return;

        playerInside = true;
        hasEscaped = false;
        idleTimer = 0f;
        previousPlayerPosition = playerRb.position;

        originalDrag = playerRb.linearDamping;
        originalUseGravity = playerRb.useGravity;

        playerRb.linearDamping = dragForce;
        playerRb.useGravity = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        idleTimer = 0f;

        if (playerRb != null)
        {
            playerRb.linearDamping = originalDrag;
            playerRb.useGravity = originalUseGravity;
            playerRb = null;
        }
    }

    private void FixedUpdate()
    {
        if (!playerInside || playerRb == null) return;

        Vector3 toCenter = centerPoint.position - playerRb.position;
        float distanceFromCenter = toCenter.magnitude;

        bool hasMovementInput = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f ||
                                Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f;
        bool jumpPressed = Input.GetButton("Jump");

        // Escape condition — movement input and jump together
        if (hasMovementInput && jumpPressed && !hasEscaped)
        {
            hasEscaped = true;

            Vector3 escapeDirection = playerRb.linearVelocity.magnitude > 0.1f
                ? playerRb.linearVelocity.normalized
                : -toCenter.normalized;

            playerRb.linearVelocity = Vector3.zero;
            playerRb.AddForce(escapeDirection * escapeForceMagnitude, ForceMode.Impulse);
            return;
        }

        // Always float toward the center point
        playerRb.AddForce(toCenter.normalized * floatToCenterForce, ForceMode.Acceleration);

        // Track idle time based on player movement in world space
        float moved = Vector3.Distance(playerRb.position, previousPlayerPosition);
        previousPlayerPosition = playerRb.position;

        if (moved < idleVelocityThreshold)
        {
            idleTimer += Time.fixedDeltaTime;
        }
        else
        {
            idleTimer = 0f;
        }

        // Idle pull toward center
        if (idleTimer >= idleTimeBeforePull)
        {
            float urgency = Mathf.Clamp01((idleTimer - idleTimeBeforePull) / 2f);
            playerRb.AddForce(toCenter.normalized * idlePullForce * (1f + urgency * 2f), ForceMode.Acceleration);
        }

        // Keep player moving with the orb when not escaping
        /*Vector3 playerLocalVelocity = playerRb.velocity - estimatedOrbVelocity;
        if (!hasMovementInput)
        {
            playerRb.velocity = estimatedOrbVelocity + playerLocalVelocity * 0.95f;
        }*/
    }
}