using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    //-----------------------------------------
    // public variables
    public GameObject TilePrefab;

    public int MapWidth = 11;
    public int MapHeight = 11;

    //-----------------------------------------
    // private variables
    List<List<Tile>> map = new List<List<Tile>>();

    private GameObject _grid;

    void Awake()
    {
        _grid = GameObject.Find("Grid");
    }

	// Use this for initialization
	void Start () 
    {
	    GenerateMap();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void GenerateMap()
    {
        map = new List<List<Tile>>();
        for (int i = 0; i < MapWidth; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < MapHeight; j++)
            {
                Tile tile = ((GameObject)Instantiate(TilePrefab, 
                                                    new Vector3(i - Mathf.Floor(MapWidth/2), 0, -j + Mathf.Floor(MapHeight/2)), 
                                                    Quaternion.identity
                                                    )).GetComponent<Tile>();
                tile.gridPosition = new Vector2(i, j);
                row.Add(tile);
                tile.transform.parent = _grid.transform;
            }

            map.Add(row);
        }
    }
}
