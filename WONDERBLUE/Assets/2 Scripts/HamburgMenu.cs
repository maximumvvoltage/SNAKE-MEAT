using UnityEngine;
using UnityEngine.EventSystems;

public class HamburgMenu : MonoBehaviour, IPointerExitHandler
{
    public bool isExpanded;
    public Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void OpenMenu()
    {
        if (!isExpanded && animator != null)
        {
            animator.SetTrigger("Hovered");
            isExpanded = true;
            Debug.Log("Menu Expanded");
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isExpanded && animator != null)
        {
            animator.SetTrigger("NotHovered");
            isExpanded = false;
        }
    }
}