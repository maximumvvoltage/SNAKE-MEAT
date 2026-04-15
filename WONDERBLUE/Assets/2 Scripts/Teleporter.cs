using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Transform destination;
    public Animator transitionAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportAfterDelay(other.gameObject));
        }
    }

    private IEnumerator TeleportAfterDelay(GameObject player)
    {
        transitionAnimator.SetTrigger("Play");
        Debug.Log("entered tunnel");

        yield return new WaitForSeconds(1f);

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = destination.position;
            player.transform.rotation = destination.rotation;
            cc.enabled = true;
        }
        else
        {
            player.transform.position = destination.position;
            player.transform.rotation = destination.rotation;
        }
        
        transitionAnimator.SetTrigger("Idle");
    }
}