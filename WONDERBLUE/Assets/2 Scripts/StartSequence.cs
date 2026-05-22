using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartSequence : MonoBehaviour
{
    [Header("References")]
    public RawImage renderer;
    public MusicManager musicManager;
    public PlayerV4 playerV4;

    [Header("Intro Sequence")]
    public GameObject betterWithSound;
    public AnimationClip betterWithSoundClip;

    void Start()
    {
        musicManager.PauseMusic(); // toggles music off
        renderer.color = Color.black;
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        betterWithSound.SetActive(true);
        yield return new WaitForSeconds(betterWithSoundClip.length);
        betterWithSound.SetActive(false);

        musicManager.PauseMusic(); // toggles music on

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            renderer.color = Color.Lerp(Color.black, Color.white, elapsed);
            yield return null;
        }
        renderer.color = Color.white;
    }
}