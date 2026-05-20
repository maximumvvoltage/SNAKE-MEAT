using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    [Header("References")] public RawImage renderer;
    public Transform startingCameraTransform;
    private MusicManager musicManager;
    private PlayerV4 playerV4;

    [Header("Start Menu Objects")] public bool started;
    public GameObject betterWithSound;
    public AnimationClip betterWithSoundClip;

    public GameObject startMenuObjects;
    public Camera mainCamera;
    private Vector3 normalPosition;
    private Quaternion normalRotation;
    public float startCameraMoveSpeed;

    void Start()
    {
        renderer.color = Color.black;
        StartCoroutine(StartSequence());

        started = true;
        musicManager.PauseMusic(); //toggle locks it into pause mode
        startMenuObjects.SetActive(true);
        mainCamera.transform.position = startingCameraTransform.position;
        mainCamera.transform.rotation = startingCameraTransform.rotation;
    }

    IEnumerator StartSequence()
    {
        betterWithSound.SetActive(true);
        yield return new WaitForSeconds(betterWithSoundClip.length);
        betterWithSound.SetActive(false);

        musicManager.PauseMusic(); //toggle unlocks it into play mode

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            renderer.color = Color.Lerp(Color.black, Color.white, elapsed); // this makes the colour of the
                                                                            // Renderer transition from black to white in 1 second (the elapsed time)
            yield return null;
        }

        renderer.color = Color.white;
    }
}
