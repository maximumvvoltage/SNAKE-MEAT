using UnityEngine;
using TMPro;

public class PauseText : MonoBehaviour
{
    // Being unable to figure this out, I used Clause code for this pause menu script.

    public TextMeshPro letterPrefab;
    public string text = "PLAYER ONE";
    public float radius = 0.5f;    // match this to your player's head width
    public float arcDegrees = 180f; // 180 = ear to ear around the front

    void Start() {
        float angleStep = arcDegrees / (text.Length - 1);
        float startAngle = 90 + arcDegrees / 2f;

        for (int i = 0; i < text.Length; i++) {
            float angle = startAngle - (angleStep * i);
            float rad = angle * Mathf.Deg2Rad;

            // Now curving around Y axis (ear to ear)
            Vector3 pos = new Vector3(
                Mathf.Cos(rad) * radius,   // left/right
                0,                          // height (adjust on the parent object)
                Mathf.Sin(rad) * radius    // forward/back depth
            );

            var letter = Instantiate(letterPrefab, transform);
            letter.transform.localPosition = pos;

            // Letters rotate to face OUTWARD from the center
            letter.transform.localRotation = Quaternion.Euler(0, -(angle - 90), 0);
            letter.text = text[i].ToString();
        }
    }
}