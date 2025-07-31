using UnityEngine;

[CreateAssetMenu(fileName = "TileViewSettings", menuName = "Scriptable Objects/TileViewSettings")]
public class TileViewSettings : ScriptableObject
{
    public TileView TilePrefab;

    [Header("Sprites")]
    public Sprite DefaultSprite;
    public Sprite RevealedSprite;
    public Sprite FlaggedSprite;
    public Sprite MinedSprite;
}
