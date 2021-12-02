using UnityEngine;

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
    public int PickupHealthChance = 60;
    public int PickupMutationChance = 40;
    public int LizardChance = 50;
    public int BeeChance = 50;
    public int SpiderChance = 0;
    public int AcidSpitterChance = 0;

    public LevelGenerationParameters(int gridXMax = 81, int gridYMax = 81, int roomMinSize = 3, int roomMaxSize = 10, int roomPlacementIterations = 80, int pathPlacementIterations = 10000, int trimCount = 25)
    {
        if (gridXMax % 2 is 0)
        {
            gridXMax++;
        }
        if (gridYMax % 2 is 0)
        {
            gridYMax++;
        }
        this.GridSizeX = Mathf.Max(gridXMax, gridYMax);
        this.GridSizeY = Mathf.Max(gridYMax, gridXMax);

        this.RoomMinSize = roomMinSize;
        this.RoomMaxSize = roomMaxSize;

        this.RoomPlacementIterations = roomPlacementIterations;
        this.PathPlacementIterations = pathPlacementIterations;

        this.TrimCount = trimCount;
    }

    public static LevelGenerationParameters Builder(int currentFloor)
    {
        LevelGenerationParameters levelParams = new LevelGenerationParameters(
            gridXMax: 20 + GlobalRandom.RandomInt(currentFloor + 1, 2),
            gridYMax: 20 + GlobalRandom.RandomInt(currentFloor + 1, 2),
            roomMinSize: 3,
            roomMaxSize: 10,
            roomPlacementIterations: 200,
            pathPlacementIterations: 2000,
            trimCount: 100
            );

        levelParams.MinimumStairsDistance = (levelParams.GridSizeX + levelParams.GridSizeY) / 2 / 2;
        levelParams.MaximumEnemiesPerRoom = 3 + currentFloor / 3;
        levelParams.MinimumEnemiesPerRoom = 0 + currentFloor / 3;
        levelParams.EnemyChance = 50 + currentFloor;
        levelParams.PickupChance = 40 + currentFloor;
        levelParams.PickupHealthChance = GlobalRandom.RandomInt(70, 40);
        levelParams.PickupMutationChance = 100 - levelParams.PickupHealthChance;
        levelParams.ResolveEnemySpawnChances(currentFloor);
        return levelParams;
    }

    private void ResolveEnemySpawnChances(int currentFloor)
    {
        EnemyChances chances = new EnemyChances(currentFloor);
        this.LizardChance = chances.Lizard;
        this.BeeChance = chances.Bee;
        this.SpiderChance = chances.Spider;
        this.AcidSpitterChance = chances.AcidSpitter;
    }
}
