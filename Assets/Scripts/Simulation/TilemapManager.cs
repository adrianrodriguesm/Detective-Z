using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : Singleton<TilemapManager>
{
    public Tilemap map;
    public List<TileData> tileDatas;
    Dictionary<TileBase, TileData> dataFromTiles;

    // Start is called before the first frame update
    void Start()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDatas)
        {
            foreach(var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gripPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gripPosition);

            float speed = dataFromTiles[clickedTile].walkingSpeed;
            Debug.Log(gripPosition + " " + clickedTile + "Speed " + speed);
        }
    }

   
    TileBase GetTileFromWorldPosition(Vector3 worldposition)
    {
        Vector3Int gripPosition = map.WorldToCell(worldposition);

        return map.GetTile(gripPosition);
    }
}
