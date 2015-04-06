using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    //-------------------------------------------------------------
    // Variable Declarations 

    // handles
    public GameObject TilePrefab;

    // private variables
    private List<List<Tile>> _map = new List<List<Tile>>();
    private GameSettings _settings;

    //-------------------------------------------------------------
    // Function Definitions

    // member functions
    public void GenerateMap(GameSettings Settings)
    {
        _settings = Settings;
        _map = new List<List<Tile>>();
        for (int i = 0; i < _settings.Width; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < _settings.Height; j++)
            {
                Tile tile = ((GameObject)Instantiate(TilePrefab,
                                                    new Vector3(i - Mathf.Floor(_settings.Width / 2), 0, -j + Mathf.Floor(_settings.Height / 2)),
                                                    Quaternion.identity
                                                    )).GetComponent<Tile>();
                tile.GridPosition = new Vector2(i, j);
                row.Add(tile);
                tile.transform.parent = transform;
                tile.GetComponent<Tile>().ParentGrid = this;
            }

            _map.Add(row);
        }

        Debug.Log("GRID::GENERATEMAP Map Generated w/ Settings: H=" + _settings.Height
                                                    + " W=" + _settings.Width
                                                   + " M=" + _settings.Mines
                                                   );

        PlaceMinesOnTiles();
        UpdateTileValues();

    }

    void PlaceMinesOnTiles()
    {
        // randomly pick locations on which mines will be placed.
        HashSet<Vector2> mineLocations = new HashSet<Vector2>();
        while (mineLocations.Count < _settings.Mines)
        {
            int x = Random.Range(0, _settings.Width);
            int y = Random.Range(0, _settings.Height);
            mineLocations.Add(_map[x][y].GridPosition);
        }

        // place mines on locations
        foreach (Vector2 loc in mineLocations)
            _map[(int)loc.x][(int)loc.y].PlaceMineOnTile();

        //Debug.Log(mineLocations.Count + " mines randomly placed on the grid");
    }

    void UpdateTileValues()
    {
        // update tile values
        foreach (List<Tile> row in _map)
        {
            // for each tile
            foreach (Tile tile in row)
            {
                SetNeighbors(tile);

                // if current tile is not a mine
                if (!tile.IsMine()) 
                { 
                    // traverse neighbors to update tile value
                    int NearbyMineCount = 0;
                    foreach (Vector2 pos in tile.NeighborTilePositions)
                    {
                        // increment nearby mine count by 1 if a neighbor tile is a mine
                        Tile _tile = _map[(int) pos.x][(int) pos.y];
                        if (_tile.IsMine()) 
                            ++NearbyMineCount;
                    }

                    // update the tile value
                    tile.TileValue = NearbyMineCount;
                }
            }
        }
    }

    void SetNeighbors(Tile tile)
    {
        // first, add every possible neighbor tile position to the possible-neighbors list
        List<Vector2> NeighborPositions = new List<Vector2>();
        NeighborPositions.Add(new Vector2(tile.GridPosition.x + 1, tile.GridPosition.y));
        NeighborPositions.Add(new Vector2(tile.GridPosition.x - 1, tile.GridPosition.y));
        NeighborPositions.Add(new Vector2(tile.GridPosition.x, tile.GridPosition.y + 1));
        NeighborPositions.Add(new Vector2(tile.GridPosition.x, tile.GridPosition.y - 1));
        NeighborPositions.Add(new Vector2(tile.GridPosition.x + 1, tile.GridPosition.y + 1));
        NeighborPositions.Add(new Vector2(tile.GridPosition.x - 1, tile.GridPosition.y + 1));
        NeighborPositions.Add(new Vector2(tile.GridPosition.x + 1, tile.GridPosition.y - 1));
        NeighborPositions.Add(new Vector2(tile.GridPosition.x - 1, tile.GridPosition.y - 1));

        // then remove those positions which are beyond boundary
        for (int i = NeighborPositions.Count - 1; i >= 0; --i)
        {
            Vector2 pos = NeighborPositions[i];
            if (pos.x < 0 || pos.x >= _settings.Width || pos.y < 0 || pos.y >= _settings.Height)
            {
                NeighborPositions.RemoveAt(i);
            } 
        }

        // set the correct neighbor positions in the given tile
        tile.NeighborTilePositions = NeighborPositions;
    }

    public void RevealAllTiles()
    {
        foreach (List<Tile> row in _map)
        {
            foreach (Tile tile in row)
                tile.Reveal();
        }

        Debug.Log("GRID:: Revealed all tiles that are not mines. Unrevealed Tile Count = " + Tile.UnrevealedTileCount);
    }

    public void HighlightArea(Vector2 pos)
    {
        // find the tile from _map by given position vector
        Tile tile = _map[(int)pos.x][(int)pos.y];

        // highlight the tile itself
        _map[(int)pos.x][(int)pos.y].Highlight();

        // highlight the neighbors of the tile
        foreach (Vector2 _pos in tile.NeighborTilePositions)
            _map[(int)_pos.x][(int)_pos.y].Highlight();


    }

    public void RevertHighlightArea(Vector2 pos)
    {
        // find the tile from _map by given position vector
        Tile tile = _map[(int) pos.x][(int) pos.y];

        // revert the tile itself
        _map[(int) pos.x][(int) pos.y].RevertHighlight();

        // revert the neighbors of the tile
        foreach (Vector2 _pos in tile.NeighborTilePositions)
            _map[(int)_pos.x][(int)_pos.y].RevertHighlight();
    }

}
