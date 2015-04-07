using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //-------------------------------------------------------------
    // Variable Declarations 

    // static variabels
    private static int  _unrevealedTileCount;

    // handles
    private GridScript _grid;
    private PlayerInput _playerInput;

    // private variables
    private Vector2         _gridPosition = Vector2.zero;   // set when GridScript::GenerateMap() is called
    private List<Vector2>   _neighborTilePositions;         // set when GridScript::SetNeighbors() is called
    private bool            _revealed;                      // set when OnMouseOver() is called
    private int             _tileValue;                     // # mines nearby, -1 if mine is on the tile

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
    public static int UnrevealedTileCount
    {
        get { return _unrevealedTileCount; }
    }


    // unity functions
    void Awake()
    {
        _playerInput = GameObject.Find("GameManager").GetComponent<PlayerInput>();
    }
    
    void Start()
    {
        _unrevealedTileCount++;
    }

    void OnMouseExit()
    {
        // handle player interaction in PlayerInput script
        _playerInput.OnMouseExit(_gridPosition);
    }

    private void OnMouseOver()
    {
        // handle player interaction in PlayerInput script
        _playerInput.OnMouseOver(_gridPosition);
    }

    // member functions
    public string Coordinates()
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

    public bool IsRevealed()
    {
        return _revealed;
    }

}
