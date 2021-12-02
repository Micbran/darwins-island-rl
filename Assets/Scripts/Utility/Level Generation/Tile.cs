using System.Collections.Generic;

public enum TileState
{
    NotSet = 0,
    Block = 1,
    Room = 2,
    Open = 3,
    Path = 4,
    Door = 5
}

public enum Direction // optimally this would be like a bit mask but i'm too lazy to engineer that
{
    None = 0,
    Left = 1,
    Right = 2,
    Below = 3,
    Above = 4,
    BelowLeft = 5,
    BelowRight = 6,
    AboveRight = 7,
    AboveLeft = 8,
    LeftRight = 9,
    AboveBelow = 10,
    AboveBelowLeft = 11,
    AboveBelowRight = 12,
    AboveLeftRight = 13,
    BelowLeftRight = 14,
    AboveBelowLeftRight = 15,
}

public enum PickupState
{
    NotSet = 0,
    HealthPickup = 1,
    MutationPickup = 2
}

public enum EnemySpawnType
{
    NotSet = 0,
    Lizard,
    Bee,
    Spider,
    AcidSpitter
}

public class Tile
{
    public int xLocation;
    public int yLocation;

    public int torchChance;

    public int RoomID;
    public int PathID;

    public bool spawnPickup;
    public bool spawnEnemy;

    public EnemySpawnType enemySpawnType;

    public bool playerSpawn;
    public bool stairsSpawn;
    public bool pathCandidate;
    public bool connectCandidate;
    public bool pickedTile;
    public bool lastPath;

    public List<Tile> neighbors;
    public List<int> connectRoomIds;
    public List<int> connectPathIds;

    public TileState state;
    public PickupState pickupState;

    public Tile()
    {
        this.xLocation = 0;
        this.yLocation = 0;

        this.torchChance = 0;

        this.RoomID = -1;
        this.PathID = -1;

        this.spawnEnemy = false;
        this.spawnPickup = false;

        this.enemySpawnType = EnemySpawnType.NotSet;

        this.playerSpawn = false;
        this.stairsSpawn = false;
        this.pathCandidate = false;
        this.connectCandidate = false;
        this.pickedTile = false;
        this.lastPath = false;

        this.neighbors = new List<Tile>();
        this.connectRoomIds = new List<int>();
        this.connectPathIds = new List<int>();

        this.state = TileState.NotSet;
    }

    public Direction ReturnNeighborBlocks()
    {
        bool left = false;
        bool right = false;
        bool above = false;
        bool below = false;

        foreach (Tile neigh in this.neighbors)
        {
            if (neigh.state == TileState.Block)
            {
                left = neigh.xLocation > this.xLocation && neigh.yLocation == this.yLocation || left; // left
                right = neigh.xLocation < this.xLocation && neigh.yLocation == this.yLocation || right; // right
                below = neigh.yLocation > this.yLocation && neigh.xLocation == this.xLocation || below; // down
                above = neigh.yLocation < this.yLocation && neigh.xLocation == this.xLocation || above; // up
            }
        }

        if (above && below && left && right)
            return Direction.AboveBelowLeftRight;

        if (above && below && left)
            return Direction.AboveBelowLeft;
        if (above && below && right)
            return Direction.AboveBelowRight;
        if (below && left && right)
            return Direction.BelowLeftRight;
        if (above && left && right)
            return Direction.AboveLeftRight;
        
        if (left && right)
            return Direction.LeftRight;
        if (above && left)
            return Direction.AboveLeft;
        if (above && right)
            return Direction.AboveRight;
        if (above && below)
            return Direction.AboveBelow;
        if (below && left)
            return Direction.BelowLeft;
        if (below && right)
            return Direction.BelowRight;

        if (left)
            return Direction.Left;
        if (right)
            return Direction.Right;
        if (above)
            return Direction.Above;
        if (below)
            return Direction.Below;

        return Direction.None;
    }
}
