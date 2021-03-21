using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentManager : Singleton<EnvironmentManager>
{

    public Tilemap[] maps;
    
    Dictionary<EnvironmentType, Tilemap> tileMaps;

    // Start is called before the first frame update
    void Start()
    {
        tileMaps = new Dictionary<EnvironmentType, Tilemap>();
        foreach (var tileMap in maps)
        {
            EnvironmentType type = tileMap.gameObject.GetComponent<Evironment>().environment;
            tileMaps.Add(type, tileMap);
        }
    }

    public void SetTile(TileBase tile, EnvironmentType type, Vector3 worldposition)
    {
        Vector3Int gripPosition = tileMaps[type].WorldToCell(worldposition);
        tileMaps[type].SetTile(gripPosition, tile);
    }

    public void SetTileCellPosition(TileBase tile, EnvironmentType type, Vector3Int cellPosition)
    {
        tileMaps[type].SetTile(cellPosition, tile);
    }

    public TileBase GetTileFromWorldPosition(Vector3 worldposition, EnvironmentType type)
    {
        Vector3Int gripPosition = tileMaps[type].WorldToCell(worldposition);
        return tileMaps[type].GetTile(gripPosition);
    }

    public Vector3Int GetTileCellPosition(Vector3 worldposition, EnvironmentType type)
    {
       return tileMaps[type].WorldToCell(worldposition);   
    }

    
}
