using UnityEngine;


public class CursorController : MonoBehaviour
{
    [Header("Cursor Textures")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D altCursor;

    [Header("Hotspot (click point within the texture, in pixels)")]
    [SerializeField] private Vector2 defaultHotspot = Vector2.zero;
    [SerializeField] private Vector2 altHotspot = Vector2.zero;

    private void Start()
    {
        ApplyDefaultCursor();
    }

    public void ApplyDefaultCursor()
    {
        if (defaultCursor == null)
        {
            Debug.LogWarning("CursorController: defaultCursor texture is not assigned.");
            return;
        }
        Cursor.SetCursor(defaultCursor, defaultHotspot, CursorMode.Auto);
    }


    public void SwapToAltCursor()
    {
        if (altCursor == null)
        {
            Debug.LogWarning("CursorController: altCursor texture is not assigned.");
            return;
        }
        Cursor.SetCursor(altCursor, altHotspot, CursorMode.Auto);
    }

    public void ResetToSystemCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}