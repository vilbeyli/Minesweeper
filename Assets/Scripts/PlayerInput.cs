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
    public static bool InitialClickIssued;
    public static bool IsGamePaused; 

    // private variables
    private GridScript _grid;

    // handles
    public UIManager UI;

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
	    ScanForKeyStroke();
	}


    // member functions
    void ScanForKeyStroke()
    {
        if (Input.GetKeyDown("escape")) TogglePauseMenu();
    }

    public void TogglePauseMenu()
    {
        /* 
        if (UI.GetComponentInChildren<Canvas>().enabled)
        {
            UI.GetComponentInChildren<Canvas>().enabled = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            UI.GetComponentInChildren<Canvas>().enabled = true;
            Time.timeScale = 0f;
        }
        */

        // shorter version of the code above
        Time.timeScale = System.Convert.ToSingle(UI.GetComponentInChildren<Canvas>().enabled);
        UI.GetComponentInChildren<Canvas>().enabled = !UI.GetComponentInChildren<Canvas>().enabled;
        IsGamePaused = UI.GetComponentInChildren<Canvas>().enabled;

        Debug.Log("PLAYERINPUT:: TimeScale: " + Time.timeScale);
    }

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
                    // if the first click is on mine, 
                    // swap tile properties with a non-mine tile
                    if (!InitialClickIssued && tile.IsMine())
                    {
                        Debug.Log("PLAYERINPUT:: Initial click on mine, swapping with a non mine tile!");
                        _grid.SwapTileWithMineFreeTile(tile.GridPosition);
                        tile = _grid.Map[(int) pos.x][(int) pos.y];
                    }
                       
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

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            _grid.RevertHighlightArea(tile.GridPosition);
        }
    }
}
