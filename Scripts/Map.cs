using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

using Random = UnityEngine.Random;

public enum TileType
{
    Grass = 0,
    WallStraight_1 = 1,
    WallStraight_2 = 2,
    Corner_1 = 3,
    Corner_2 = 4,
    Corner_3 = 5,
    Corner_4 = 6,
    Corner_inner_1 = 7,
    Corner_inner_2 = 8,
    Corner_inner_3 = 9,
    Corner_inner_4 = 10,

    Floor_near_corner = 13,
    Floor_no_wall_neighbours = 14,
    Floor = 15,
    Floor_reserved = 16,


    WallEndLeft = 18,
    WallEndRight = 19,
    WallEndUp = 20,
    WallEndDown = 21,
    WallEndFull = 22,
    Window = 23,

    Door = 25,
    Kitchen_sink = 26,
    Kitchen_stove = 27,
    Kitchen_worktop = 28,
    Kitchen_table = 29,
    Kitchen_chair = 30

}

public class Map
{
    public int rows = 0;
    public int cols = 0;
    public Tile[] tiles;
    public Tile[] childTiles;
    public int endRow = 4;


    private int[][] presets = new int[2][];


    public Tile[] wallTiles_1
    {
        get
        {
            //find all the tiles that are not grass. At this stage our tile map only contains tiles with id 0-15
            return tiles.Where(t => (t.autotileID == (((int)TileType.WallStraight_1)))).ToArray();
            //after it has run we also have -1 as the ID
        }
    }

    public Tile[] wallTiles_2
    {
        get
        {
            //find all the tiles that are not grass. At this stage our tile map only contains tiles with id 0-15
            return tiles.Where(t => (t.autotileID == (((int)TileType.WallStraight_2)))).ToArray();
            //after it has run we also have -1 as the ID
        }
    }

    public Tile[] wallTiles_all
    {
        get
        {
            return tiles.Where(t => (t.autotileID == (((int)TileType.WallStraight_2)) | t.autotileID == (((int)TileType.WallStraight_1)))).ToArray();
        }
    }

    public Tile[] floorNearWall
    {
        get
        {
            return tiles.Where(t => (t.autotileID == ((int)TileType.Floor))).ToArray();
        }
    }

    public Tile[] floorNoWallNeighbours
    {
        get
        {
            return tiles.Where(t => (t.autotileID == ((int)TileType.Floor_no_wall_neighbours))).ToArray();
        }
    }

    public Tile[] kitchenTiles
    {
        get
        {
            return tiles.Where(t => (t.autotileID == ((int)TileType.Kitchen_sink) | t.autotileID == ((int)TileType.Kitchen_stove) | t.autotileID == ((int)TileType.Kitchen_worktop))).ToArray();
        }
    }

    //public Tile[] landTiles
    //{
    //    get
    //    {
    //        //now we retrieve only grass tiles
    //        return tiles.Where(t => t.autotileID == (int)TileType.Wall).ToArray();
    //    }
    //}

    //public Tile castleTile
    //{
    //    get
    //    {
    //        return tiles.FirstOrDefault(t => t.autotileID == (int)TileType.Castle);
    //    }
    //}

    //public Tile[] singleIslands
    //{
    //    get
    //    {
    //        return tiles.Where(t => t.autotileID == 0).ToArray();
    //    }
    //}

    public void NewMap(int rows, int cols)
    {
        this.rows = cols;
        this.cols = rows;

        //prepare our array by giving it enough spaces to store every node
        tiles = new Tile[rows * cols];
        childTiles = new Tile[rows * cols];

        //loop through the spaces in the array and create empty nodes

        CreateTiles();
    }

