using System;
using System.Collections;
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
    private const int  TILE_FALSE_FLAG = 13;
    private const int TILE_MINE_PRESSED = 14;

    // handles
    private GridScript _grid;
    private PlayerInput _playerInput;
    private GameManager GM;

    // private variables
    private Vector2         _gridPosition = Vector2.zero;   // set when GridScript::GenerateMap() is called
    private List<Vector2>   _neighborTilePositions;         // set when GridScript::SetNeighbors() is called
    private bool            _revealed;                      // set when OnMouseOver() is called
    private int             _tileValue;                     // # mines nearby, 9 if mine is on the tile
    private bool             _flagged;                      // set when ToggleMine() is called

    // public variables
    public Material[]       Materials;

    [SerializeField] 
    private Lighting Lighting;

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
        GM = _playerInput.GetComponent<GameManager>();
    }
    
    void Start()
    {
        _flagged = false;
        _revealed = false;
    }

    void Update()
    {
       
    }

    void OnMouseExit()
    {
        // handle player interaction in PlayerInput script
        if(!GameManager.IsGamePaused)
            _playerInput.OnMouseExit(this);
    }

    void OnMouseOver()
    {
        // handle player interaction in PlayerInput script
        if (!GameManager.IsGamePaused)
            _playerInput.OnMouseOver(this);
    }

    // member functions
    public void Reveal()
    {
        _revealed = true;

        // if clicked on mine
        if (this.IsMine())
        {
            //PutOutLights();
            GetComponent<Renderer>().material = Materials[TILE_MINE_PRESSED];  
            GM.Detonate(this);
            GM.GameOver(false); // end game with negative result
        }
        else
        {
            GetComponent<Renderer>().material = Materials[_tileValue];
            StartCoroutine("LightUp");
            if (_tileValue == 0)    RevealNeighbors();
        }


        if(_grid.AreAllTilesRevealed())   GM.GameOver(true);
    }

    IEnumerator LightUp()
    {
        Light l = GetComponentInChildren<Light>();
        l.intensity = 0;
        l.enabled = true;
        
        while (l.intensity < Lighting.IntensityValue)
        {
            l.intensity += Lighting.IntensityIteration;
            yield return new WaitForSeconds(0); // necessary for increased duration of lighting up
        }
    }

    IEnumerator LightDown(Tile tile)
    {
        //Debug.Log("LightDown(): " + tile.GridPosition);
        Light l = tile.GetComponentInChildren<Light>();

        while (l.intensity > 0)
        {
            l.intensity -= Lighting.IntensityIteration;
            yield return new WaitForSeconds(0); // necessary for increased duration of lighting up
        }

        //Debug.Log("LightDown(): " + tile.GridPosition + " DONE");
    }

    void PutOutLights()
    {
        foreach (List<Tile> row in _grid.Map)
        {
            foreach (Tile tile in row)
            {
                if (tile.GetComponentInChildren<Light>().enabled)
                {
                    StartCoroutine("LightDown", tile);
                }
            }
        }
    }

    void RevealNeighbors()
    {
        foreach (Vector2 pos in _neighborTilePositions)
        {
            Tile neighbor = _grid.Map[(int)pos.x][(int)pos.y];
            if (!neighbor.IsRevealed() && !neighbor.IsFlagged())
                neighbor.Reveal();
        }
    }

    public void Conceal()
    {
        _revealed = false;
        GetComponent<Renderer>().material = Materials[TILE_UNREVEALED];
    }

    public void PlaceMineOnTile()
    {
        _tileValue = TILE_MINE;
    }

    public void Highlight()
    {
        GetComponent<Renderer>().material = Materials[TILE_HIGHLIGHT];
    }

    public void RevertHighlight()
    {
        GetComponent<Renderer>().material = Materials[TILE_UNREVEALED];
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

    public bool IsNeighborsFlagged()
    {
        int remaining_flags = _tileValue;

        foreach (Vector2 pos in _neighborTilePositions)
        {
            Tile neighbor = _grid.Map[(int) pos.x][(int) pos.y];
            if (neighbor.IsFlagged())
            {
                remaining_flags--;
            }
        }

        return remaining_flags <= 0;
    }

    public void ToggleFlag()
    {
        _flagged = !_flagged;

        // tile related changes
        GetComponent<Renderer>().material = _flagged ? Materials[TILE_FLAGGED] : Materials[TILE_UNREVEALED];

        // game state
        GM.UpdateFlagCounter(_flagged);

        // animation
        if (_flagged)
        {
            StartCoroutine("LightUp");
        }
        else
        {
            GetComponentInChildren<Light>().enabled = false;
        }


    }
}

[Serializable]
class Lighting
{
    [SerializeField]
    private float _rangeIteration;
    [SerializeField]
    private float _intensityIteration;
    [SerializeField]
    private float _intensityValue;
    [SerializeField]
    private float _rangeValue;

    public float RangeIteration
    {
        get { return _rangeIteration; }
    }

    public float IntensityIteration
    {
        get { return _intensityIteration; }
    }

    public float IntensityValue
    {
        get { return _intensityValue; }
    }

    public float RangeValue
    {
        get { return _rangeValue; }
    }
}