using System.Collections;
using UnityEngine;

public class WaterStepper : MonoBehaviour
{
    [Header("Settings")]
    public GameObject waterStepPrefab;
    public BoxCollider feetCollider;
    public float stepCooldown = 0.7f;

    private bool _isInAir = false;
    private bool _onCooldown = false;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Air")) return;
        _isInAir = true;

        if (!_onCooldown)
        {
            StartCoroutine(SpawnStepEffect());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Air"))
        {
            _isInAir = false;
        }
    }

    private IEnumerator SpawnStepEffect()
    {
        _onCooldown = true;

        // Spawn at the feet collider's world-space center
        Vector3 spawnPosition = feetCollider != null ? feetCollider.transform.TransformPoint(feetCollider.center) : transform.position;

        GameObject stepEffect = Instantiate(waterStepPrefab, spawnPosition, waterStepPrefab.transform.rotation);
        
        Animator animator = stepEffect.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("WaterStep");
            yield return null;
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
        }

        Destroy(stepEffect);

        float remainingCooldown = stepCooldown - (animator != null ? animator.GetCurrentAnimatorStateInfo(0).length : 0.5f);

        if (remainingCooldown > 0)
            yield return new WaitForSeconds(remainingCooldown);

        _onCooldown = false;
    }
}