    public void CreateIsland(
        int doorNumber,
        int erodeIterations,
        float treePercent,
        float hillPercent,
        float mountainPercent,
        float townPercent,
        float monsterPercent,
        float lakePercent
        )
    {
        //DecorateTiles(landTiles, lakePercent, TileType.Water);
        //for (int i = 0; i < erodeIterations; i++)
        //{
        //    DecorateTiles(wallTiles, erodePercent, TileType.Water);
        //}
        //var openTiles = landTiles;
        //RandomizeTileArray(openTiles);
        //openTiles[0].autotileID = (int)TileType.Castle;
        ////we take all the grass tiles and turn some into trees
        //DecorateTiles(landTiles, treePercent, TileType.Tree);
        ////we take what is left of grass tiles and turn some into hills 
        //DecorateTiles(landTiles, hillPercent, TileType.Hills);
        ////we tak again what is left of grass tiles and turn some into mountains
        //DecorateTiles(landTiles, mountainPercent, TileType.Mountains);
        //DecorateTiles(landTiles, townPercent, TileType.Towns);
        //DecorateTiles(landTiles, monsterPercent, TileType.Monster);
        //DecorateTiles(singleIslands, 1, TileType.Water);

    }



    private void CreateTiles()
    {
        var total = tiles.Length;

        for (int i = 0; i < total; i++)
        {
            // create a new node
            Tile tile = new Tile();
            //assign its label to its number
            tile.id = i;
            //put it inside the array
            tiles[i] = tile;

            //prepare decorative tiles array
            Tile childTile = new Tile();
            childTile.id = i;
            childTile.autotileID = -1;
            childTiles[i] = childTile;

        }

        FindNeighbours8();
    }

    private void FindNeighbours()
    {
        //Indexing the tiles - to create a correct 2d space from them

        // iterating through each row
        for (int r = 0; r < rows; r++)
        {
            //iterate through each column in this row
            for (int c = 0; c < cols; c++)
            {
                // get the node from our array storage
                Tile tile = tiles[cols * r + c];

                // Top
                if (r > 0)
                {
                    tile.AddNeighbours(Sides.Top, tiles[cols * (r - 1) + c]);
                }

                // Right
                if (c < cols - 1)
                {
                    tile.AddNeighbours(Sides.Right, tiles[cols * r + c + 1]);
                }

                // Down
                if (r < rows - 1)
                {
                    tile.AddNeighbours(Sides.Bottom, tiles[cols * (r + 1) + c]);
                }

                // Left
                if (c > 0)
                {
                    tile.AddNeighbours(Sides.Left, tiles[cols * r + c - 1]);
                }

            }
        }
    }

    private void FindNeighbours8()
    {
        //Indexing the tiles - to create a correct 2d space from them

        // iterating through each row
        for (int r = 0; r < rows; r++)
        {
            //iterate through each column in this row
            for (int c = 0; c < cols; c++)
            {
                // get the node from our array storage
                Tile tile = tiles[cols * r + c];

                // Top
                if (r > 0)
                {
                    //top left
                    if (c - 1 >= 0)
                    {
                        tile.AddNeighbours(Sides.Top_Left, tiles[cols * (r - 1) + (c - 1)]);
                    }
                    //center
                    tile.AddNeighbours(Sides.Top, tiles[cols * (r - 1) + c]);

                    //top right
                    if ((c + 1) < cols)
                    {
                        tile.AddNeighbours(Sides.Top_Right, tiles[cols * (r - 1) + (c + 1)]);
                    }
                }

                // Right
                if (c < cols - 1)
                {
                    tile.AddNeighbours(Sides.Right, tiles[cols * r + c + 1]);
                }

                // Down
                if (r < rows - 1)
                {
                    //left
                    if (c - 1 >= 0)
                    {
                        tile.AddNeighbours(Sides.Bottm_Left, tiles[cols * (r + 1) + (c - 1)]);
                    }
                    //center
                    tile.AddNeighbours(Sides.Bottom, tiles[cols * (r + 1) + c]);

                    //right
                    if ((c + 1) < cols)
                    {
                        tile.AddNeighbours(Sides.Bottom_Right, tiles[cols * (r + 1) + (c + 1)]);
                    }
                }

                // Left
                if (c > 0)
                {
                    tile.AddNeighbours(Sides.Left, tiles[cols * r + c - 1]);
                }

            }
        }
    }

