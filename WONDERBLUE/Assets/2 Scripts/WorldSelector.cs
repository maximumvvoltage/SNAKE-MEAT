using UnityEngine;

/// <summary>
/// WorldSelector — Attach to the Player (or any persistent GameObject).
/// 
/// With the mouse cursor FREE (not locked), left-clicking on any object
/// that is on the "Collectable" layer AND within range will call
/// Collectable.Interact(), printing itemDescription to the Console.
/// 
/// Hover highlight: objects glow (emission boost) when the cursor hovers over them.
/// 
/// Setup:
///   1. Add this script to the Player or a Manager object.
///   2. Assign 'playerTransform' to the Player's Transform.
///   3. Assign 'playerCamera'    to the scene Camera.
///   4. Ensure the "Collectable" layer exists (Project Settings → Tags and Layers).
/// </summary>
public class WorldSelector : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;   // Used to measure distance
    public Camera    playerCamera;      // Used for the raycast

    [Header("Selection")]
    public LayerMask collectableLayer;  // Set to "Collectable" in the Inspector
    public float     maxRayDistance = 50f; // How far the ray reaches into the scene

    [Header("Hover Highlight")]
    public bool  enableHoverHighlight = true;
    public Color hoverTint = new Color(1.3f, 1.3f, 0.8f); // Warm glow

    // ─── Private state ────────────────────────────────────────────────────────

    private Collectable   hoveredCollectable;
    private Renderer      hoveredRenderer;
    private Color         originalEmission;
    private bool          hadEmission;

    void Update()
    {
        HandleHover();
        HandleClick();
    }

    // ─── Hover ────────────────────────────────────────────────────────────────

    void HandleHover()
    {
        if (!enableHoverHighlight) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, collectableLayer))
        {
            Collectable col = hit.collider.GetComponent<Collectable>();
            if (col == null) col = hit.collider.GetComponentInParent<Collectable>();

            if (col != null)
            {
                bool inRange = IsInRange(col);

                if (col != hoveredCollectable)
                {
                    ClearHover();
                    if (inRange) ApplyHover(col, hit.collider.GetComponent<Renderer>()
                                                ?? hit.collider.GetComponentInParent<Renderer>());
                }
                return;
            }
        }

        // Nothing valid under cursor
        ClearHover();
    }

    void ApplyHover(Collectable col, Renderer rend)
    {
        hoveredCollectable = col;
        hoveredRenderer    = rend;

        if (rend != null && rend.material.HasProperty("_EmissionColor"))
        {
            originalEmission = rend.material.GetColor("_EmissionColor");
            hadEmission      = rend.material.IsKeywordEnabled("_EMISSION");
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", hoverTint * 0.4f);
        }
    }

    void ClearHover()
    {
        if (hoveredRenderer != null)
        {
            if (hoveredRenderer.material.HasProperty("_EmissionColor"))
            {
                hoveredRenderer.material.SetColor("_EmissionColor", originalEmission);
                if (!hadEmission) hoveredRenderer.material.DisableKeyword("_EMISSION");
            }
        }

        hoveredCollectable = null;
        hoveredRenderer    = null;
    }

    // ─── Click ────────────────────────────────────────────────────────────────

    void HandleClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, collectableLayer))
            return;

        // Try to find Collectable on the hit object or its parent
        Collectable col = hit.collider.GetComponent<Collectable>();
        if (col == null) col = hit.collider.GetComponentInParent<Collectable>();
        if (col == null) return;

        if (!IsInRange(col))
        {
            Debug.Log($"[WorldSelector] '{col.name}' is too far away.");
            return;
        }

        col.Interact();
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    bool IsInRange(Collectable col)
    {
        if (playerTransform == null) return true; // Fail open if not assigned
        float dist = Vector3.Distance(playerTransform.position, col.transform.position);
        return dist <= col.InteractRange;
    }

    void OnDisable()
    {
        ClearHover(); // Restore materials if script is disabled mid-hover
    }
}