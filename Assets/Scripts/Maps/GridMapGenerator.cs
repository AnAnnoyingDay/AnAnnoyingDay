using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GridMapGenerator
{
    private int nbBox = 10;
    private int gridWidth = 30;
    private int gridHeight = 30;

    private BoxController[,] Grid;
    private Vector2Int bossBox;
    private Vector2Int keyBox;

    public BoxController[] finalGrid;
    private int indexBoss;
    private int indexKey;

    // - Using Vector2Int                           : OK 
    // - Create Boss Room while creating the field  : OK
    // - Add Exists array in all rooms              : OK
    // - Delete DistFromStart                       : OK
    // - Make a small array after filling the grid  : OK
    // - Set the key room                           : OK
    // - Comment all code :)                        : OK
    // - Err array[10] not create cause : emergency : OK
    // - Err Start + Key Room in the same           : OK

    // Use this for initialization
    public GridMapGenerator(int nbBox)
    {
        this.nbBox = nbBox;
        this.CreateGrid();
    }

    public void CreateGrid()
    {
        InitGrid();
        FillGrid();
        SmallenGrid();
        SetKeyRoom();
        //DisplayFinalGrid();
    }

    // Set the start Box and create all Box
    public void InitGrid()
    {
        // Create all Box on field
        Grid = new BoxController[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Grid[x, y] = new BoxController
                {
                    X = x,
                    Y = y,
                    Exits = new Vector2Int[4]
                };
                Grid[x, y].Exits[0] = new Vector2Int(-1, -1);
                Grid[x, y].Exits[1] = new Vector2Int(-1, -1);
                Grid[x, y].Exits[2] = new Vector2Int(-1, -1);
                Grid[x, y].Exits[3] = new Vector2Int(-1, -1);
            }
        }

        // Init the started Box
        System.Random rand = new System.Random();
        Vector2Int start = new Vector2Int((int) this.gridWidth / 2, (int) this.gridHeight / 2);
        bossBox = start;
        Grid[start.x, start.y].X = start.x;
        Grid[start.x, start.y].Y = start.y;
        Grid[start.x, start.y].ActionOrder = 0;
        Grid[start.x, start.y].State = "Busy";
        Grid[start.x, start.y].IsBoss = true;
    }

    // Fill the whole array of Box
    public void FillGrid()
    {
        System.Random rand = new System.Random();
        int nbBoxPlaced = 1;
        int currentActionOrder = 0;
        int boxAddedThisTurn = 0;
        Vector2Int emergency = new Vector2Int(0, 0);
        int emergencyExit = 0;
        while (nbBoxPlaced < nbBox)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    if (Grid[x, y].ActionOrder == currentActionOrder && Grid[x, y].State == "Busy")
                    {
                        int toCreate = rand.Next(0, 2);
                        Vector2Int source = new Vector2Int(x, y);
                        for (int indexExit = 0; indexExit < 4; indexExit++)
                        {
                            Vector2Int target = GetCoordFromExit(new Vector2Int(x, y), indexExit);
                            if (target.x != -1 && target.y != -1 && CanCreateAt(target))
                            {
                                if (toCreate == 1)
                                {
                                    GenerateBox(source, target);
                                    boxAddedThisTurn++;
                                    nbBoxPlaced++;
                                }
                                else
                                {
                                    emergency.x = x;
                                    emergency.y = y;
                                    emergencyExit = indexExit;
                                }
                            }
                            toCreate = rand.Next(0, 2);
                            if (nbBoxPlaced == nbBox)
                                break;
                        }
                        // Avoid the start box to be the key box
                        if (currentActionOrder == 0 && nbBoxPlaced < 3)
                        {
                            if (nbBoxPlaced == 1)
                            {
                                Debug.Log("1st turn 0 created");
                                Vector2Int coordTarget2 = GetCoordFromExit(emergency, 0);
                                GenerateBox(source, coordTarget2);
                                boxAddedThisTurn++;
                                nbBoxPlaced++;
                            }
                            Debug.Log("1st turn");
                            Vector2Int coordTarget = GetCoordFromExit(emergency, emergencyExit);
                            GenerateBox(source, coordTarget);
                            boxAddedThisTurn++;
                            nbBoxPlaced++;
                        }
                    }
                    if (nbBoxPlaced == nbBox)
                        break;

                }
                if (nbBoxPlaced == nbBox)
                    break;
            }
            if (boxAddedThisTurn == 0)
            {
                Vector2Int target = GetCoordFromExit(emergency, emergencyExit);
                GenerateBox(new Vector2Int(emergency.x, emergency.y), target);
                nbBoxPlaced++;
            }
            currentActionOrder++;
            boxAddedThisTurn = 0;
        }
    }

    // Return true if a Box can be crated at this place
    public bool CanCreateAt(Vector2Int pos)
    {
        // Check if there is less than two box next to the place we wanted to create a new box and if the box is empty
        if (Grid[pos.x, pos.y].State != "Empty")
        {
            return false;
        }
        else
        {
            int closeBoxTaken = 0;
            if ((pos.y + 1) < gridHeight)
                if (Grid[pos.x, pos.y + 1].State == "Busy")
                    closeBoxTaken++;
            if ((pos.y - 1) >= 0)
                if (Grid[pos.x, pos.y - 1].State == "Busy")
                    closeBoxTaken++;
            if ((pos.x + 1) < gridWidth)
                if (Grid[pos.x + 1, pos.y].State == "Busy")
                    closeBoxTaken++;
            if ((pos.x - 1) >= 0)
                if (Grid[pos.x - 1, pos.y].State == "Busy")
                    closeBoxTaken++;
            if (closeBoxTaken > 1)
            {
                Grid[pos.x, pos.y].State = "DontTouch";
                return false;
            }
        }
        return true;
    }

    // Return the coord from an initial position and a direction
    public Vector2Int GetCoordFromExit(Vector2Int pos, int exit)
    {
        Vector2Int _return = new Vector2Int(pos.x, pos.y);
        if (exit == 0)
            _return.y = (pos.y == gridHeight - 1) ? -1 : pos.y + 1;
        else if (exit == 1)
            _return.x = (pos.x == gridWidth - 1) ? -1 : pos.x + 1;
        else if (exit == 2)
            _return.y = (pos.y == 0) ? -1 : pos.y - 1;
        else if (exit == 3)
            _return.x = (pos.x == 0) ? -1 : pos.x - 1;
        return _return;
    }

    // Generate the next Box
    public void GenerateBox(Vector2Int source, Vector2Int target)
    {
        Grid[target.x, target.y].State = "Busy";
        Grid[target.x, target.y].ActionOrder = Grid[source.x, source.y].ActionOrder + 1;
        // Update all the Exits
        if (target.y > source.y)
        {
            Grid[target.x, target.y].Exits[2] = source;
            Grid[source.x, source.y].Exits[0] = target;
        }
        else if (target.y < source.y)
        {
            Grid[target.x, target.y].Exits[0] = source;
            Grid[source.x, source.y].Exits[2] = target;
        }
        else if (target.x > source.x)
        {
            Grid[target.x, target.y].Exits[3] = source;
            Grid[source.x, source.y].Exits[1] = target;
        }
        else if (target.x < source.x)
        {
            Grid[target.x, target.y].Exits[1] = source;
            Grid[source.x, source.y].Exits[3] = target;
        }

        // Update the Boss room if needed
        if (Grid[target.x, target.y].ActionOrder > Grid[bossBox.x, bossBox.y].ActionOrder)
        {
            Grid[bossBox.x, bossBox.y].IsBoss = false;
            Grid[target.x, target.y].IsBoss = true;
            bossBox = target;
        }
    }

    // Delete all useless Box in Grid
    public void SmallenGrid()
    {
        finalGrid = new BoxController[nbBox];
        int currentIndex = 0;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (Grid[x, y].State == "Busy")
                {
                    finalGrid[currentIndex] = new BoxController();
                    finalGrid[currentIndex] = Grid[x, y];
                    if (Grid[x, y].IsBoss)
                    {
                        indexBoss = currentIndex;
                        finalGrid[currentIndex].DistFromBoss = 0;
                    }
                    currentIndex++;
                }
            }
        }
    }

    // Set the farest box from Boss as Key Box
    public void SetKeyRoom()
    {
        indexKey = indexBoss;
        bool allBoxAreDone = false;
        // Fill distance from Boss room for each room
        while (!allBoxAreDone)
        {
            for (int i = 0; i < finalGrid.Length; i++)
            {
                if (finalGrid[i].DistFromBoss == -1)
                {
                    for (int x = 0; x < finalGrid[i].Exits.Length; x++)
                    {
                        int tmp = SearchIndexByCoord(new Vector2Int(finalGrid[i].Exits[x].x, finalGrid[i].Exits[x].y));
                        if (tmp != -1 && finalGrid[tmp].DistFromBoss != -1)
                            finalGrid[i].DistFromBoss = finalGrid[tmp].DistFromBoss + 1;
                    }
                }
            }
            allBoxAreDone = true;
            for (int i = 0; i < finalGrid.Length; i++)
            {
                if (finalGrid[i].DistFromBoss == -1)
                    allBoxAreDone = false;
            }
        }

        // Set the new Key room
        for (int i = 0; i < finalGrid.Length; i++)
        {
            if (finalGrid[i].DistFromBoss > finalGrid[indexKey].DistFromBoss)
            {
                finalGrid[indexKey].IsKey = false;
                finalGrid[i].IsKey = true;
                indexKey = i;
            }
        }
    }

    // Return index of the room which has the coord given
    public int SearchIndexByCoord(Vector2Int coord)
    {
        for (int i = 0; i < finalGrid.Length; i++)
        {
            if (finalGrid[i].X == coord.x && finalGrid[i].Y == coord.y)
                return i;
        }
        return -1;
    }

    // Display the grid Box
    public void DisplayGrid()
    {
        Debug.Log("=== FINISHED ===");
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (Grid[x, y].State == "Busy")
                    if (Grid[x, y].IsBoss)
                        Debug.Log("Grid[" + x + "," + y + "] : top(" + Grid[x, y].Exits[0].x + ", " + Grid[x, y].Exits[0].y + ") right(" + Grid[x, y].Exits[1].x + ", " + Grid[x, y].Exits[1].y + ") bot(" + Grid[x, y].Exits[2].x + ", " + Grid[x, y].Exits[2].y + ") left(" + Grid[x, y].Exits[3].x + ", " + Grid[x, y].Exits[3].y + ") + Boss");
                    else if (Grid[x, y].IsKey)
                        Debug.Log("Grid[" + x + "," + y + "] : top(" + Grid[x, y].Exits[0].x + ", " + Grid[x, y].Exits[0].y + ") right(" + Grid[x, y].Exits[1].x + ", " + Grid[x, y].Exits[1].y + ") bot(" + Grid[x, y].Exits[2].x + ", " + Grid[x, y].Exits[2].y + ") left(" + Grid[x, y].Exits[3].x + ", " + Grid[x, y].Exits[3].y + ") + Key");
                    else if (Grid[x, y].ActionOrder == 0)
                        Debug.Log("Grid[" + x + "," + y + "] : top(" + Grid[x, y].Exits[0].x + ", " + Grid[x, y].Exits[0].y + ") right(" + Grid[x, y].Exits[1].x + ", " + Grid[x, y].Exits[1].y + ") bot(" + Grid[x, y].Exits[2].x + ", " + Grid[x, y].Exits[2].y + ") left(" + Grid[x, y].Exits[3].x + ", " + Grid[x, y].Exits[3].y + ") + Start");
                    else
                        Debug.Log("Grid[" + x + "," + y + "] : top(" + Grid[x, y].Exits[0].x + "," + Grid[x, y].Exits[0].y + ") right(" + Grid[x, y].Exits[1].x + "," + Grid[x, y].Exits[1].y + ") bot(" + Grid[x, y].Exits[2].x + "," + Grid[x, y].Exits[2].y + ") left(" + Grid[x, y].Exits[3].x + "," + Grid[x, y].Exits[3].y + ")");
            }
        }
    }

    // Display the grid Box
    public void DisplayFinalGrid()
    {
        Debug.Log("=== MAP ===");

        for (int x = 0; x < finalGrid.Length; x++)
        {
            String display = "Grid[" + finalGrid[x].X + "," + finalGrid[x].Y + "]"; 
            if (finalGrid[x].IsBoss)
                Debug.Log(display + "Boss");
            else if (finalGrid[x].IsKey)
                Debug.Log(display + "Key");
            else if (finalGrid[x].ActionOrder == 0)
                Debug.Log(display + "Start");
            else
                Debug.Log(display);
        }

        Debug.Log("=== END MAP ===");
    }
}
