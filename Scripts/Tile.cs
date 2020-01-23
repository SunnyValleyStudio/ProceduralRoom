using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum Sides
{
    Bottom,
    Right,
    Left,
    Top,
    Bottom_Right,
    Bottm_Left,
    Top_Right,
    Top_Left
}

public class Tile
{

    public int id = 0;
    public string gameObjectName = null;
    public Tile[] neighbours = new Tile[8];
    public int autotileID = 0;
    public bool visited;
    public bool rotate = false;
    public int rotateAngle = 0;
 
    public void AddNeighbours(Sides side, Tile tile)
    {
        neighbours[(int)side] = tile;
        //CalculateAutoTileID();
    }

    /// <summary>
    /// Removes a specyfic tile from neighbours of this tile (if it's its neighbour)
    /// </summary>
    /// <param name="tile"></param>
    public void RemoveNeighbour(Tile tile)
    {
        var total = neighbours.Length;
        for (int i = 0; i < total; i++)
        {
            if (neighbours[i] != null)
            {
                if (neighbours[i].id == tile.id)
                {
                    neighbours[i] = null;
                }
            }
        }
        CalculateAutoTileID();
    }

    /// <summary>
    /// Removes all the neighbours
    /// </summary>
    public void ClearNeighbours()
    {
        var total = neighbours.Length;
        for (int i = 0; i < total; i++)
        {
            var tile = neighbours[i];
            if (tile != null)
            {
                tile.RemoveNeighbour(this);
                neighbours[i] = null;
            }
        }

        //CalculateAutoTileID();
    }

    /// <summary>
    /// Creates a 4-bit value ex. 0000 where each digit stands for one of the sides [bottom,right,left,top] 
    /// value of 0 means that there is no neighbour on this side
    /// </summary>
    private void CalculateAutoTileID()
    {
        if (autotileID == 0)
        {
            StringBuilder sideValues = new StringBuilder();
            foreach (Tile tile in neighbours)
            {
                sideValues.Append(tile == null ? "0" : "1");
            }

            //converts a string to a number 
            autotileID = Convert.ToInt32(sideValues.ToString(), 2);
        }
        
    }

    public int CalculateId()
    {
        StringBuilder sideValues = new StringBuilder();
        foreach (Tile tile in neighbours)
        {
            sideValues.Append((tile !=null && 
                (tile.autotileID == (int)TileType.Floor 
                || tile.autotileID == (int)TileType.Floor_no_wall_neighbours
                || tile.autotileID == (int)TileType.Floor_near_corner)) ? "1" : "0");
        }
        //Debug.Log("8bit value: "+sideValues.ToString());
        //converts a string to a number 
        return Convert.ToInt32(sideValues.ToString(), 2);
    }

}

