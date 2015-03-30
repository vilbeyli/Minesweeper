using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   
    // SERIALIZE FIELD ???? DOESNT WORK???
    [System.Serializable]
    public class TileOptions
    {
        public int a = 0;
        public float b = 0.5f;
        public bool c = false;
    };
    // ????????????????????????????????????

    //----------------------------------------
    // handles
    public GameObject TilePrefab;
    public GameObject GridPrefab;
    public UIManager UI;
    public Transform Grid;


    //-----------------------------------------
    // private variables
    List<List<Tile>> map = new List<List<Tile>>();

    //-----------------------------------------
    // function definitions
    void Start ()
	{
        //Debug.Log("GAMEMANAGER:: ON START()");
        GenerateMap(GetComponent<GameSettings>());
    }

	void Update ()
	{

	}

    void GenerateMap(GameSettings settings)
    {
        map = new List<List<Tile>>();
        for (int i = 0; i < settings.Width; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < settings.Height; j++)
            {
                Tile tile = ((GameObject)Instantiate(TilePrefab,
                                                    new Vector3(i - Mathf.Floor(settings.Width / 2), 0, -j + Mathf.Floor(settings.Height / 2)), 
                                                    Quaternion.identity
                                                    )).GetComponent<Tile>();
                tile.gridPosition = new Vector2(i, j);
                row.Add(tile);
                tile.transform.parent = Grid;
            }

            map.Add(row);
        }


        Debug.Log("GAMEMANAGER::GENERATEMAP Current Game Settings: H=" + settings.Height
                                                    + " W=" + settings.Width
                                                    + " M=" + settings.Mines
                                                     );
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
        Time.timeScale = Convert.ToSingle(UI.GetComponentInChildren<Canvas>().enabled);
        UI.GetComponentInChildren<Canvas>().enabled = !UI.GetComponentInChildren<Canvas>().enabled;

        Debug.Log("GAMEMANAGER:: TimeScale: " + Time.timeScale);
    }

    public void StartNewGame()
    {
        // delete current scene & instantiate new grid
        Destroy(GameObject.Find("Grid(Clone)"));
        Grid = ((GameObject) Instantiate(GridPrefab, new Vector3(0,0,0), Quaternion.identity)).transform;
        
        // build new scene with new settings
        GenerateMap(GetComponent<GameSettings>());
    }
}
