using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public static class LevelGeneration // a very epic, stateful class
{
    private static List<Rectangle> rooms = new List<Rectangle>();
    private static List<Tile> startPaths = new List<Tile>();
    private static List<Tile> stackPaths = new List<Tile>();
    private static int currentRoom = 0;
    private static int currentPath = 0;

    public static Tile[,] CreateLevel(LevelGenerationParameters parameters)
    {
        Tile[,] grid = InitializeGrid(parameters.GridSizeX, parameters.GridSizeY);
        grid = CreateRooms(grid, parameters);
        grid = CreateSpawns(grid, parameters);
        grid = PreparePaths(grid, parameters);
        grid = SetTileNeighbors(grid, parameters);
        grid = GeneratePaths(grid, parameters);
        grid = ConnectRooms(grid, parameters);
        grid = TrimPaths(grid, parameters);

        ResetGenerator();
        return grid;
    }

    private static Tile[,] InitializeGrid(int sizeX, int sizeY)
    {
        Tile[,] grid = new Tile[sizeX, sizeY];
        for (int row = 0; row < sizeY; row++)
        {
            for (int column = 0; column < sizeX; column++)
            {
                Tile currentTile = new Tile();
                currentTile.xLocation = column;
                currentTile.yLocation = row;
                currentTile.torchChance = 8;
                if (column == 0 || column == sizeX - 1 || row == 0 || row == sizeY - 1)
                {
                    currentTile.state = TileState.Block;
                }
                else
                {
                    currentTile.state = TileState.Open;
                }
                grid[row, column] = currentTile;
            }
        }

        return grid;
    }

    private static Tile[,] CreateRooms(Tile[,] grid, LevelGenerationParameters parameters)
    {
        for (int i = 0; i < parameters.RoomPlacementIterations; i++)
        {
            int rSizeX = GlobalRandom.RandomOddInt(parameters.RoomMinSize, parameters.RoomMaxSize);
            int rSizeY = GlobalRandom.RandomOddInt(parameters.RoomMinSize, parameters.RoomMaxSize);

            int rLocX = GlobalRandom.RandomOddInt(1, parameters.GridSizeX - 1 - rSizeX);
            int rLocY = GlobalRandom.RandomOddInt(1, parameters.GridSizeY - 1 - rSizeY);

            Rectangle newRoom = new Rectangle(rLocX, rLocY, rSizeX, rSizeY);
            bool overlaps = false;

            foreach (Rectangle room in rooms)
            {
                if (newRoom.IntersectsWith(room))
                {
                    overlaps = true;
                    break;
                }
            }

            if (overlaps) continue; // room invalid

            rooms.Add(newRoom); // room valid

            StartRoom();
            for (int row = rLocY - 1; row < rLocY + rSizeY + 1; row++)
            {
                for (int column = rLocX - 1; column < rLocX + rSizeX + 1; column++)
                {
                    Tile currentTile = grid[row, column];
                    if (column == rLocX - 1 || column == rLocX + rSizeX || row == rLocY - 1 || row == rLocY + rSizeY)
                    {
                        currentTile.state = TileState.Block; // edge of room
                        currentTile.RoomID = currentRoom;
                    }
                    else
                    {
                        currentTile.state = TileState.Room;
                        currentTile.RoomID = currentRoom;
                    }
                    grid[row, column] = currentTile;
                }
            }
        }

        return grid;
    }

    private static Tile[,] CreateSpawns(Tile[,] grid, LevelGenerationParameters parameters)
    {
        // Assign Player Position
        // Assign Stairs
        // Place Spawns
        int randomIndex = GlobalRandom.RandomInt(rooms.Count - 1, 0);
        Rectangle randomRoom = rooms[randomIndex];
        (int, int) randomPosition = (randomRoom.X + GlobalRandom.RandomInt(randomRoom.Width - 1, 0), randomRoom.Y + GlobalRandom.RandomInt(randomRoom.Height - 1, 0));
        // RC index
        grid[randomPosition.Item2, randomPosition.Item1].playerSpawn = true;

        List<Rectangle> copyRooms = new List<Rectangle>(rooms);
        // Place Stairs
        // remove player spawn from this list
        copyRooms.RemoveAt(randomIndex);
        bool stairsPlaced = false;
        Rectangle stairRoom;
        while (copyRooms.Count > 0)
        {
            Rectangle randomStairRoom = copyRooms[GlobalRandom.RandomInt(copyRooms.Count - 1, 0)];
            (int, int) randomStairPosition = (randomStairRoom.X + GlobalRandom.RandomInt(randomStairRoom.Width - 1, 0), randomStairRoom.Y + GlobalRandom.RandomInt(randomStairRoom.Height - 1, 0));
            if (Math.Sqrt(Math.Pow(randomPosition.Item2 - randomStairPosition.Item1, 2) + Math.Pow(randomPosition.Item1 - randomStairPosition.Item1, 2)) > parameters.MinimumStairsDistance)
            {
                grid[randomStairPosition.Item2, randomStairPosition.Item1].stairsSpawn = true;
                stairsPlaced = true;
                stairRoom = randomStairRoom;
                break;
            }
            else
            {
                copyRooms.Remove(randomStairRoom);
            }
        }
        // if we can't place, give up and put it in same room as player
        while(!stairsPlaced)
        {
            (int, int) randomStairsPosition = (randomRoom.X + GlobalRandom.RandomInt(randomRoom.Width - 1, 0), randomRoom.Y + GlobalRandom.RandomInt(randomRoom.Height - 1, 0));
            if (!grid[randomStairsPosition.Item2, randomStairsPosition.Item1].playerSpawn)
            {
                grid[randomStairsPosition.Item2, randomStairsPosition.Item1].stairsSpawn = true;
                stairRoom = randomRoom;
                stairsPlaced = true;
            }
        }

        // Spawns
        copyRooms = new List<Rectangle>(rooms);
        copyRooms.Remove(randomRoom);
        copyRooms.Remove(stairRoom);
        foreach (Rectangle room in copyRooms)
        {
            int enemiesSpawned = 0;
            for (int i = 0; i < parameters.MaximumEnemiesPerRoom; i++)
            {
                bool validPosition = false;
                Tile currentTile = new Tile();
                while(!validPosition)
                {
                    (int, int) randomLocation = (room.X + GlobalRandom.RandomInt(room.Width - 1, 0), room.Y + GlobalRandom.RandomInt(room.Height - 1, 0));
                    currentTile = grid[randomLocation.Item2, randomLocation.Item1];
                    if (!currentTile.spawnEnemy)
                    {
                        validPosition = true;
                    }
                }
                if (GlobalRandom.RandomInt(100) <= parameters.EnemyChance)
                {
                    currentTile.spawnEnemy = true;
                    enemiesSpawned++;
                    currentTile = DetermineEnemySpawnType(parameters, currentTile);
                }
            }
            // minimum catching

            int pickupsSpawned = 0;
            for (int i = 0; i < parameters.MaximumPickupsPerRoom; i++)
            {
                bool validPosition = false;
                Tile currentTile = new Tile();
                while(!validPosition)
                {
                    (int, int) randomLocation = (room.X + GlobalRandom.RandomInt(room.Width - 1, 0), room.Y + GlobalRandom.RandomInt(room.Height - 1, 0));
                    currentTile = grid[randomLocation.Item2, randomLocation.Item1];
                    if (!currentTile.spawnPickup)
                    {
                        validPosition = true;
                    }
                }
                if (GlobalRandom.RandomInt(100) <= parameters.PickupChance)
                {
                    currentTile.spawnPickup = true;
                    pickupsSpawned++;
                    int pickupType = GlobalRandom.RandomInt(100);
                    if (pickupType <= parameters.PickupHealthChance)
                    {
                        currentTile.pickupState = PickupState.HealthPickup;
                    }
                    else if (pickupType - parameters.PickupHealthChance <= parameters.PickupMutationChance)
                    {
                        currentTile.pickupState = PickupState.MutationPickup;
                    }
                }
            }
        }
        return grid;
    }

    private static Tile DetermineEnemySpawnType(LevelGenerationParameters parameters, Tile currentTile)
    {
        int result = GlobalRandom.RandomInt(100);
        if (result <= parameters.LizardChance)
        {
            currentTile.enemySpawnType = EnemySpawnType.Lizard;
        }
        else if (result - parameters.LizardChance <= parameters.BeeChance)
        {
            currentTile.enemySpawnType = EnemySpawnType.Bee;
        }
        else if (result - parameters.LizardChance - parameters.BeeChance <= parameters.SpiderChance)
        {
            currentTile.enemySpawnType = EnemySpawnType.Spider;
        }
        else if (result - parameters.LizardChance - parameters.BeeChance - parameters.SpiderChance <= parameters.AcidSpitterChance)
        {
            currentTile.enemySpawnType = EnemySpawnType.AcidSpitter;
        }

        return currentTile;
    }

    private static Tile[,] PreparePaths(Tile[,] grid, LevelGenerationParameters parameters)
    {
        for (int row = 0; row < parameters.GridSizeY; row++)
        {
            for (int column = 0; column < parameters.GridSizeX; column++)
            {
                Tile currentTile = grid[row, column];
                if (currentTile.state == TileState.Open && (row % 2 == 0 && column % 2 == 0))
                {
                    currentTile.state = TileState.Block;
                    grid[row, column] = currentTile;
                }
            }
        }

        for (int row = 0; row < parameters.GridSizeY; row++)
        {
            for (int column = 0; column < parameters.GridSizeX; column++)
            {
                Tile currentTile = grid[row, column];
                if (currentTile.state == TileState.Open && (row % 2 == 1 && column % 2 == 1))
                {
                    currentTile.pathCandidate = true;
                    startPaths.Add(currentTile);
                    grid[row, column] = currentTile;
                }
            }
        }
        int randomIndex = GlobalRandom.RandomInt(startPaths.Count - 1, 0);

        startPaths[randomIndex].lastPath = true;
        startPaths[randomIndex].state = TileState.Path;
        startPaths[randomIndex].PathID = currentPath;
        startPaths[randomIndex].pathCandidate = false;
        stackPaths.Insert(0, startPaths[randomIndex]);
        startPaths.RemoveAt(randomIndex);

        return grid;
    }

    private static Tile[,] SetTileNeighbors(Tile[,] grid, LevelGenerationParameters parameters)
    {
        for (int row = 0; row < parameters.GridSizeY; row++)
        {
            for (int column = 0; column < parameters.GridSizeX; column++)
            {
                Tile currentTile = grid[row, column];
                // left, up, right, down: column - 1, row + 1, column + 1, row - 1
                if (column - 1 > -1)
                {
                    currentTile.neighbors.Add(grid[row, column - 1]);
                }
                if (row + 1 < parameters.GridSizeY)
                {
                    currentTile.neighbors.Add(grid[row + 1, column]);
                }
                if (column + 1 < parameters.GridSizeX)
                {
                    currentTile.neighbors.Add(grid[row, column + 1]);
                }
                if (row - 1 > -1)
                {
                    currentTile.neighbors.Add(grid[row - 1, column]);
                }
                grid[row, column] = currentTile;
            }
        }

        return grid;
    }

    private static Tile[,] GeneratePaths(Tile[,] grid, LevelGenerationParameters parameters)
    {
        bool restart = false;
        bool useOther = false;

        for (int iterations = 0; iterations < parameters.PathPlacementIterations; iterations++)
        {
            // Path Generation
            for (int row = 0; row < parameters.GridSizeY; row++)
            {
                for (int column = 0; column < parameters.GridSizeX; column++)
                {
                    List<Tile> validPaths = new List<Tile>();
                    Tile currentTile = grid[row, column];
                    if (!currentTile.lastPath) continue;
                    foreach (Tile neighbor in currentTile.neighbors)
                    {
                        if (neighbor.state == TileState.Open)
                        {
                            int pathCount = 0;
                            foreach (Tile neighborNeighbor in neighbor.neighbors)
                            {
                                pathCount += neighborNeighbor.state == TileState.Path ? 1 : 0;
                            }
                            if (pathCount < 2)
                            {
                                validPaths.Add(neighbor);
                            }
                        }
                    }
                    if (validPaths.Count > 0)
                    {
                        Tile choice = validPaths[GlobalRandom.RandomInt(validPaths.Count - 1, min: 0)];

                        choice.state = TileState.Path;
                        choice.lastPath = true;
                        choice.PathID = currentPath;
                        stackPaths.Insert(0, choice);

                        if (choice.pathCandidate)
                        {
                            choice.pathCandidate = false;
                            startPaths.Remove(choice);
                        }
                    }
                    else
                    {
                        restart = true;
                    }

                    currentTile.lastPath = false;
                    grid[row, column] = currentTile;
                }
            }

            // Restart From Stack
            if (restart)
            {
               List <Tile> copyStack = stackPaths.GetRange(0, stackPaths.Count);
                bool lastPathCreated = false;

                for (int j = 0; j < stackPaths.Count; j++)
                {
                    Tile tile = stackPaths[j];
                    if (!lastPathCreated)
                    {
                        for (int i = 0; i < tile.neighbors.Count; i++)
                        {
                            Tile neigh = tile.neighbors[i];
                            if (neigh.state == TileState.Open)
                            {
                                int pathCount = 0;
                                foreach (Tile neighNeigh in neigh.neighbors)
                                {
                                    pathCount += neighNeigh.state == TileState.Path ? 1 : 0;
                                }
                                if (pathCount < 2)
                                {
                                    neigh.state = TileState.Path;
                                    neigh.lastPath = true;
                                    neigh.PathID = currentPath;
                                    
                                    lastPathCreated = true;
                                    restart = false;
                                    break;
                                }
                                else
                                {
                                    neigh.state = TileState.Block;
                                }
                                tile.neighbors[i] = neigh;
                            }
                        }

                        if (!lastPathCreated)
                        {
                            copyStack.Remove(tile);
                        }
                    }
                    stackPaths[j] = tile;

                }

                stackPaths = copyStack;

                if (!lastPathCreated)
                {
                    useOther = true;
                    restart = false;
                    currentPath++;
                }
            }

            // Flip to Other Path
            if (useOther)
            {
                if (startPaths.Count > 0)
                {
                    int randomIndex = GlobalRandom.RandomInt(startPaths.Count - 1, 0);
                    Tile randomTile = startPaths[randomIndex];
                    randomTile.state = TileState.Path;
                    randomTile.lastPath = true;
                    randomTile.PathID = currentPath;
                    randomTile.pathCandidate = false;
                    stackPaths.Insert(0, randomTile);

                    startPaths.RemoveAt(randomIndex);
                    restart = false;
                    useOther = false;
                }
                else
                {
                    // we're out of paths to start from
                    break;
                }
            }
        }

        return grid;
    }

    private static Tile[,] ConnectRooms(Tile[,] grid, LevelGenerationParameters parameters)
    {
        // Setup
        List<Tile> pickedTiles = new List<Tile>();
        List<int> pickedRooms = new List<int>();
        List<int> pickedPaths = new List<int>();
        List<int> roomIDs = new List<int>();
        List<int> pathIDs = new List<int>();
        for (int i = 1; i <= currentRoom; i++)
        {
            roomIDs.Add(i);
        }
        for (int i = 1; i < currentPath; i++)
        {
            pathIDs.Add(i);
        }

        // Candidate Door Creation
        for (int iterations = 0; iterations < 10000; iterations++)
        {
            int candidateCount = 0;
            for (int row = 0; row < parameters.GridSizeY; row++)
            {
                for (int column = 0; column < parameters.GridSizeX; column++)
                {
                    Tile currentTile = grid[row, column];
                    if (currentTile.state != TileState.Block) continue;
                    int roomCount = 0;
                    int pathCount = 0;
                    List<int> rooms = new List<int>();
                    List<int> paths = new List<int>();
                    foreach (Tile neighbor in currentTile.neighbors)
                    {
                        if (neighbor.state == TileState.Block) continue;
                        bool isRoom = neighbor.state == TileState.Room;
                        bool isPath = neighbor.state == TileState.Path;
                        roomCount += isRoom ? 1 : 0;
                        pathCount += isPath ? 1 : 0;

                        if (isRoom)
                        {
                            int roomId = neighbor.RoomID;
                            if (rooms.Contains(roomId))
                            {
                                roomCount -= 1;
                            }
                            else
                            {
                                rooms.Add(roomId);
                            }
                        }
                        if (isPath)
                        {
                            int pathId = neighbor.PathID;
                            paths.Add(pathId);
                        }
                    }

                    if (roomCount == 2 || (roomCount == 1 && pathCount == 1))
                    {
                        currentTile.connectCandidate = true;
                        candidateCount++;
                        currentTile.connectPathIds = paths;
                        currentTile.connectRoomIds = rooms;
                    }
                    grid[row, column] = currentTile;
                }
            }

            // Random Room ID
            int pickedRoomId = -1;
            int pickedPathId = -1;

            if (roomIDs.Count == 0)
            {
                pickedPathId = pathIDs[GlobalRandom.RandomInt(roomIDs.Count - 1, 0)];
                for (int row = 0; row < parameters.GridSizeY; row++)
                {
                    for (int column = 0; column < parameters.GridSizeX; column++)
                    {
                        Tile currentTile = grid[row, column];
                        if (currentTile.connectCandidate && currentTile.connectPathIds.Contains(pickedPathId))
                        {
                            pickedTiles.Add(currentTile);
                        }
                    }
                }
            }
            else
            {
                pickedRoomId = roomIDs[GlobalRandom.RandomInt(roomIDs.Count - 1, 0)];
                for (int row = 0; row < parameters.GridSizeY; row++)
                {
                    for (int column = 0; column < parameters.GridSizeX; column++)
                    {
                        Tile currentTile = grid[row, column];
                        if (currentTile.connectCandidate && currentTile.connectRoomIds.Contains(pickedRoomId))
                        {
                            pickedTiles.Add(currentTile);
                        }
                    }
                }
            }
            // PickedTiles Update



            // Pick Connector
            Tile pickedTile = null;
            if (pickedTiles.Count > 0)
            {
                pickedTile = pickedTiles[GlobalRandom.RandomInt(pickedTiles.Count - 1, 0)];
                pickedTile.pickedTile = true;
                pickedTile.state = TileState.Door;
                pickedRooms = pickedTile.connectRoomIds;
                pickedPaths = pickedTile.connectPathIds;

                //pickedTile = pickedTiles[GlobalRandom.RandomInt(pickedTiles.Count - 1, 0)];
                //pickedTile.pickedTile = true;
                //pickedTile.state = TileState.Door;
                //pickedRooms = pickedTile.connectRoomIds;
                //pickedPaths = pickedTile.connectPathIds;
                pickedTiles.Clear();
            }

            // Swap Room Out
            for (int row = 0; row < parameters.GridSizeY; row++)
            {
                for (int column = 0; column < parameters.GridSizeX; column++)
                {
                    Tile currentTile = grid[row, column];
                    bool isRoom = currentTile.state == TileState.Room;
                    bool isPath = currentTile.state == TileState.Path;
                    bool isDoor = currentTile.state == TileState.Door;
                    if (!(isRoom || isPath || isDoor)) continue;
                    if (isPath)
                    {
                        foreach (int ppi in pickedPaths)
                        {
                            if (currentTile.PathID == ppi)
                            {
                                currentTile.state = TileState.Room;
                                currentTile.PathID = -1;
                                currentTile.RoomID = pickedTile.RoomID;
                            }
                        }
                    }
                    if (isRoom)
                    {
                        foreach (int roomId in pickedRooms)
                        {
                            if (currentTile.RoomID == roomId)
                            {
                                currentTile.RoomID = pickedTile.RoomID;
                            }
                        }
                    }
                    if (isDoor)
                    {
                        currentTile.RoomID = pickedTile.RoomID;
                        currentTile.connectCandidate = false;
                        currentTile.state = TileState.Room; // TODO: if doors are desired, this should be reversed
                    }
                    grid[row, column] = currentTile;
                }
            }

            // Reset
            foreach (int pId in pickedRooms)
            {
                if (pId != pickedRoomId)
                {
                    // roomIDs.Remove(pId);
                }
            }

            foreach (int pId in pickedPaths)
            {
                // pathIDs.Remove(pId);
            }
            pickedRooms.Clear();
            pickedPaths.Clear();
            pickedRoomId = 0;

            // Reset Candidates
            for (int row = 0; row < parameters.GridSizeY; row++)
            {
                for (int column = 0; column < parameters.GridSizeX; column++)
                {
                    Tile currentTile = grid[row, column];
                    currentTile.connectCandidate = false;
                    grid[row, column] = currentTile;
                }
            }

            // Check exit condition
            if (roomIDs.Count == 1 && pathIDs.Count == 0)
            {
                break;
            }
        }

        return grid;
    }

    private static Tile[,] TrimPaths(Tile[,] grid, LevelGenerationParameters parameters)
    {
        for (int iterations = 0; iterations < parameters.TrimCount; iterations++)
        {
            for (int row = 0; row < parameters.GridSizeY; row++)
            {
                for (int column = 0; column < parameters.GridSizeX; column++)
                {
                    int roomCount = 0;
                    Tile currentTile = grid[row, column];
                    foreach (Tile neigh in currentTile.neighbors)
                    {
                        roomCount += neigh.state == TileState.Room ? 1 : 0;
                    }

                    if (roomCount == 1)
                    {
                        currentTile.state = TileState.Block;
                    }

                    grid[row, column] = currentTile;
                }
            }
        }

        for (int row = 0; row < parameters.GridSizeY; row++)
        {
            for (int column = 0; column < parameters.GridSizeX; column++)
            {
                Tile currentTile = grid[row, column];
                if (currentTile.state == TileState.Path)
                {
                    currentTile.state = TileState.Block;
                }
            }
        }

        return grid;
    }

    private static void StartRoom()
    {
        currentRoom++;
    }

    private static void ResetGenerator()
    {
        rooms = new List<Rectangle>();
        startPaths = new List<Tile>();
        stackPaths = new List<Tile>();
        currentRoom = 0;
        currentPath = 0;
    }
}
