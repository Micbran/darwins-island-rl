using UnityEngine;
using UnityEngine.Events;

public class LevelVisualizer : MonoBehaviour
{
    public UnityEvent LevelGenerationFinished = new UnityEvent();
    private Tile[,] Level;
    private bool levelGenComplete = false;

    [SerializeField] private LevelBlock Block;
    [SerializeField] private GameObject Room;
    [SerializeField] private GameObject Path;
    [SerializeField] private GameObject NotSet;
    [SerializeField] private GameObject Open;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Stairs;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private GameObject Health;
    [SerializeField] private GameObject Mutation;

    [SerializeField] private float xOffset = 0.5f;
    [SerializeField] private float zOffset = 0.5f;

    [Space(10)]
    [SerializeField] private TileMap defaultTileMap;

    private void Awake()
    {
        this.Level = LevelGeneration.CreateLevel(new LevelGenerationParameters(gsx: 21, gsy: 21, rmins: 3, rmaxs: 10, rpi: 100, ppi: 1000, tc: 100));
        for (int row = 0; row < Level.GetLength(0); row++)
        {
            for (int column = 0; column < Level.GetLength(1); column++)
            {
                Tile currentTile = Level[row, column];
                switch (currentTile.state)
                {
                    case TileState.Block:
                        LevelBlock block = Instantiate(this.Block, new Vector3(currentTile.xLocation + this.xOffset, 2.5f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                        block.DetermineTexture(this.defaultTileMap, currentTile);
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
                    switch (currentTile.pickupState)
                    {
                        case PickupState.HealthPickup:
                            Instantiate(this.Health, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                            break;

                        case PickupState.MutationPickup:
                            Instantiate(this.Mutation, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                            break;
                    }

                }
            }
        }
        this.levelGenComplete = true;
        Debug.Log("Instantiation complete.");
    }

    private void Start()
    {
        this.LevelGenerationFinished?.Invoke();
    }
}
