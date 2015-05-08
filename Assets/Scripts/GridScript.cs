using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;


public class GridScript : MonoBehaviour
{

    //-------------------------------------------------------------
    // Variable Declarations 

    // handles
    public GameObject TilePrefab;

    // private variables
    private List<List<Tile>> _map = new List<List<Tile>>();
    private GameSettings _settings;

    private Tile _detonationTile;

    [SerializeField]
    private AnimationSettings _animationSettings;

    public List<List<Tile>> Map
    {
        get { return _map; }
    }

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
                tile.GetComponent<Tile>().Grid = this;
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
                        Tile _tile = _map[(int)pos.x][(int)pos.y];
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
            {
                if(!tile.IsMine())
                    tile.Reveal();
            }
        }

    }

    public void ConcealAllTiles()
    {
        PlayerInput.InitialClickIssued = false;

        foreach (List<Tile> row in _map)
        {
            foreach (Tile tile in row)
                tile.Conceal();
        }

        //Debug.Log("GRID:: Revealed all tiles that are not mines. Unrevealed Tile Count = " + Tile.UnrevealedTileCount);
    }

    public void RevealArea(Tile tile)
    {
        foreach (Vector2 neighborTilePos in tile.NeighborTilePositions)
        {
            Tile neighbor = _map[(int) neighborTilePos.x][(int) neighborTilePos.y];
            if(!neighbor.IsRevealed() && !neighbor.IsFlagged())
                neighbor.Reveal();
        }

    }

    public void HighlightArea(Vector2 pos)
    {
        // find the tile from _map by given position vector
        Tile tile = _map[(int)pos.x][(int)pos.y];

        // highlight the neighbors of the tile
        foreach (Vector2 _pos in tile.NeighborTilePositions)
        {
            Tile neighbor = _map[(int) _pos.x][(int) _pos.y];
            if (!(neighbor.IsFlagged() || neighbor.IsRevealed()))
                neighbor.Highlight();
        }


    }

    public void HighlightTile(Vector2 pos)
    {
        Tile tile = _map[(int) pos.x][(int) pos.y];

        // highlight the tile itself
        if (!(tile.IsFlagged() || tile.IsRevealed()))
            _map[(int)pos.x][(int)pos.y].Highlight();
    }

    public void RevertHighlightArea(Vector2 pos)
    {
        // find the tile from _map by given position vector
        Tile tile = _map[(int)pos.x][(int)pos.y];

        // revert the neighbors of the tile
        foreach (Vector2 _pos in tile.NeighborTilePositions)
        {
            Tile neighbor = _map[(int) _pos.x][(int) _pos.y];

            if(!neighbor.IsRevealed() && !neighbor.IsFlagged())
                neighbor.RevertHighlight();
        }
    }

    public void RevertHighlightTile(Vector2 pos)
    {
        // revert the tile itself
        Tile tile = _map[(int)pos.x][(int)pos.y];
        if(!tile.IsRevealed() && !tile.IsFlagged())
            tile.RevertHighlight();
    }

    public void SwapTileWithMineFreeTile(Vector2 pos)
    {
        // Find the mine-free tile
        Vector2 mineFreePos = new Vector2();  // mine free tile position
        bool found = false;
        foreach (List<Tile> row in _map)
        {
            foreach (Tile tile in row)
            {
                if (!tile.IsMine())
                {
                    mineFreePos = tile.GridPosition;
                    found = true;
                    break;
                }
            }
            if (found) break;
        }

        int tmp = _map[(int) mineFreePos.x][(int) mineFreePos.y].TileValue;
        _map[(int) mineFreePos.x][(int) mineFreePos.y].TileValue = _map[(int) pos.x][(int) pos.y].TileValue;
        _map[(int) pos.x][(int) pos.y].TileValue = tmp;

        UpdateTileValues();

    }

    void Log(string msg, Vector2 pos, Vector2 mineFreePos)
    {
        Debug.Log(msg + ":\n"
            + "_map[" + pos.x + "][" + pos.y + "]: ONCLICK"
                + "\nGridPos=" + _map[(int)pos.x][(int)pos.y].GridPosition
                + "\nMine=" + _map[(int)pos.x][(int)pos.y].IsMine()
                + "\nCoord: " + _map[(int)pos.x][(int)pos.y].transform.position
           + "\n\n"
           + "_map[" + mineFreePos.x + "][" + mineFreePos.y + "]: MINE-FREE"
                + "\nGridPos=" + _map[(int)mineFreePos.x][(int)mineFreePos.y].GridPosition
                + "\nMine=" + _map[(int)mineFreePos.x][(int)mineFreePos.y].IsMine()
                + "\nCoord: " + _map[(int)mineFreePos.x][(int)mineFreePos.y].transform.position + "\n");
    }

    public bool AreAllTilesRevealed()
    {
        // check if all revealed
        foreach (List<Tile> row in _map)
        {
            foreach (Tile tile in row)
            {
                if (!tile.IsMine() && !tile.IsRevealed())
                    return false;
            }
        }


        // if all revealed AND if not all mined tiles are flagged
        // simply flag them
        foreach (List<Tile> row in _map)
        {
            foreach (Tile tile in row)
            {
                if (tile.IsMine() && !tile.IsFlagged())
                    tile.ToggleFlag();
            }
        }
        return true;
    }

    public void RevealMines()
    {
        foreach (List<Tile> row in _map)
        {
            foreach (Tile tile in row)
            {
                if (tile.IsMine() && !tile.IsFlagged() && !tile.IsRevealed())
                    tile.GetComponent<Renderer>().material = tile.Materials[9]; // 9: mine
                if (!tile.IsMine() && tile.IsFlagged())
                    tile.GetComponent<Renderer>().material = tile.Materials[13]; // 13: flase flag
            }
        } 
    }
}

[System.Serializable]
public class AnimationSettings
{
    [SerializeField]
    private float _detonateMineDelay;
    [SerializeField]
    private float _midDetonationDelayMin;
    [SerializeField]
    private float _midDetonationDelayMax;

    public float DetonateMineDelay
    {
        get { return _detonateMineDelay; }
    }

    public float MidDetonationDelayMin
    {
        get { return _midDetonationDelayMin; }
    }

    public float MidDetonationDelayMax
    {
        get { return _midDetonationDelayMax; }
    }
}
