using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;
    public AudioClip musicClip;

    [Header("Volume Cels")]
    public GameObject[] cels;        // drag Cel, Cel(1)... Cel(9) in order
    public Button plusButton;
    public Button minusButton;
    private int activeCels = 0;

    [Header("Music Toggle")]
    public bool musicPaused;
    public Image musicImageIcon;
    public Sprite musicOff;
    public Sprite musicOn;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (musicClip != null)
            PlayBGM(false, musicClip);

        musicImageIcon.sprite = musicOn;

        plusButton.onClick.AddListener(VolumeUp);
        minusButton.onClick.AddListener(VolumeDown);

        UpdateVolume();
        
        for (int i = 0; i < 5; i++) // this basically reuses the existing VolumeUp logic so the
            VolumeUp();             // cels activate in order and activeCels are set to 5, making 50% volume from the start
                                    // so the player doesnt get blasted

        
        musicPaused = true; // DEBUG - remove when ready
        audioSource.Pause();
    }

    void VolumeUp()
    {
        if (activeCels < cels.Length)
        {
            cels[activeCels].SetActive(true);
            activeCels++;
            UpdateVolume();
        }
    }

    void VolumeDown()
    {
        if (activeCels > 0)
        {
            activeCels--;
            cels[activeCels].SetActive(false);
            UpdateVolume();
        }
    }

    void UpdateVolume()
    {
        // 1 cel = 10%, 10 cels = 100%
        audioSource.volume = activeCels / (float)cels.Length;

        // grey out buttons at limits
        plusButton.interactable  = activeCels < cels.Length;
        minusButton.interactable = activeCels > 0;
    }

    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;
    }

    public void PlayBGM(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
            audioSource.clip = audioClip;

        if (audioSource.clip != null)
        {
            if (resetSong) audioSource.Stop();
            audioSource.Play();
        }
    }

    public void PauseMusic()
    {
        musicPaused = !musicPaused;

        if (musicPaused)
        {
            musicImageIcon.sprite = musicOff;
            audioSource.Pause();
        }
        else
        {
            musicImageIcon.sprite = musicOn;
            audioSource.UnPause();
        }
    }
}