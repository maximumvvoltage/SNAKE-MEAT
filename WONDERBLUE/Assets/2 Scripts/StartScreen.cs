using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    [Header("References")]
    public RawImage renderer;
    public Transform cameraTransform;
    public Transform startMenuTransform;

    [Header("Start Menu Objects")] 
    public bool started;
    public GameObject betterWithSound;
    public AnimationClip betterWithSoundClip;
    
    public GameObject startMenuObjects;
    private Vector3 normalPosition;
    private Quaternion normalRotation;
    public float startCameraMoveSpeed;

    void Start()
    {
        normalPosition = cameraTransform.position; //normal pos + rot saves the cam's current transform position so that it
        normalRotation = cameraTransform.rotation; // can move back into place upon unpausing
        
        renderer.color = Color.black;
        StartCoroutine(StartSequence());
        started = true;
        SlideCameraTo(startMenuTransform.position, startMenuTransform.rotation);
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
        started = !started;
        
        if (started)
        {
            cameraTransform.position = startMenuTransform.position;
            cameraTransform.rotation = startMenuTransform.rotation;  
            startMenuObjects.SetActive(true);
        }
        if (!started)
            startMenuObjects.SetActive(false);
    }
    
    void SlideCameraTo(Vector3 targetPos, Quaternion targetRot)
    {
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, startCameraMoveSpeed * Time.deltaTime);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRot, startCameraMoveSpeed * Time.deltaTime);
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