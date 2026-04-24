using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image overlayImage;
    [SerializeField] private TextMeshProUGUI subtitleText;

    [Header("Initial Settings")]
    [SerializeField] private string openingSubtitle = "Your opening subtitle here...";
    [SerializeField] private string postFadeSubtitle = "Your second subtitle here...";
    [SerializeField] private float transitionDelay = 5f;

    private void Start()
    {

        SetOverlayColour(Color.black);
        ShowSubtitle(openingSubtitle);

        StartCoroutine(InitialTransition());
    }

    private IEnumerator InitialTransition()
    {
        yield return new WaitForSeconds(transitionDelay);

        SetOverlayColour(Color.white);
        ShowSubtitle(postFadeSubtitle);
    }


    public void ShowSubtitle(string text)
    {
        if (subtitleText == null) return;
        subtitleText.text = text;
    }

    public void ClearSubtitle()
    {
        if (subtitleText == null) return;
        subtitleText.text = string.Empty;
    }

    public void SetOverlayColour(Color colour)
    {
        if (overlayImage == null) return;
        overlayImage.color = colour;
    }
    public void FadeOutOverlay(float duration)
    {
        StartCoroutine(FadeCoroutine(overlayImage.color, Color.clear, duration));
    }

    private IEnumerator FadeCoroutine(Color from, Color to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            overlayImage.color = Color.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        overlayImage.color = to;
    }
}