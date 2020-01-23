using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRoomTester : MonoBehaviour {

    [Header("Map Dimensions")]
    public int mapWidth = 10;
    public int mapHeight = 10;

    [Space]
    [Header("Visualize map")]
    public GameObject mapContainer;
    public GameObject tilePrefab;
    public Vector2 tileSize = new Vector2(64, 64);

    [Space]
    [Header("Map Sprites")]
    public Sprite[] wallTexture;

    [Space]
    [Header("Map options")]
    public int doorCount = 1;
    public float percentOfWindows = 0.3f;

    public Map map;
    public Grid grid;


    // Use this for initialization
    void Start () {
        map = new Map();
        grid = gameObject.AddComponent<Grid>();
        MakeMap();

    }



    public void MakeMap () {
        map.NewMap(mapWidth, mapHeight);
        //map.UsePreset(1);
        int noOfEmptyRoom = 0;
        noOfEmptyRoom = map.CreateFloorsStartingFromTile(Mathf.Clamp(Random.Range(1, (map.rows) / 2 + 1), 1, ((map.rows) / 2) - 4), noOfEmptyRoom); //Mathf.Clamp(Random.Range(1, (map.rows) / 2+1),1, ((map.rows) / 2)-3)
        //Debug.Log(noOfEmptyRoom);
        map.CreateFloorsStartingFromTile(map.endRow+1, noOfEmptyRoom);
        map.CreateWalls();
        map.CompleteTheHouse(doorCount, percentOfWindows);
        grid.CreateGrid(map,tileSize,tilePrefab,mapContainer,wallTexture);
        

    }

    
}
