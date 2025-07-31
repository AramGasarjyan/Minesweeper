using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int BoardWidth;
    public int BoardHeight;
    public int BoardMinesCount;
}
