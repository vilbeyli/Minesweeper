using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //-------------------------------------------------------------
    // Variable Declarations 

    // static variabels
    private static bool _rightAndLeftIssued;
    private static int  _unrevealedTileCount;

    // handles
    private Grid _grid;

    // private variables
    private Vector2         _gridPosition = Vector2.zero;   // set when Grid::GenerateMap() is called
    private List<Vector2>   _neighborTilePositions;         // set when Grid::SetNeighbors() is called
    private bool            _revealed;                      // set when OnMouseOver() is called
    private int             _tileValue;                     // # mines nearby, -1 if mine is on the tile

    public Material[]       Materials;

    // IDEA: Mouse functions in tile, tile informs input manager (handle, awake())
    // IDEA: Generic functions like Vec2ToText() --> "(X, Y)" or Vec3ToText --> "(X, Y, Z)"

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
        get { return _tileValue; }
        set
        {
            _tileValue = value;
        }
    }
    public static int UnrevealedTileCount
    {
        get { return _unrevealedTileCount; }
    }


    // unity functions
    void Awake()
    {
        
    }
    
    void Start()
    {
        _unrevealedTileCount++;
    }

    void OnMouseEnter()
    {
        
    }

    void OnMouseExit()
    {
        //-------------------------------------------------------------------
        // PROBLEM: When dragged with both buttons down FAST, sometimes
        //          some tiles stay highlighted
        // TODO: Fix it.

        if (_grid == null) return;
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            Debug.Log(" BOTH MOUSE BUTTONS DOWN ON TILE " + Coordinates());
            _grid.RevertHighlightArea(_gridPosition);
        }
    }

    private void OnMouseOver()
    {
        //---------------
        // Mouse logic:
        // - Left&Right:    if(tile is revealed)        Reveal Neighbors
        // - Left click:    if(not after Left&Right)    Reveal Tile
        // - Right click:   if(not after Left&Right)    Flag Tile

        //-------------------------------------------------------------------
        // Release Events 

        // left click released
        if (Input.GetMouseButtonUp(0))
        {
            if (Input.GetMouseButton(1)) // if right click is held as well
            {
                if (_revealed)
                {
                    // CARE FLAG LOGIC HERE!!!
                    Debug.Log("TILE:: Reveal Neightbors of Tile " + Coordinates());
                }
                else
                {
                    // SAME AS BELOW
                    Debug.Log("TILE:: Ineffective Simultaneous click(0up) on Unrevealed Tile " + Coordinates());
                    _grid.RevertHighlightArea(_gridPosition);
                }
                _rightAndLeftIssued = true;
            }
            else // if only left click is released
            {
                if (_rightAndLeftIssued)
                    _rightAndLeftIssued = false;
                else
                {
                    Debug.Log("TILE:: Reveal Tile " + Coordinates());
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
                    Debug.Log("TILE:: Reveal Neightbors of Tile " + Coordinates());
                }
                else
                {
                    // SAME AS ABOVE
                    Debug.Log("TILE:: Ineffective Simultaneous click(1up) on Unrevealed Tile " + Coordinates());
                    _grid.RevertHighlightArea(_gridPosition);
                }

            }
            else // if only right click is released
            {
                if (_rightAndLeftIssued)
                    _rightAndLeftIssued = false;
                else
                {
                    Debug.Log("TILE:: Flag the Tile " + Coordinates());
                }
            }
        }

        //-------------------------------------------------------------------
        // Click Events 
        
        if (_grid == null) return;
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            Debug.Log(" BOTH MOUSE BUTTONS DOWN ON TILE " + Coordinates());
            _grid.HighlightArea(_gridPosition);
        }
    }


    // member functions
    string Coordinates()
    {
        return String.Format("(" + _gridPosition.x + ", " + _gridPosition.y + ")");
    }

    public void Reveal()
    {
        if (this.IsMine())
        {
            //TODO: GAME OVER LOGIC
        }
        else
        {
            _unrevealedTileCount--;
            _revealed = true;
        }

        renderer.material = Materials[_tileValue];
    }

    public void PlaceMineOnTile()
    {
        _tileValue = 9;
        //Debug.Log("Tile " + Coordinates() + " has Mine on it!");
    }

    public bool IsMine()
    {
        return _tileValue == 9;
    }

    public void Highlight()
    {
        if(!_revealed)
            renderer.material = Materials[10];
    }

    public void RevertHighlight()
    {
        if(!_revealed)
            renderer.material = Materials[11];
    }

    private bool IsHighlighted()
    {
        return renderer.material == Materials[10];
    }
}
