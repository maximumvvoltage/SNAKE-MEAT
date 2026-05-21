using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class Errand
{
    public string errandName;
    public ErrandData errandData;
    public List<GameObject> npcTriggers;
    public List<GameObject> completedTriggers;
    public Transform stampTransform;
    public Image stampImage;
    public Sprite claimSprite;
    public Sprite claimedSprite;
    public bool isCompleted = false;
    public bool isClaimed = false;
}

public class ErrandManager : MonoBehaviour
{
    public static ErrandManager instance;
    public List<Errand> errands = new List<Errand>();

    public GameObject stampcardGraphic;
    public Button stampButtonPrefab;
    public DialogueManager dialogueManager;
    public GameObject dialogueUI;

    [Header("Errand Info Display")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        foreach (Errand errand in errands) //with every new errand, make a fresh list for completed triggers, so
            errand.completedTriggers = new List<GameObject>(); //that it doesn't begin with some other errand's triggers
    }
    
    public void RegisterTrigger(string errandName, GameObject trigger)// call this on the final word of a completed errand
    {
        Errand errand = errands.Find(e => e.errandName == errandName);
        if (errand == null || errand.isCompleted) return;

        if (!errand.completedTriggers.Contains(trigger))
            errand.completedTriggers.Add(trigger);

        // only complete if every required NPC has been visited
        if (errand.completedTriggers.Count >= errand.npcTriggers.Count)
        {
            errand.isCompleted = true;
            SpawnStamp(errand);
        }
    }

    public void OpenStampcard()
    {
        stampcardGraphic.SetActive(true);

        if (dialogueManager.dialogueIsPlaying)
            dialogueUI.SetActive(false); 
    }

    public void CloseStampcard()
    {
        stampcardGraphic.SetActive(false);
        
        if (dialogueManager.dialogueIsPlaying)
            dialogueUI.SetActive(true); 
    }

    void SpawnStamp(Errand errand)
    {
        if (errand.stampImage == null) return;

        errand.stampImage.gameObject.SetActive(true);
        errand.stampImage.sprite = errand.claimSprite;

        // wire up click via ClaimButton script
        ClaimButton claimButton = errand.stampImage.GetComponent<ClaimButton>();
        if (claimButton != null)
        {
            claimButton.errandData = errand.errandData;
            claimButton.titleText = titleText;
            claimButton.bodyText = bodyText;
            claimButton.SetClaimable(true, () => ClaimStamp(errand));
        }
    }

    void ClaimStamp(Errand errand)
    {
        if (errand.isClaimed) return;

        errand.isClaimed = true;

        foreach (GameObject trigger in errand.npcTriggers)
            if (trigger != null)
                trigger.SetActive(false);

        errand.stampImage.sprite = errand.claimedSprite;

        ClaimButton claimButton = errand.stampImage.GetComponent<ClaimButton>();
        if (claimButton != null)
            claimButton.SetClaimable(false, null);

        Debug.Log(errand.errandName + " claimed.");
    }

    public bool IsClaimed(string errandName)
    {
        Errand errand = errands.Find(e => e.errandName == errandName);
        return errand != null && errand.isClaimed;
    }
}