using UnityEngine;
using System.Collections;

public class TunnelEntrance : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public Transform cameraTransform;
    public PlayerV4 shum;

    [Header("Destinations")]
    public Transform[] previewPoints;
    public Transform[] teleportDestinations;

    public float cameraFlySpeed = 3f;
    public float teleportDelay = 1f;

    private bool playerInside = false;
    private bool isInTeleportMode = false;
    private int currentIndex = 0;
    private Vector3 originalCamPos;
    private Quaternion originalCamRot;

    public GameObject teleportUI;
    public GameObject bubble;
    public TeleportConfirmButton confirmButton;
    
    [Header("Transitions")]
    public AnimationClip transitionClip;
    public Animator transitionAnimator;
    public GameObject transition;
    void Update()
    {
        if (playerInside && !isInTeleportMode && Input.GetKeyDown(KeyCode.E))
            EnterTeleportMode();

        if (!isInTeleportMode) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            ShiftPreview(-1);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            ShiftPreview(1);

        // smoothly fly camera to current preview point
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, previewPoints[currentIndex].position, cameraFlySpeed * Time.deltaTime);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, previewPoints[currentIndex].rotation, cameraFlySpeed * Time.deltaTime);
    }

    void EnterTeleportMode()
    {
        isInTeleportMode = true;
        teleportUI.SetActive(true);
        
        currentIndex = 0;
        originalCamPos = cameraTransform.position;
        originalCamRot = cameraTransform.rotation;
        shum.isTeleporting = true;
        confirmButton.SetTunnel(this);
    }

    void ShiftPreview(int direction)
    {
        currentIndex = (currentIndex + direction + previewPoints.Length) % previewPoints.Length;
    }
    
    public void ConfirmTeleportButton()
    {
        StartCoroutine(ConfirmTeleport());
    }

    IEnumerator ConfirmTeleport()
    {
        isInTeleportMode = false;
        teleportUI.SetActive(false);
        
        StartCoroutine(TransitionSequence());

        float elapsed = 0f;
        while (elapsed < teleportDelay)
        {
            elapsed += Time.deltaTime;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, originalCamPos, elapsed / teleportDelay);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, originalCamRot, elapsed / teleportDelay);
            yield return null;
        }

        playerTransform.position = teleportDestinations[currentIndex].position;
        playerTransform.rotation = teleportDestinations[currentIndex].rotation;
        shum.isTeleporting = false;
    }

    IEnumerator TransitionSequence()
    {
        transition.SetActive(true);
        transitionAnimator.SetTrigger("Play");
        yield return new WaitForSeconds(transitionClip.length);
        transition.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
            bubble.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
            bubble.SetActive(false);
    }
}