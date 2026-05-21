using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class ErrandData : ScriptableObject
{
    public string title;
    public string body;
}
