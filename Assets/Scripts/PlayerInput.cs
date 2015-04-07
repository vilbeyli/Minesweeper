using UnityEngine;


//---------------
// Mouse logic:
// - Left&Right:    if(tile is revealed)        Reveal Neighbors
// - Left click:    if(not after Left&Right)    Reveal Tile
// - Right click:   if(not after Left&Right)    Flag Tile

public class PlayerInput : MonoBehaviour {

    //-------------------------------------
    // Variable Declarations
    
    // static variables
    private static bool _rightAndLeftIssued;

    // private variables
    private GridScript _grid;

    // public variables


    //--------------------------------------------------------
    // Function Definitions

    // getters & setters
    public GridScript Grid
    {
        get { return _grid; }
        set { _grid = value; }
    }

    // unity functions
	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    // member functions
    public void OnMouseOver(Vector2 pos)
    {
        Tile tile = _grid.Map[(int)pos.x][(int)pos.y];
        

        //------------------------------------------------------------------
        // Release Events 

        // left click released
        if (Input.GetMouseButtonUp(0))
        {
            if (Input.GetMouseButton(1)) // if right click is held as well
            {
                if (tile.IsRevealed())
                {
                    // CARE FLAG LOGIC HERE!!!
                    Debug.Log("PLAYERINPUT:: Reveal Neightbors of Tile " + tile.Coordinates());
                }
                else
                {
                    // SAME AS BELOW
                    Debug.Log("PLAYERINPUT:: Ineffective Simultaneous click(0up) on Unrevealed Tile " + tile.Coordinates());
                    _grid.RevertHighlightArea(tile.GridPosition);
                }
                _rightAndLeftIssued = true;
            }
            else // if only left click is released
            {
                if (_rightAndLeftIssued)
                    _rightAndLeftIssued = false;
                else
                {
                    Debug.Log("PLAYERINPUT:: Reveal Tile " + tile.Coordinates());
                    tile.Reveal();
                }

            }
        }

        // right click released
        if (Input.GetMouseButtonUp(1))
        {
            if (Input.GetMouseButton(0)) // if left click is held as well
            {
                _rightAndLeftIssued = true;
                if (tile.IsRevealed())
                {
                    // CARE FLAG LOGIC HERE!!!
                    Debug.Log("PLAYERINPUT:: Reveal Neightbors of Tile " + tile.Coordinates());
                }
                else
                {
                    // SAME AS ABOVE
                    Debug.Log("PLAYERINPUT:: Ineffective Simultaneous click(1up) on Unrevealed Tile " + tile.Coordinates());
                    _grid.RevertHighlightArea(tile.GridPosition);
                }

            }
            else // if only right click is released
            {
                if (_rightAndLeftIssued)
                    _rightAndLeftIssued = false;
                else
                {
                    Debug.Log("PLAYERINPUT:: Flag the Tile " + tile.Coordinates());
                }
            }
        }

        //-------------------------------------------------------------------
        // Click Events 

        if (_grid == null) return;
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            //Debug.Log("BOTH MOUSE BUTTONS DOWN ON TILE " + Coordinates());
            _grid.HighlightArea(tile.GridPosition);
        }
    }

    public void OnMouseExit(Vector2 pos)
    {
        //-------------------------------------------------------------------
        // PROBLEM: When dragged with both buttons down FAST, sometimes
        //          some tiles stay highlighted
        // TODO: Fix it.

        Tile tile = _grid.Map[(int)pos.x][(int)pos.y];

        if (_grid == null) return;
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            _grid.RevertHighlightArea(tile.GridPosition);
        }
    }
}
