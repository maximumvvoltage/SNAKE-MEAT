using UnityEngine;
using UnityEngine.UI;

public class WaterVolume : MonoBehaviour
{
    [Header("References")]
    public BoxCollider waterVolume;
    public GameObject waterScreen;

    [Header("Floating")]
    public float floatHeight = 0.5f;
    public float buoyancyForce = 8f;
    public float sinkDepth = 1.5f;
    public float sinkDuration = 0.6f;

    public float dragForce = 3f;
    public float boostForce = 8f;
    
    [Header("UI")]
    public Image waterScreenTimer;
    public Animator waterTimerAnim;

    [Header("Underwater Mode")]
    public float underwaterDepth = 2f;
    public float underwaterTransitionSpeed = 3f;
    public float holdThreshold = 1.5f;

    private Rigidbody playerRb;
    private bool playerInside = false;

    private float originalDrag;
    private bool originalUseGravity;

    private float sinkTimer = 0f;
    private bool isSinking = true;

    private float waterSurfaceY;
    private float targetY;
    private float surfaceTargetY;
    private float underwaterTargetY;

    private bool isUnderwater = false;
    private float spaceHoldTimer = 0f;
    private bool spacePreviouslyHeld = false;
    private bool holdRegistered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerRb = other.GetComponent<Rigidbody>();
        if (playerRb == null) return;

        playerInside = true;
        isSinking = true;
        isUnderwater = false;
        sinkTimer = 0f;
        spaceHoldTimer = 0f;
        holdRegistered = false;

        waterSurfaceY = transform.position.y + (waterVolume.size.y * transform.lossyScale.y * 0.5f);
        surfaceTargetY = waterSurfaceY - floatHeight;
        underwaterTargetY = waterSurfaceY - underwaterDepth;
        targetY = surfaceTargetY;

        originalDrag = playerRb.drag;
        originalUseGravity = playerRb.useGravity;

        playerRb.drag = dragForce;
        playerRb.useGravity = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        isSinking = false;
        isUnderwater = false;

        if (playerRb != null)
        {
            playerRb.drag = originalDrag;
            playerRb.useGravity = originalUseGravity;
            playerRb = null;
        }
    }

    private void Update()
    {
        if (!playerInside || playerRb == null || isSinking) return;

        bool spaceHeld = Input.GetKey(KeyCode.Space);
        
        if (spaceHeld)
        {
            if (!spacePreviouslyHeld)
            {
                waterScreenTimer.fillAmount = 0f;
                waterTimerAnim.SetTrigger("Appear");
            }

            spaceHoldTimer += Time.deltaTime;
            waterScreenTimer.fillAmount = spaceHoldTimer / holdThreshold;

            if (spaceHoldTimer >= holdThreshold && !holdRegistered)
            {
                holdRegistered = true;
                isUnderwater = !isUnderwater;
                targetY = isUnderwater ? underwaterTargetY : surfaceTargetY;
                waterTimerAnim.SetTrigger("Disappear");
            }
        }
        else
        {
            if (spacePreviouslyHeld && !holdRegistered)
            {
                Vector3 boostDirection = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z).normalized;

                if (boostDirection == Vector3.zero)
                    boostDirection = playerRb.transform.forward;

                playerRb.AddForce(boostDirection * boostForce, ForceMode.Impulse);
                waterTimerAnim.SetTrigger("Disappear");
            }

            spaceHoldTimer = 0f;
            holdRegistered = false;
        }

        spacePreviouslyHeld = spaceHeld;

        /*if (spaceHeld)
        {
            spaceHoldTimer += Time.deltaTime;

            if (spaceHoldTimer >= holdThreshold && !holdRegistered)
            {
                holdRegistered = true;
                isUnderwater = !isUnderwater;
                targetY = isUnderwater ? underwaterTargetY : surfaceTargetY;
                
                waterScreen.SetActive(isUnderwater ? true : false);
            }
        }
        else
        {
            // Space was released — if it was a tap, boost forward
            if (spacePreviouslyHeld && !holdRegistered)
            {
                Vector3 boostDirection = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z).normalized;

                if (boostDirection == Vector3.zero)
                    boostDirection = playerRb.transform.forward;

                playerRb.AddForce(boostDirection * boostForce, ForceMode.Impulse);
            }

            spaceHoldTimer = 0f;
            holdRegistered = false;
        }*/

        spacePreviouslyHeld = spaceHeld;
    }

    private void FixedUpdate()
    {
        if (!playerInside || playerRb == null) return;

        if (isSinking)
        {
            sinkTimer += Time.fixedDeltaTime;
            if (sinkTimer >= sinkDuration)
            {
                isSinking = false;
                playerRb.useGravity = false;
            }
            return;
        }

        float currentY = playerRb.position.y;
        float distanceFromTarget = targetY - currentY;

        if (currentY < targetY)
        {
            float upwardForce = buoyancyForce * Mathf.Clamp01(Mathf.Abs(distanceFromTarget));
            playerRb.AddForce(Vector3.up * upwardForce * (isUnderwater ? underwaterTransitionSpeed : 1f), ForceMode.Acceleration);
        }
        else
        {
            playerRb.AddForce(Vector3.down * buoyancyForce * 0.5f, ForceMode.Acceleration);
        }
    }
}