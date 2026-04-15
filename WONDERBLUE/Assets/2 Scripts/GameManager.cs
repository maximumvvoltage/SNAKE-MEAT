using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject ground;
    public bool hamburger;
    public bool gamepaused;
    public Animator animator;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SoundManager.Play("Song1");
        animator = GetComponent<Animator>();
        SetupGround();
    }
    
    public void SetupGround()
    {
        ground.name = "Ground";
        ground.layer = LayerMask.NameToLayer("Ground"); //basically automatically sets it up with the name + layer in case something corrupts
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HamburgerMenuOut();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HamburgerMenuIn();
    }
    
    public void HamburgerMenuOut()
    {
        animator.SetBool("isHovered", true);
    }

    public void HamburgerMenuIn()
    {
        animator.SetBool("isHovered", true);
    }

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
