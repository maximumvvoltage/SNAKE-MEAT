using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    
    public static AudioSource audioSource;
    private static AllSounds allSounds;

    [Header("Volume Cels")]
    public GameObject[] cels;
    private int activeCels = 0;
// I GOT LAZY THIS LITERALLY USES THE EXACT SAME SYSTEM FOR MUSIC MANAGER
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            allSounds = GetComponent<AllSounds>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (GameObject cel in cels)
            cel.SetActive(false);

        for (int i = 0; i < 5; i++)
            VolumeUp();
    }

    public void VolumeUp()
    {
        if (activeCels < cels.Length)
        {
            cels[activeCels].SetActive(true);
            activeCels++;
            UpdateVolume();
        }
    }

    public void VolumeDown()
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
        audioSource.volume = activeCels / (float)cels.Length;
    }
    
    public static void Play(string soundName)
    { 
        AudioClip audioClip = allSounds.GetRandomClip(soundName);
        if (audioSource != null)
            audioSource.PlayOneShot(audioClip);
    }
    
    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}