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

public enum PickupState
{
    NotSet = 0,
    HealthPickup = 1,
    MutationPickup = 2
}

public class Tile
{
    public int xLocation;
    public int yLocation;

    public int RoomID;
    public int PathID;

    public bool spawnPickup;
    public bool spawnEnemy;

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

        this.RoomID = -1;
        this.PathID = -1;

        this.spawnEnemy = false;
        this.spawnPickup = false;

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
}
