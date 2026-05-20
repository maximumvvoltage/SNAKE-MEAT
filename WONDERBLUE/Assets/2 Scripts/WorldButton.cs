using UnityEngine;

public class WorldButtonSelector : MonoBehaviour //confusing naming conventions: THIS IS THE PER-OBJECT SCRIPT
{
    public string buttonName;

    public void OnClick(PlayerV4 playerV4)
    {
        switch (buttonName)
        {
            case "Options":
                playerV4.HandleOptions();
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }
}