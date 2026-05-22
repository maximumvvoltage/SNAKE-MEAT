using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public PlayerV4 playerV4;
    public bool startMenu;

    public Transform startTransform;
    public Transform optionsMenuTransform;

    public GameObject startMenuObjects;
    public GameObject optionsMenuObjects;

    void Start()
    {
        startMenu = true;
        playerV4.isTeleporting = true;
        playerV4.isOptions = false;
        startMenuObjects.SetActive(true);
        optionsMenuObjects.SetActive(false);
    }

    void Update()
    {
        if (!startMenu) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playerV4.isOptions)// turns options off, return camera, and turn start menu items back on
            {
                playerV4.isOptions = false;
                startMenuObjects.SetActive(true);
                optionsMenuObjects.SetActive(false);
            }
        }

        if (playerV4.isOptions)
            playerV4.SlideCameraTo(optionsMenuTransform.position, optionsMenuTransform.rotation);
        else
            playerV4.SlideCameraTo(startTransform.position, startTransform.rotation);
    }

    public void StartPressed()
    {
        startMenu = false;
        startMenuObjects.SetActive(false);
        optionsMenuObjects.SetActive(false);
        playerV4.isTeleporting = false;
        playerV4.isOptions = false;
        this.enabled = false;
    }
}