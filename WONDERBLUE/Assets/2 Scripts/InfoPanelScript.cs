using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoPanelScript : MonoBehaviour
{
    [System.Serializable]
    public class InfoSlide
    {
        public string title;
        [TextArea] public string body;
    }

    [Header("Slides")]
    public InfoSlide[] slides;

    [Header("References")]
    public GameObject bubble;
    public GameObject panel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI counterText;
    public Button nextButton;
    public Button prevButton;

    private bool playerInside = false;
    private int currentSlide = 0;

    void Start()
    {
        nextButton.onClick.AddListener(NextSlide);
        prevButton.onClick.AddListener(PrevSlide);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            currentSlide = 0;
            UpdatePanel();
            panel.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            bubble.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            bubble.SetActive(false);
            panel.SetActive(false);
        }
    }

    void NextSlide()
    {
        if (currentSlide < slides.Length - 1)
        {
            currentSlide++;
            UpdatePanel();
        }
    }

    void PrevSlide()
    {
        if (currentSlide > 0)
        {
            currentSlide--;
            UpdatePanel();
        }
    }

    void UpdatePanel()
    {
        titleText.text = slides[currentSlide].title;
        bodyText.text  = slides[currentSlide].body;
        counterText.text = $"{currentSlide + 1} / {slides.Length}";

        prevButton.interactable = currentSlide > 0;
        nextButton.interactable = currentSlide < slides.Length - 1;
    }
}