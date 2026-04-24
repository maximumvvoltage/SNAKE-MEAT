using UnityEngine;
using UnityEngine.UI;

public class DrawingController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float opacityIncreasePerSecond = 50f;

    [Header("References (optional)")]
    [SerializeField] private ScreenController screenController;
    [SerializeField] private string fullyRevealedSubtitle = "";

    private Image image;
    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isComplete = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // this diisables raycasting on the image entirely. we do our own hover check so this is not needed
        image.raycastTarget = false;
        SetAlpha(0f);
    }

    private void Update()
    {
        if (isComplete) return;
        if (!Input.GetMouseButton(0)) return;
        if (!IsMouseOver()) return;

        Color c = image.color;
        c.a = Mathf.Min(1f, c.a + (opacityIncreasePerSecond / 255f) * Time.deltaTime);
        image.color = c;

        if (c.a >= 1f)
        {
            isComplete = true;

            if (screenController != null && !string.IsNullOrEmpty(fullyRevealedSubtitle))
                screenController.ShowSubtitle(fullyRevealedSubtitle);
        }
    }

    private bool IsMouseOver()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera
        );
    }

    private void SetAlpha(float a)
    {
        if (image == null) return;
        Color c = image.color;
        c.a = a;
        image.color = c;
    }
}