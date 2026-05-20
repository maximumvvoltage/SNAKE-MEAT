using UnityEngine;

public class WorldButtons : MonoBehaviour //confusing naming conventions: THIS IS THE MOTHER SCRIPT THAT HANDLES RAYCASTING
{
    public Camera mainCamera;
    private PlayerV4 playerV4;

    void Start()
    {
        playerV4 = FindFirstObjectByType<PlayerV4>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                WorldButtonSelector button = hit.collider.GetComponent<WorldButtonSelector>();
                if (button != null)
                    button.OnClick(playerV4);
            }
        }
    }
}