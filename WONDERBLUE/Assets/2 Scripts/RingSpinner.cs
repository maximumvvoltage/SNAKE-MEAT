using UnityEngine;

public class RingSpinner : MonoBehaviour
{
    [Header("Spin Settings")]
    [SerializeField] private float ring1Speed = 0.5f;
    [SerializeField] private float ring2Speed = 0.2f;
    private (Transform child, float speed)[] spinners;

    void Start()
    {
        var tempList = new System.Collections.Generic.List<(Transform, float)>();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Ring1"))
                tempList.Add((child, ring1Speed));
            else if (child.CompareTag("Ring2"))
                tempList.Add((child, ring2Speed));
        }

        spinners = tempList.ToArray(); // assorts the children of the skypool with tags "ring1" or "ring2" into
        // an array, so that the script doesnt have to be attached to every single child object
    }

    void Update()
    {
        foreach (var (child, speed) in spinners)
        {
            if (child == null) continue;
            child.Rotate(0f, speed * Time.deltaTime, 0f, Space.Self);
        }
    }
}