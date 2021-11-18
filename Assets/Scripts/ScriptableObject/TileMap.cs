using UnityEngine;

[CreateAssetMenu(fileName = "TileMap.asset", menuName = "Tile Map")]
public class TileMap : ScriptableObject
{
    public Texture None;
    public Texture Left;
    public Texture Right;
    public Texture Below;
    public Texture Above;
    [Space(5)]
    public Texture BelowLeft;
    public Texture BelowRight;
    public Texture AboveRight;
    public Texture AboveLeft;
    public Texture LeftRight;
    public Texture AboveBelow;
    [Space(5)]
    public Texture AboveBelowLeft;
    public Texture AboveBelowRight;
    public Texture AboveLeftRight;
    public Texture BelowLeftRight;
    [Space(5)]
    public Texture All;
}
