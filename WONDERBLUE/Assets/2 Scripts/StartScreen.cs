using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    [Header("References")]
    public RawImage renderer;
    public MusicManager musicManager;
    public PlayerV4 playerV4;
    public Camera mainCamera;
    

    [Header("Start Menu")]
    public Transform startMenuTransform;
    public GameObject startMenuObjects;

    [Header("Intro Sequence")]
    public GameObject betterWithSound;
    public AnimationClip betterWithSoundClip;

    void Start()
    {
        renderer.color = Color.black;
        
        musicManager.PauseMusic();
        startMenuObjects.SetActive(true);

        StartCoroutine(StartSequence());
    }

    void Update()
    {
        if (playerV4.isTeleporting)
        {
            mainCamera.transform.position = startMenuTransform.position;//makes the cam face the start position
            mainCamera.transform.rotation = startMenuTransform.rotation;
            //playerV4.SlideCameraTo(startMenuTransform.position, startMenuTransform.rotation);
        }
            
    }

    IEnumerator StartSequence()
    {
        playerV4.jogSpeed = 0f;
        playerV4.runSpeed = 0f;
        
        
        
        betterWithSound.SetActive(true);
        yield return new WaitForSeconds(betterWithSoundClip.length);
        betterWithSound.SetActive(false);

        musicManager.PauseMusic(); // toggle unlocks into play mode
        

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            renderer.color = Color.Lerp(Color.black, Color.white, elapsed);
            yield return null;
        }

        renderer.color = Color.white;
    }

    public void HandleStart()
    {
        playerV4.jogSpeed = 8f;
        playerV4.runSpeed = 15f;
        startMenuObjects.SetActive(false);
        playerV4.isTeleporting = false;
    }
}