using UnityEngine;
using UnityEngine.Events;

public class LevelVisualizer : MonoBehaviour
{
    public UnityEvent LevelGenerationFinished = new UnityEvent();
    private Tile[,] Level;

    [SerializeField] private GameObject Block;
    [SerializeField] private GameObject Room;
    [SerializeField] private GameObject Path;
    [SerializeField] private GameObject NotSet;
    [SerializeField] private GameObject Open;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Stairs;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private GameObject Health;

    [SerializeField] private float xOffset = 0.5f;
    [SerializeField] private float zOffset = 0.5f;

    private void Awake()
    {
        this.Level = LevelGeneration.CreateLevel(new LevelGenerationParameters(gsx: 15, gsy: 15, rmins: 3, rmaxs: 10, rpi: 1000, ppi: 10000, tc: 100));
        for (int row = 0; row < Level.GetLength(0); row++)
        {
            for (int column = 0; column < Level.GetLength(1); column++)
            {
                Tile currentTile = Level[row, column];
                switch (currentTile.state)
                {
                    case TileState.Block:
                        Instantiate(this.Block, new Vector3(currentTile.xLocation + this.xOffset, 2.5f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                        break;
                    case TileState.NotSet:
                        Instantiate(this.NotSet, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                        break;
                    case TileState.Open:
                        Instantiate(this.Open, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                        break;
                    case TileState.Path:
                        Instantiate(this.Path, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                        break;
                    case TileState.Door:
                    case TileState.Room:
                        Instantiate(this.Room, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                        break;
                }
                if (currentTile.playerSpawn)
                {
                    Instantiate(this.Player, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                }
                if (currentTile.stairsSpawn)
                {
                    Instantiate(this.Stairs, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                }
                if (currentTile.spawnEnemy)
                {
                    Instantiate(this.Enemy, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);

                }
                if (currentTile.spawnPickup)
                {
                    Instantiate(this.Health, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);

                }
            }
        }
        this.LevelGenerationFinished?.Invoke();
        Debug.Log("Instantiation complete.");
    }

    private void Start()
    {

    }
}
