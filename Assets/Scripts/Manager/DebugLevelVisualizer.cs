using UnityEngine;
using UnityEngine.Events;

public class DebugLevelVisualizer : MonoBehaviour
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

    [Space(10)]
    [SerializeField] private GameObject Health;
    [SerializeField] private GameObject Mutation;

    [Space(10)]
    [SerializeField] private GameObject Lizard;
    [SerializeField] private GameObject Bee;
    [SerializeField] private GameObject Spider;
    [SerializeField] private GameObject AcidSpitter;

    [Space(10)]
    [SerializeField] private float xOffset = 0.5f;
    [SerializeField] private float zOffset = 0.5f;

    [Space(10)]
    [SerializeField] private TileMap defaultTileMap;

    [Space(10)]
    [SerializeField] private Transform playerSpawnPoint;

    [Space(10)]
    [SerializeField] private int GridSize;
    [SerializeField] private int RoomMinSize;
    [SerializeField] private int RoomMaxSize;
    [SerializeField] private int EnemyChance;
    [SerializeField] private int MaxEnemies;
    [SerializeField] private int PickupChance;
    [SerializeField] private int LizardChance;
    [SerializeField] private int BeeChance;
    [SerializeField] private int SpiderChance;
    [SerializeField] private int AcidSpitterChance;

    private void Awake()
    {
        
    }

    private void Start()
    {
        if (GameManager.Instance.IsStaticLevel)
        {
            Instantiate(this.Player, new Vector3(playerSpawnPoint.position.x, 0f, playerSpawnPoint.position.z), Quaternion.identity);
            this.levelGenComplete = true;
            this.LevelGenerationFinished?.Invoke();
            return;
        }
        LevelGenerationParameters levelParams = new LevelGenerationParameters(gridXMax: GridSize, gridYMax: GridSize, roomMinSize: RoomMinSize, roomMaxSize: RoomMaxSize, roomPlacementIterations: 80, pathPlacementIterations: 10000, trimCount: 25);
        levelParams.EnemyChance = EnemyChance;
        levelParams.MaximumEnemiesPerRoom = MaxEnemies;
        levelParams.PickupChance = PickupChance;
        levelParams.LizardChance = LizardChance;
        levelParams.BeeChance = BeeChance;
        levelParams.SpiderChance = SpiderChance;
        levelParams.AcidSpitterChance = AcidSpitterChance;
        this.Level = LevelGeneration.CreateLevel(levelParams);
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
                    this.DetermineEnemySpawnType(currentTile);
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
        this.LevelGenerationFinished?.Invoke();
    }

    private void DetermineEnemySpawnType(Tile currentTile)
    {
        switch (currentTile.enemySpawnType)
        {
            case EnemySpawnType.NotSet:
                Instantiate(this.Lizard, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                break;
            case EnemySpawnType.Lizard:
                Instantiate(this.Lizard, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                break;
            case EnemySpawnType.Bee:
                Instantiate(this.Bee, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                break;
            case EnemySpawnType.Spider:
                Instantiate(this.Spider, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                break;
            case EnemySpawnType.AcidSpitter:
                Instantiate(this.AcidSpitter, new Vector3(currentTile.xLocation + this.xOffset, 0f, currentTile.yLocation + this.zOffset), Quaternion.identity);
                break;
        }
    }
}
