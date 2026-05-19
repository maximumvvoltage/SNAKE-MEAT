using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "InfoPanel", menuName = "Scriptable Objects/InfoPanel")]
public class InfoPanel : ScriptableObject
{
    public int totalPanels;
    public string[] title;
    public string[] body;
    public Image[] pictures;

}
