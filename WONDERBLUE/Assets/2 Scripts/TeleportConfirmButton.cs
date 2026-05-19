using UnityEngine;
using UnityEngine.UI;

public class TeleportConfirmButton : MonoBehaviour
{
    // I used Claude code to help me with this script so that I didn't have to assign every single tunnel to the confirm teleport button.
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetTunnel(TunnelEntrance tunnel)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(tunnel.ConfirmTeleportButton);
    }
}