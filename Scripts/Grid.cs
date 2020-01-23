using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    private GameObject mapContainer = null;
    private GameObject tilePrefab = null;
    private Map map = null;
    private Sprite[] wallTexture = null;
    /// <summary>
    /// Create a 2d grid of objects that will represent the tiles
    /// </summary>
    public void CreateGrid(Map map, Vector2 tileSize, GameObject tilePrefab, GameObject mapContainer, Sprite[] wallTexture)
    {
        ClearMapContainer(mapContainer);
        var total = map.tiles.Length;

        var maxColumns = map.cols;
        var column = 0;
        var row = 0;

        this.mapContainer = mapContainer;
        this.tilePrefab = tilePrefab;
        this.map = map;
        this.wallTexture = wallTexture;

        for (int i = 0; i < total; i++)
        {
            column = i % maxColumns;

            var newX = column * tileSize.x;
            //we are moving rows down - negative value
            var newY = -row * tileSize.y;


            //create base sprites
            var go = CreateTile(i, newX, newY,mapContainer);

            var tile = map.tiles[i];
            var spriteID = tile.autotileID;
            if (spriteID >= 0)
            {
                var spriteRenderer = go.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = wallTexture[spriteID];
            }
            if (tile.rotate)
            {
                go.transform.Rotate(new Vector3(0, 0, Mathf.Clamp(tile.rotateAngle,-360,360)));
            }

            //create decorative sprites
            spriteID = map.childTiles[i].autotileID;
            if (spriteID != -1)
            {
                var gochild = CreateTile(i, 0, 0,go);
                var childSpriteRenderer = gochild.GetComponent<SpriteRenderer>();
                childSpriteRenderer.sprite = wallTexture[spriteID];
                childSpriteRenderer.sortingLayerName = "InnerTiles";
                if (map.childTiles[i].rotate)
                {
                    gochild.transform.Rotate(new Vector3(0, 0, Mathf.Clamp(map.childTiles[i].rotateAngle, -360, 360)));
                }
            }

            //move to the next row
            if (column == (maxColumns - 1))
            {
                row++;
            }
        }


    }

    public void TileAddChildTile(int tileId, TileType type, bool rotate)
    {
        //var go = CreateTile(this.map.tiles.Length + tileId, 0f, 0f);

        var go = Instantiate(tilePrefab, mapContainer.transform);
        //GameObject.Find("Tile " + tileId).transform
        go.name = "Tile " + this.map.tiles.Length + tileId;
        var spriteID = map.tiles[tileId].autotileID;
        if (spriteID >= 0)
        {
            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = wallTexture[(int)type];
            spriteRenderer.sortingLayerName = "InnerTiles";
        }
        if (rotate)
        {
            go.transform.Rotate(new Vector3(0, 0, 90));
        }
        go.transform.position = GameObject.Find("Tile " + tileId).transform.position;
    }

    public void RepaintTilesWithSprite(Tile tilesToRepaint, TileType type)
    {
        Debug.Log("Tile " + tilesToRepaint.id+" Type "+ type);
        var spriteRenderer = GameObject.Find("Tile " + tilesToRepaint.id).GetComponent<SpriteRenderer>();
        map.tiles[tilesToRepaint.id].autotileID = (int)type;
        spriteRenderer.sprite = wallTexture[(int)type];
        Debug.Log(spriteRenderer.sprite.name);
        
    }

    public void ClearMapContainer(GameObject mapContainer)
    {
        var children = mapContainer.transform.GetComponentsInChildren<Transform>();
        //looping in reverse because destroyin an element of the array shrinks it
        for (int i = children.Length - 1; i > 0; i--)
        {
            Destroy(children[i].gameObject);
        }
    }

    public GameObject CreateTile(int i, float newX, float newY, GameObject parent)
    {
        var go = Instantiate(tilePrefab);
        go.name = "Tile " + i;
        //go.GetComponent<Tile>().gameObjectName = go.name;
        go.transform.SetParent(parent.transform);
        go.transform.localPosition = new Vector3(newX, newY, 0);
        return go;
    }
}
