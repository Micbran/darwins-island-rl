public class LevelGenerationParameters
{
    public int GridSizeX;
    public int GridSizeY;

    public int RoomMinSize;
    public int RoomMaxSize;

    public int RoomPlacementIterations;
    public int PathPlacementIterations;

    public int TrimCount;

    public int MinimumStairsDistance = 10;
    public int MaximumEnemiesPerRoom = 3;
    public int MinimumEnemiesPerRoom = 0;
    public int EnemyChance = 50;
    public int MaximumPickupsPerRoom = 2;
    public int MinimumPickupsPerRoom = 0;
    public int PickupChance = 40;

    public LevelGenerationParameters(int gsx = 81, int gsy = 81, int rmins = 3, int rmaxs = 10, int rpi = 80, int ppi = 10000, int tc = 25)
    {
        if (gsx % 2 is 0)
        {
            gsx++;
        }
        if (gsy % 2 is 0)
        {
            gsy++;
        }
        this.GridSizeX = gsx;
        this.GridSizeY = gsy;

        this.RoomMinSize = rmins;
        this.RoomMaxSize = rmaxs;

        this.RoomPlacementIterations = rpi;
        this.PathPlacementIterations = ppi;

        this.TrimCount = tc;
    }
    
}
