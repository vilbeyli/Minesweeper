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
    private static bool _rightAndLeftPressed;
    private static bool _revealAreaIssued;
    private static bool _initialClickIssued;    // used to track first-click-death and start-timer


    // private variables
    private GridScript _grid;

    // handles
    public UIManager UI;
    public Canvas PauseMenu;

    //--------------------------------------------------------
    // Function Definitions

    // getters & setters
    public GridScript Grid
    {
        get { return _grid; }
        set { _grid = value; }
    }

    public static bool InitialClickIssued
    {
        get { return _initialClickIssued; }
        set
        {
            _initialClickIssued = value;
        }
    }

    // unity functions

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
        // Toggle pause menu and companion components
        //PauseMenu.GetComponentInChildren<Canvas>().enabled = !PauseMenu.enabled; 
        PauseMenu.enabled = !PauseMenu.enabled;
        GameObject.FindGameObjectWithTag("UILight").GetComponent<Light>().intensity = (PauseMenu.enabled ? 1 : 0);

        // update gamestate
        if(!GameManager.IsGameOver)
            GameManager.IsGamePaused = PauseMenu.enabled;

        Time.timeScale = System.Convert.ToSingle(!PauseMenu.enabled);
        //Debug.Log("PLAYERINPUT:: TimeScale: " + Time.timeScale);
    }

    public void OnMouseOver(Tile tile)
    {
       
        // RIGHT CLICK: FLAG
        if (!_revealAreaIssued && Input.GetMouseButtonDown(1))
        {
            if(!Input.GetMouseButton(0) && !tile.IsRevealed())
                tile.ToggleFlag();
        }

        // LEFT CLICK: HIGHLIGHT TILE
        if (Input.GetMouseButton(0))
        {
            if(!tile.IsRevealed() && !tile.IsFlagged())
                _grid.HighlightTile(tile.GridPosition);
            
            // LEFT & RIGHT CLICK: HIGHLIGHT AREA
            if (Input.GetMouseButton(1))
            {
                _rightAndLeftPressed = true;
            }
        }

        // LEFT RELEASE: REVEAL
        if (Input.GetMouseButtonUp(0) && !_revealAreaIssued)
        {
            if (!tile.IsFlagged() && !tile.IsRevealed())
            {
                if (!_initialClickIssued)
                {
                    if (tile.IsMine())
                        _grid.SwapTileWithMineFreeTile(tile.GridPosition);

                    _initialClickIssued = true;
                    GetComponent<GameManager>().StartTimer();
                }

                else
                {
                    if(!Input.GetMouseButton(1))
                        tile.Reveal();
                }
            }
                
        }

        // LEFT & RIGHT RELEASE: REVEAL NEIGHBORS IF ENOUGH NEIGHBOR FLAGGED
        if (_rightAndLeftPressed)
        {
            _grid.HighlightArea(tile.GridPosition);
            if(!tile.IsRevealed() && !tile.IsFlagged())  tile.Highlight();
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                _revealAreaIssued = true;
                if (tile.IsRevealed() && tile.IsNeighborsFlagged())
                {
                    _grid.RevealArea(tile);
                }
                else
                {
                    _grid.RevertHighlightArea(tile.GridPosition);
                    _grid.RevertHighlightTile(tile.GridPosition);
                }
            }
        }

        if (!Input.GetMouseButton(0) || !Input.GetMouseButton(1))
        {
            _rightAndLeftPressed = false;
        }

        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            _revealAreaIssued = false;
        }
    }

    public void OnMouseExit(Tile tile)
    {
    
        // revert highlighted tiles
        if(!tile.IsRevealed() && !tile.IsFlagged()) 
            tile.RevertHighlight();

        foreach (Vector2 pos in tile.NeighborTilePositions)
        {
            Tile neighbor = _grid.Map[(int) pos.x][(int) pos.y];
            if (!neighbor.IsRevealed() && !neighbor.IsFlagged())
                neighbor.RevertHighlight();
        }
        
    }

}
