using UnityEngine;
using UnityEngine.EventSystems;

public class HamburgerMenuController : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] private Animator menuAnimator;
    private bool isExpanded = false;
    
    public void OpenMenu()
    {
        if (!isExpanded && menuAnimator != null)
        {
            menuAnimator.SetTrigger("Hovered");
            isExpanded = true;
            Debug.Log("Menu Expanded");
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isExpanded && menuAnimator != null)
        {
            menuAnimator.SetTrigger("NotHovered");
            isExpanded = false;
        }
    }
}