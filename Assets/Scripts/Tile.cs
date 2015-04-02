using System;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    //-------------------------------------------------------------
    // Variable Declarations 

    // static variabels
    private static bool _rightAndLeftIssued;

    // handles
    private Grid _grid;

    // non-static variables
    private Vector2         _gridPosition = Vector2.zero;   // set when Grid::GenerateMap() is called
    private List<Vector2>   _neighborTilePositions;         // set when Grid::SetNeighbors() is called
    private bool            _revealed;                      // set when OnMouseOver() is called
    private int             _tileValue;                     // # mines nearby, -1 if mine is on the tile

    public Material[]       Materials;


    //-------------------------------------------------------------
    // Function Definitions

    // getters & setters
    public Vector2 GridPosition
    {
        get { return _gridPosition; }
        set { _gridPosition = value; }
    }
    public List<Vector2> NeighborTilePositions
    {
        get { return _neighborTilePositions; }
        set { _neighborTilePositions = value; }
    }
    public Grid ParentGrid
    {
        get { return _grid; }
        set { _grid = value; }
    }
    public int TileValue
    {
        set
        {
            _tileValue = value;
            renderer.material = Materials[_tileValue];
        }
    }

    // unity functions
    void Awake()
    {
        
    }
    
    void Start()
    {
    }

    void OnMouseOver()
    {
        //---------------
        // Mouse logic:
        // - Left&Right:    if(tile is revealed)        Reveal Neighbors
        // - Left click:    if(not after Left&Right)    Reveal Tile
        // - Right click:   if(not after Left&Right)    Flag Tile

        // left click released
        if (Input.GetMouseButtonUp(0))
        {
            if (Input.GetMouseButton(1))    // if right click is held as well
            {
                if (_revealed)
                {
                    // CARE FLAG LOGIC HERE!!!
                    Debug.Log("TILE:: Reveal Neightbors of Tile " + Vec2toText(_gridPosition));
                }
                else
                {
                    Debug.Log("TILE:: Ineffective Simultaneous click(0up) on Unrevealed Tile " + Vec2toText(_gridPosition));
                    
                    //if (_grid == null) Debug.Log("NULL GRID!");
                    //else _grid.HighlightNeighbors(_gridPosition);
                    //
                    //_grid.HighlightArea(_gridPosition);

                    //TODO: FIND EVENTs AND CALL HIGHLIGHTAREA() FUNCTION
                }
                _rightAndLeftIssued = true;
            }
            else  // if only left click is released
            {
                if (_rightAndLeftIssued)
                    _rightAndLeftIssued = false;
                else
                {
                    Debug.Log("TILE:: Reveal Tile " + Vec2toText(_gridPosition));
                    Reveal();
                }

            }
        }

        // right click released
        if (Input.GetMouseButtonUp(1))
        {
            if (Input.GetMouseButton(0)) // if left click is held as well
            {
                _rightAndLeftIssued = true;
                if (_revealed)
                {
                    // CARE FLAG LOGIC HERE!!!
                    Debug.Log("TILE:: Reveal Neightbors of Tile " + Vec2toText(_gridPosition));
                }
                else
                {
                    Debug.Log("TILE:: Ineffective Simultaneous click(1up) on Unrevealed Tile " + Vec2toText(_gridPosition));
                    
                    //if (_grid == null) Debug.Log("NULL GRID!");
                    //else _grid.HighlightNeighbors(_gridPosition);
                    //
                    //_grid.HighlightArea(_gridPosition);

                    //TODO: FIND EVENTs AND CALL HIGHLIGHTAREA() FUNCTION
                }

            }
            else // if only right click is released
            {
                if (_rightAndLeftIssued)
                    _rightAndLeftIssued = false;
                else
                {
                    Debug.Log("TILE:: Flag the Tile " + Vec2toText(_gridPosition));
                }
            }
        }
    }

    // member functions
    string Vec2toText(Vector2 v)
    {
        return String.Format("(" + v.x + ", " + v.y + ")");
    }

    public void Reveal()
    {
        _revealed = true;
        //renderer.material.color = _neighborMineCount != -1 ? Color.cyan : Color.red;
    }

    public void PlaceMineOnTile()
    {
        _tileValue = 9;
        renderer.material = Materials[9];
        //Debug.Log("Tile " + Vec2toText(_gridPosition) + " has Mine on it!");
    }

    public bool IsMine()
    {
        return _tileValue == 9;
    }
}