    /// <summary>
    /// Adds decorative elements to the house (besides walls and floors)
    /// </summary>
    /// <param name="doorCount"></param>
    public void CompleteTheHouse(int doorCount, float percentOfWindows)
    {
        //add doors
        CreateDoor(doorCount);
        CreateWindows(percentOfWindows);
        CreateKitchen();
        CreateTableArea();
    }

    private void CreateKitchen()
    {
        Tile[] kitchen = floorNearWall;
        RandomizeTileArray(kitchen);
        foreach(var tile in kitchen)
        {
            //check if we can find 3 consecutive floor tiles (in either vertical or horizontal direction), each adjacent to a wall
            if (tile.neighbours[1]!=null && tile.neighbours[2] != null && tile.neighbours[1].autotileID == (int)TileType.Floor && tile.neighbours[2].autotileID == (int)TileType.Floor)
            {
                tile.neighbours[2].autotileID = (int)TileType.Floor_reserved;
                tile.neighbours[1].autotileID = (int)TileType.Floor_reserved;
                tile.autotileID = (int)TileType.Floor_reserved;
                if (tile.neighbours[0] != null && tile.neighbours[0].autotileID != (int)TileType.Floor_no_wall_neighbours)
                {
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[2].id] }, 1, TileType.Kitchen_sink, false, 0);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.id] }, 1, TileType.Kitchen_stove, false, 0);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[1].id] }, 1, TileType.Kitchen_worktop, false, 0);
                }
                else
                {
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[1].id] }, 1, TileType.Kitchen_sink, true, 180);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.id] }, 1, TileType.Kitchen_stove, true, 180);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[2].id] }, 1, TileType.Kitchen_worktop, true, 180);
                }
                return;
            }else if (tile.neighbours[0] != null && tile.neighbours[3] != null && tile.neighbours[0].autotileID == (int)TileType.Floor && tile.neighbours[3].autotileID == (int)TileType.Floor)
            {
                tile.neighbours[3].autotileID = (int)TileType.Floor_reserved;
                tile.neighbours[0].autotileID = (int)TileType.Floor_reserved;
                tile.autotileID = (int)TileType.Floor_reserved;
                if (tile.neighbours[1] != null && tile.neighbours[1].autotileID != (int)TileType.Floor_no_wall_neighbours)
                {
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[0].id] }, 1, TileType.Kitchen_sink, true, 90);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.id] }, 1, TileType.Kitchen_stove, true, 90);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[3].id] }, 1, TileType.Kitchen_worktop, true, 90);
                }
                else
                {
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[3].id] }, 1, TileType.Kitchen_sink, true, -90);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.id] }, 1, TileType.Kitchen_stove, true, -90);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[0].id] }, 1, TileType.Kitchen_worktop, true, -90);
                }
                return;
            }
        }
    }

    private void CreateTableArea()
    {
        Tile[] tableArea = floorNoWallNeighbours;
        RandomizeTileArray(tableArea);
        foreach (var tile in tableArea)
        {
            //check if we can find 3 consecutive floor tiles (in either vertical or horizontal direction), each adjacent to a wall
            if (tile.neighbours[1] != null && tile.neighbours[2] != null && tile.neighbours[1].autotileID == (int)TileType.Floor_no_wall_neighbours && tile.neighbours[2].autotileID == (int)TileType.Floor_no_wall_neighbours)
            {
                tile.neighbours[2].autotileID = (int)TileType.Floor_reserved;
                tile.neighbours[1].autotileID = (int)TileType.Floor_reserved;
                tile.autotileID = (int)TileType.Floor_reserved;

                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[2].id] }, 1, TileType.Kitchen_chair, true, -90);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.id] }, 1, TileType.Kitchen_table, false, 0);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[1].id] }, 1, TileType.Kitchen_chair, true, 90);
                

                return;
            }
            else if (tile.neighbours[0] != null && tile.neighbours[3] != null && tile.neighbours[0].autotileID == (int)TileType.Floor_no_wall_neighbours && tile.neighbours[3].autotileID == (int)TileType.Floor_no_wall_neighbours)
            {
                tile.neighbours[3].autotileID = (int)TileType.Floor_reserved;
                tile.neighbours[0].autotileID = (int)TileType.Floor_reserved;
                tile.autotileID = (int)TileType.Floor_reserved;

                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[0].id] }, 1, TileType.Kitchen_chair, false, 0);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.id] }, 1, TileType.Kitchen_table, true, 90);
                    DecorateNumberOfTiles(new Tile[] { childTiles[tile.neighbours[3].id] }, 1, TileType.Kitchen_chair, true, 180);
                
                return;
            }
        }
    }

    /// <summary>
    /// Creates door in wall tiles. Makes changes to the neighbours to look more natural. 
    /// </summary>
    /// <param name="grid"></param>
    private void CreateDoor(int numberOfInstances)
    {
        Tile[] doors;
        if (Random.Range(0, 100) > 50)
        {
            doors = DecorateNumberOfInnerTiles(wallTiles_1, numberOfInstances, TileType.Floor_reserved, TileType.Door, true, 90);

        }
        else
        {
            doors = DecorateNumberOfInnerTiles(wallTiles_2, numberOfInstances, TileType.Floor_reserved, TileType.Door, false, 0);
        }
        //create cornertile where door opens
        for (int i = 0; i < doors.Length; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                Tile neighbour = doors[i].neighbours[k];
                if (neighbour != null && neighbour.autotileID == (int)TileType.Floor)
                {
                    neighbour.autotileID = (int)TileType.Floor_reserved;
                }
            }
        }
        
        CloseWallsAroundTiles(doors);
    }

    private void CreateWindows(float percent)
    {
        float clampPercent = Mathf.Clamp(percent, 0.1f, 0.4f);
        int numberOfInstances = (int)(wallTiles_all.Length * clampPercent);
        if (numberOfInstances < 2)
        {
            numberOfInstances = 2;
        }
        Tile[] windows;
        Tile[] windows_1 = DecorateNumberOfInnerTiles(wallTiles_1, numberOfInstances/2, TileType.Floor_reserved, TileType.Window, true, 90);
        Tile[] windows_2 = DecorateNumberOfInnerTiles(wallTiles_2, numberOfInstances / 2, TileType.Floor_reserved, TileType.Window, false, 0);
        windows = windows_1.Concat(windows_2).ToArray();
        CloseWallsAroundTiles(windows);
    }

    /// <summary>
    /// Takes an array of Tile[] and if it is surrounded by walls it closes the walls to make the new tile look more natural.
    /// </summary>
    /// <param name="tilesToCheck"></param>
    private void CloseWallsAroundTiles(Tile[] tilesToCheck)
    {
        if (tilesToCheck.Length > 0)
        {
            for (int i = 0; i < tilesToCheck.Length; i++)
            {

                // after closing walls check for strainght parts and corners
                foreach (var neighbour in tilesToCheck[i].neighbours)
                {
                    //change straight walls into a closed walls
                    if (neighbour != null && (neighbour.autotileID == (int)TileType.WallStraight_1 || neighbour.autotileID == (int)TileType.WallStraight_2))
                    {
                        //left neighbour
                        if (neighbour.id == tilesToCheck[i].id - 1)
                        {
                            neighbour.autotileID = (int)TileType.WallEndLeft;
                        }
                        //right neighbour
                        else if (neighbour.id == tilesToCheck[i].id + 1)
                        {
                            neighbour.autotileID = (int)TileType.WallEndRight;
                        }
                        //up neighbour
                        else if (neighbour.id == tilesToCheck[i].id - cols)
                        {
                            neighbour.autotileID = (int)TileType.WallEndUp;
                        }
                        //down neighbour
                        else if (neighbour.id == tilesToCheck[i].id + cols)
                        {
                            neighbour.autotileID = (int)TileType.WallEndDown;
                        }
                    }
                    //change corner into a half-closed walls
                    else if (neighbour != null && (neighbour.autotileID == (int)TileType.Corner_1
                        || neighbour.autotileID == (int)TileType.Corner_2
                        || neighbour.autotileID == (int)TileType.Corner_3
                        || neighbour.autotileID == (int)TileType.Corner_4))
                    {
                        if (neighbour.id == tilesToCheck[i].id - 1)
                        {
                            if (neighbour.autotileID == (int)TileType.Corner_1)
                            {
                                neighbour.autotileID = (int)TileType.WallEndDown;
                            }
                            else if (neighbour.autotileID == (int)TileType.Corner_4)
                            {
                                neighbour.autotileID = (int)TileType.WallEndUp;
                            }

                        }
                        else if (neighbour.id == tilesToCheck[i].id + 1)
                        {
                            if (neighbour.autotileID == (int)TileType.Corner_2)
                            {
                                neighbour.autotileID = (int)TileType.WallEndDown;
                            }
                            else if (neighbour.autotileID == (int)TileType.Corner_3)
                            {
                                neighbour.autotileID = (int)TileType.WallEndUp;
                            }
                        }
                        else if (neighbour.id == tilesToCheck[i].id - cols)
                        {
                            if (neighbour.autotileID == (int)TileType.Corner_1)
                            {
                                neighbour.autotileID = (int)TileType.WallEndRight;
                            }
                            else if (neighbour.autotileID == (int)TileType.Corner_2)
                            {
                                neighbour.autotileID = (int)TileType.WallEndLeft;
                            }
                        }
                        else if (neighbour.id == tilesToCheck[i].id + cols)
                        {
                            if (neighbour.autotileID == (int)TileType.Corner_4)
                            {
                                neighbour.autotileID = (int)TileType.WallEndRight;
                            }
                            else if (neighbour.autotileID == (int)TileType.Corner_3)
                            {
                                neighbour.autotileID = (int)TileType.WallEndLeft;
                            }
                        }
                    }
                    //change half-closed wall to full closed wall
                    else if (neighbour != null && (neighbour.autotileID == (int)TileType.WallEndDown
                        || neighbour.autotileID == (int)TileType.WallEndLeft
                        || neighbour.autotileID == (int)TileType.WallEndRight
                        || neighbour.autotileID == (int)TileType.WallEndUp))
                    {
                        //left neighbour
                        if (neighbour.id == tilesToCheck[i].id - 1)
                        {
                            //check each neighbour
                            for (int k = 0; k < neighbour.neighbours.Length; k++)
                            {
                                CheckForDecorativeChildTiles(i, neighbour, k);
                            }

                        }
                        //right neighbour
                        else if (neighbour.id == tilesToCheck[i].id + 1)
                        {
                            //check each neighbour
                            for (int k = 0; k < neighbour.neighbours.Length; k++)
                            {
                                CheckForDecorativeChildTiles(i, neighbour, k);
                            }
                        }
                        //up neighbour
                        else if (neighbour.id == tilesToCheck[i].id - cols)
                        {
                            //check each neighbour
                            for (int k = 0; k < neighbour.neighbours.Length; k++)
                            {
                                CheckForDecorativeChildTiles(i, neighbour, k);
                            }
                        }
                        //down neighbour
                        else if (neighbour.id == tilesToCheck[i].id + cols)
                        {
                            //check each neighbour
                            for (int k = 0; k < neighbour.neighbours.Length; k++)
                            {
                                CheckForDecorativeChildTiles(i, neighbour, k);
                            }
                        }
                    }

                }
            }
        }
    }

    private void CheckForDecorativeChildTiles(int i, Tile neighbour, int k)
    {
        //if neighbour exists and it isnt the current tile we are checking
        if (neighbour.neighbours[k] != null && neighbour.neighbours[k].id != i)
        {
            //if its child is a decorative tile
            if (childTiles[neighbour.neighbours[k].id].autotileID > -1)
            {
                neighbour.autotileID = (int)TileType.WallEndFull;
            }
        }
    }

    public void DecorateTiles(Tile[] tiles, float percent, TileType type)
    {
        //we will calculate how many tiles do we want to modify
        var total = Mathf.FloorToInt(tiles.Length * percent);
        RandomizeTileArray(tiles);
        for (int i = 0; i < total; i++)
        {
            var tile = tiles[i];

            //if tile is empty we will turn it into a ocean tile (obstacle)
            //if (type == TileType.Water)
            //{
            //    tile.ClearNeighbours();
            //}

            //we will override the id of the tile to match its new look
            tile.autotileID = (int)type;
        }
    }

    public Tile[] DecorateNumberOfTiles(Tile[] tiles, int number, TileType type)
    {
        RandomizeTileArray(tiles);
        Tile[] result = new Tile[number];
        for (int i = 0; i < number; i++)
        {
            var tile = tiles[i];

            //if tile is empty we will turn it into a ocean tile (obstacle)
            //if (type == TileType.Water)
            //{
            //    tile.ClearNeighbours();
            //}

            //we will override the id of the tile to match its new look
            tile.autotileID = (int)type;
            result[i] = tile;
        }
        return result;
    }

    public Tile[] DecorateNumberOfTiles(Tile[] tiles, int number, TileType type, bool rotateInnerTile, int angle)
    {
        RandomizeTileArray(tiles);
        Tile[] result = new Tile[number];
        for (int i = 0; i < number; i++)
        {
            var tile = tiles[i];

            //if tile is empty we will turn it into a ocean tile (obstacle)
            //if (type == TileType.Water)
            //{
            //    tile.ClearNeighbours();
            //}

            //we will override the id of the tile to match its new look
            tile.autotileID = (int)type;
            tile.rotate = true;
            tile.rotateAngle = angle;
            result[i] = tile;
        }
        return result;
    }

    public Tile[] DecorateNumberOfInnerTiles(Tile[] tiles, int number, TileType baseType, TileType innerType, bool rotateInnerTile, int angle)
    {
        Tile[] result = DecorateNumberOfTiles(tiles, number, baseType);

        for (int i = 0; i < number; i++)
        {
            //find a tile in childTiles[] array with the same index as the parent tile
            var selectedTile = childTiles[result[i].id];

            selectedTile.autotileID = (int)innerType;
            selectedTile.rotate = rotateInnerTile;
            selectedTile.rotateAngle = angle;
        }
        return result;
    }

    /// <summary>
    /// Fisher-Yates algorithm implementation for suffeling the array of tiles
    /// </summary>
    /// <param name="tiles"></param>
    public void RandomizeTileArray(Tile[] tiles)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            var tmp = tiles[i];
            var r = Random.Range(i, tiles.Length);
            tiles[i] = tiles[r];
            tiles[r] = tmp;
        }
    }

    public void UsePreset(int number)
    {
        int index = number < presets.Length ? number : 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].autotileID = presets[index][i];
        }
    }

    /// <summary>
    /// We will create a building that is minimum 4x4 + outline of walls
    /// </summary>
    /// <param name="row"></param>
    /// <param name="noOfEmptyPart"></param>
    public int CreateFloorsStartingFromTile(int row, int noOfEmptyPart)
    {
        int startCol = 1;
        int startRow = row;
        int endCol = 0;
        int returnBool = 0;
        //try creating a room starting from tile [1,col]
        if ((Random.Range(0, 100) > 30))
        {
            //our end index can be from 4 (getting us minimal room space) or maximum room space of 8
            int randomNo = Random.Range(0, (cols - 2) / 2+1);
            if (noOfEmptyPart == 1)
            {
                randomNo = Random.Range(3, (cols - 2) / 2);
            }

            if (randomNo == 0)
            {
                returnBool = 2;
            }
            endCol = Mathf.Clamp(startCol + ((cols - 2) / 2 - 1) + randomNo, 1, cols - 2);

        }
        //we have not created the 1st chunk of a room - we need to create a second starting from [5,startCol]
        else
        {
            if (noOfEmptyPart != 2)
            {
                returnBool = 1;
                startCol = ((cols - 2) / 2 + 1);
                endCol = cols - 2;
            }
            else
            {
                returnBool = 2;
                startCol = 1;
                int randomNo = 0;
                if ((Random.Range(0, 100) > 50))
                {
                    //our end index can be from 4 (getting us minimal room space) or maximum room space of 8
                    randomNo = Random.Range(0, 5);
                }
                endCol = Mathf.Clamp(startCol + ((cols - 2) / 2 - 1) + randomNo, 1, cols - 2);
            }


        }

        endRow = Mathf.Clamp(Random.Range(startRow + 3, startRow + ((rows - 2) / 2)), startRow + 3, rows - 1);

        for (int r = startRow; r <= endRow; r++)
        {
            //iterate through each column in this row
            for (int c = startCol; c <= endCol; c++)
            {
                // get the node from our array storage
                Tile tile = tiles[cols * r + c];
                tile.autotileID = (int)TileType.Floor;
            }
        }

        return returnBool;
    }

    public void CreateWalls()
    {
        for (int r = 0; r < rows; r++)
        {
            //iterate through each column in this row
            for (int c = 0; c < cols; c++)
            {
                // get the node from our array storage
                Tile tile = tiles[cols * r + c];

                if (tile.autotileID == (int)TileType.Floor)
                {
                    //if it is floor tile it is bound to have 4 neighbours
                    for (int i = 0; i < tile.neighbours.Length; i++)
                    {
                        if (tile.neighbours[i] != null)
                        {
                            //setting up straight walls
                            Tile neighbour = tile.neighbours[i];
                            if (neighbour.autotileID == (int)TileType.Grass)
                            {
                                switch (i)
                                {
                                    case 0:
                                        neighbour.autotileID = (int)TileType.WallStraight_1;
                                        break;
                                    case 1:
                                        neighbour.autotileID = (int)TileType.WallStraight_2;
                                        break;
                                    case 2:
                                        neighbour.autotileID = (int)TileType.WallStraight_2;
                                        break;
                                    case 3:
                                        neighbour.autotileID = (int)TileType.WallStraight_1;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                    }

                    //putting up corners
                    int id = tile.CalculateId();
                    //Debug.Log("Tile " + tile.id + " calculated " + id);
                    switch (id)
                    {
                        case 200:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[7] != null)
                                tile.neighbours[7].autotileID = (int)TileType.Corner_1;
                            break;
                        case 164:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[6] != null)
                                tile.neighbours[6].autotileID = (int)TileType.Corner_2;
                            break;
                        case 49:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[4] != null)
                                tile.neighbours[4].autotileID = (int)TileType.Corner_3;
                            break;
                        case 82:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[5] != null)
                                tile.neighbours[5].autotileID = (int)TileType.Corner_4;
                            break;
                        case 251:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[5] != null)
                                tile.neighbours[5].autotileID = (int)TileType.Corner_2;
                            break;
                        case 254:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[7] != null)
                                tile.neighbours[7].autotileID = (int)TileType.Corner_3;
                            break;
                        case 253:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[6] != null)
                                tile.neighbours[6].autotileID = (int)TileType.Corner_4;
                            break;
                        case 247:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[4] != null)
                                tile.neighbours[4].autotileID = (int)TileType.Corner_1;
                            break;
                        case 165:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[6] != null)
                                tile.neighbours[6].autotileID = (int)TileType.Corner_2;
                            break;
                        case 217:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[6] != null)
                                tile.neighbours[6].autotileID = (int)TileType.Corner_4;
                            if (tile.neighbours[2] != null)
                                tile.neighbours[2].autotileID = (int)TileType.Corner_2;
                            break;
                        case 249:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[6] != null)
                                tile.neighbours[6].autotileID = (int)TileType.Corner_4;
                            if (tile.neighbours[5] != null)
                                tile.neighbours[5].autotileID = (int)TileType.Corner_2;
                            break;
                        case 53:
                            tile.autotileID = (int)TileType.Floor_near_corner;
                            if (tile.neighbours[4] != null)
                                tile.neighbours[4].autotileID = (int)TileType.Corner_3;
                            break;
                        case 255:
                            tile.autotileID = (int)TileType.Floor_no_wall_neighbours;
                            break;
                        default:
                            break;
                    }
                }


            }
        }
    }
}

