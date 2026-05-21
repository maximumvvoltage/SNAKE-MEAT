using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue = null;

    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Errand")]
    public string errandName;

    [Header("Sequence")]
    public DialogueTrigger nextDialogue;

    [SerializeField] private bool isInRange = false;
    private bool hasPlayed = false;

    void Start()
    {
       if (visualCue != null) visualCue.SetActive(false);
    }

    void Update()
    {
       if (visualCue != null)
       {
          if (isInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
          {
             visualCue.SetActive(true);

             if (Input.GetKeyDown(KeyCode.E))
             {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON, this.gameObject);
                
                if (!string.IsNullOrEmpty(errandName))
                    ErrandManager.instance.RegisterTrigger(errandName, this.gameObject);
                
                hasPlayed = true;
                visualCue.SetActive(false);

                if (nextDialogue != null)
                {
                    nextDialogue.enabled = true;
                    this.enabled = false;
                }
             }
          }
          else
          {
             visualCue.SetActive(false);
          }
       }
       else
       {
          if (isInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
          {
             DialogueManager.GetInstance().EnterDialogueMode(inkJSON, this.gameObject);

             if (!string.IsNullOrEmpty(errandName))
                 ErrandManager.instance.RegisterTrigger(errandName, this.gameObject);

             hasPlayed = true;

             if (nextDialogue != null)
             {
                 nextDialogue.enabled = true;
                 this.enabled = false;
             }
          }
       }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
       {
          isInRange = true;
       }
    }

    private void OnTriggerExit(Collider other)
    {
       if (other.CompareTag("Player"))
       {
          isInRange = false;
       }
    }
}