using UnityEngine;

[CreateAssetMenu(fileName = "UIConfig", menuName = "Scriptable Objects/UIConfig")]
public class UIConfig : ScriptableObject
{
    public string WinMessage = "You Won!";
    public string LoseMessage = "Game Over";
}
