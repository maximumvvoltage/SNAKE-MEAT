using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Item Data")]
    public CollectableSO collectableSO;

    public void Interact()
    {
        if (collectableSO == null)
        {
            Debug.LogWarning($"[Collectable] '{name}' has no CollectableData assigned!");
            return;
        }

        Debug.Log((collectableSO.itemName) + (collectableSO.itemDescription));
    }

    public float InteractRange =>
        collectableSO != null ? collectableSO.interactRange : 3f;
}
