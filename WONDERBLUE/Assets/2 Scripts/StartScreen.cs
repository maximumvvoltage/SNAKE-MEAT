using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    [Header("References")]
    public RawImage renderer;
    public Transform cameraTransform;

    [Header("Start Menu Objects")] 
    public bool started;
    public GameObject betterWithSound;
    public AnimationClip betterWithSoundClip;
    
    public GameObject startMenuObjects;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        renderer.color = Color.black;
        StartCoroutine(StartSequence());
        started = true;
        HandleStart();
    }

    IEnumerator StartSequence()
    {
        betterWithSound.SetActive(true);
        
        yield return new WaitForSeconds(betterWithSoundClip.length);

        betterWithSound.SetActive(false);

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            renderer.color = Color.Lerp(Color.black, Color.white, elapsed); // thiss line makes the colour of the Renderer transition from black to white in 1 second (the elapsed time)
            yield return null;
        }

        renderer.color = Color.white;
    }

    public void HandleStart() //literally the exact same as HandlePause in player V4 but I was too lazy to optimize the code
    {
        startMenuObjects.SetActive(true);

        started = !started;

        if (started)
        {
            cameraTransform.position = startPosition; //prepausee saves the cam's current transform position so that it
            cameraTransform.rotation = startRotation;  // can move back into place upon unpausing
            startMenuObjects.SetActive(true);
        }
        if (!started)
            startMenuObjects.SetActive(false);
    }

    public void NewGame()
    {
        
    }

    public void LoadGame()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}