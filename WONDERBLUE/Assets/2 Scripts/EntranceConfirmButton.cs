using UnityEngine;
using UnityEngine.UI;

public class EntranceConfirmButton : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetEntrance(EnterTrigger entrance)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(entrance.OnGoPressed);
    }
}