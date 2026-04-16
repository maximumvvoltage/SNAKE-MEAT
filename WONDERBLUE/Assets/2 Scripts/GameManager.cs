using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject ground;
    public bool isExpanded;
    public bool gamepaused;
    public Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SoundManager.Play("Song1");
        animator = GetComponent<Animator>();
    }

    //---------------------------------- MENU BUTTONS
    public void OpenOptions()
    {
        options.SetActive(true);
        gamepaused = true;
    }

    public void CloseOptions()
    {
        options.SetActive(false); 
        gamepaused = false;
    }

    public void OpenQuests()
    {
        
    }
}
