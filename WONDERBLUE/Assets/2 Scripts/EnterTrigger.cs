using UnityEngine;
using UnityEngine.UI;

public class EnterTrigger : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public Transform cameraTransform;
    public PlayerV4 playerController;

    [Header("Area Settings")]
    public string areaName;
    public Transform previewPoint;
    public GameObject enterUI;
    public Button go;

    [Header("Settings")]
    public float cameraFlySpeed = 3f;

    private bool isEntered = false;

    void Update()
    {
        if (!isEntered) return;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, previewPoint.position, cameraFlySpeed * Time.deltaTime);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, previewPoint.rotation, cameraFlySpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEntered = true;
            playerController.isTeleporting = true;
            enterUI.SetActive(true);

            go.onClick.RemoveAllListeners();
            go.onClick.AddListener(OnGoPressed);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEntered = false;
            playerController.isTeleporting = false;
            enterUI.SetActive(false);
        }
    }

    public void OnGoPressed()
    {
        Debug.Log("Player entered: " + areaName);
    }
}