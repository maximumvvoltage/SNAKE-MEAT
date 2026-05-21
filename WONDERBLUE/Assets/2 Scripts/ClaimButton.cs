using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class ClaimButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ErrandData errandData;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;

    private bool isClaimable = false;
    private Action onClaim;

    public void SetClaimable(bool claimable, Action callback)
    {
        isClaimable = claimable;
        onClaim = callback;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (errandData == null) return;
        if (isClaimable)
        {
            titleText.text = "Unclaimed";
            bodyText.text = "Claim your stamp!";
        }

        if (!isClaimable)
        {
            titleText.text = errandData.title;
            bodyText.text = errandData.body;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (errandData == null) return;
        titleText.text = " ";
        bodyText.text = " ";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClaimable || onClaim == null) return;
        onClaim.Invoke();
    }
}