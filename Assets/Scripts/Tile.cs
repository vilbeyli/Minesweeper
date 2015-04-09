using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //-------------------------------------------------------------
    // Variable Declarations 

    // static variabels

    // const variables
    private const int     TILE_FLAGGED = 12;
    private const int  TILE_UNREVEALED = 11;
    private const int   TILE_HIGHLIGHT = 10;
    private const int        TILE_MINE = 9;

    // handles
    private GridScript _grid;
    private PlayerInput _playerInput;

    // private variables
    private Vector2         _gridPosition = Vector2.zero;   // set when GridScript::GenerateMap() is called
    private List<Vector2>   _neighborTilePositions;         // set when GridScript::SetNeighbors() is called
    private bool            _revealed;                      // set when OnMouseOver() is called
    private int             _tileValue;                     // # mines nearby, 9 if mine is on the tile
    private bool             _flagged;                      // set when ToggleMine() is called

    // public variablesf
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
    public GridScript Grid
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


    // unity functions
    void Awake()
    {
        _playerInput = GameObject.Find("GameManager").GetComponent<PlayerInput>();
    }
    
    void Start()
    {
        _flagged = false;
        _revealed = false;
    }

    void OnMouseExit()
    {
        // handle player interaction in PlayerInput script
        if(!PlayerInput.IsGamePaused)
            _playerInput.OnMouseExit(_gridPosition);
    }

    private void OnMouseOver()
    {
        // handle player interaction in PlayerInput script
        if (!PlayerInput.IsGamePaused)
            _playerInput.OnMouseOver(_gridPosition);
    }

    // member functions
    public void Reveal()
    {
        // state variable
        PlayerInput.InitialClickIssued = true;

        if (this.IsMine())
        {
            //TODO: GAME OVER LOGIC
            Debug.Log("------------  STATE:  GAME OVER  ------------");
        }
        else
        {
            //_unrevealedTileCount--; //TODO: move it to gridscript
            _revealed = true;
        }

        renderer.material = Materials[_tileValue];

        Debug.Log("TILE:: Reveal Tile " + _gridPosition);
    }

    public void Conceal()
    {
        // TODO: update unrevealiedTileCount in gridscript
        _revealed = false;
        renderer.material = Materials[TILE_UNREVEALED];
    }

    public void PlaceMineOnTile()
    {
        _tileValue = TILE_MINE;
    }

    public void Highlight()
    {
        renderer.material = Materials[TILE_HIGHLIGHT];
    }

    public void RevertHighlight()
    {
        renderer.material = Materials[TILE_UNREVEALED];
    }

    public bool IsRevealed()
    {
        return _revealed;
    }

    public bool IsFlagged()
    {
        return _flagged;
    }

    public bool IsMine()
    {
        return _tileValue == TILE_MINE;
    }

    public void ToggleFlag()
    {
        _flagged = !_flagged;
        renderer.material = _flagged ? Materials[TILE_FLAGGED] : Materials[TILE_UNREVEALED];
    }
}
