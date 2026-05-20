using UnityEngine;
using UnityEngine.EventSystems;

public class WorldButtonSelector : MonoBehaviour
{
    public string buttonName;
    private PlayerV4 playerV4;
    private MusicManager musicManager;
    private SoundManager sfxManager;

    void Start()
    {
        playerV4 = FindFirstObjectByType<PlayerV4>();
    }

    public void OnClick(PlayerV4 playerV4)
    {
        switch (buttonName)
        {
            case "Options":
                playerV4.HandleOptions();
                break;
            case "Reset":
                playerV4.ResetScene();
                break;
            case "Quit":
                Application.Quit();
                break;
            case "+VolumeMusic":
                FindFirstObjectByType<MusicManager>().VolumeUp();
                break;
            case "-VolumeMusic":
                FindFirstObjectByType<MusicManager>().VolumeDown();
                break;
            case "+VolumeSFX":
                FindFirstObjectByType<SoundManager>().VolumeUp();
                break;
            case "-VolumeSFX":
                FindFirstObjectByType<SoundManager>().VolumeDown();
                break;
        }
    }
}