using UnityEngine;
using UnityEngine.UI;

public class WorldButtons : MonoBehaviour //Extremely complex code for handling raycasting with in-world text buttons
                                          //through a render texture.
                                          //I used Claude Code to help me on this.
{
    public Camera mainCamera;
    public RawImage renderTextureDisplay; // the RawImage showing the RenderTexture

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // convert mouse position to RenderTexture UV space
        RectTransform rt = renderTextureDisplay.rectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out localPoint);

        // normalize to 0-1
        float u = (localPoint.x / rt.rect.width) + 0.5f;
        float v = (localPoint.y / rt.rect.height) + 0.5f;

        if (u < 0 || u > 1 || v < 0 || v > 1) return; // clicked outside the RenderTexture

        // convert UV to viewport ray
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(u, v, 0));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            WorldButtonSelector button = hit.collider.GetComponent<WorldButtonSelector>();
            if (button != null)
                button.OnClick(playerV4);
        }
        else
            Debug.Log("Hit nothing");
    }

    private PlayerV4 playerV4;

    void Start()
    {
        playerV4 = FindFirstObjectByType<PlayerV4>();
    }
}