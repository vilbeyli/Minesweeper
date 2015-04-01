using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   
    //----------------------------------------
    // handles
    public GameObject TilePrefab;
    public GameObject GridPrefab;
    public UIManager UI;


    //-----------------------------------------
    // private variables
    private Transform _grid;
    List<List<Tile>> _map = new List<List<Tile>>();
    public GameSettings Settings { get; set; }

    //-----------------------------------------
    // function definitions
    void Awake()
    {
        Settings = new GameSettings();
        Settings = GameSettings.Intermediate;
    }

    void Start ()
	{
        StartNewGame();
    }

	void Update ()
	{

	}

    public void StartNewGame()
    {
        // delete current scene & instantiate new grid
        Destroy(GameObject.Find("Grid(Clone)"));
        _grid = ((GameObject)Instantiate(GridPrefab, new Vector3(0, 0, 0), Quaternion.identity)).transform;

        // build new scene with new settings
        UI.ReadSettings();    // updates the Settings property
        GenerateMap(Settings);
    }

    void GenerateMap(GameSettings settings)
    {
        _map = new List<List<Tile>>();
        for (int i = 0; i < settings.Width; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < settings.Height; j++)
            {
                Tile tile = ((GameObject)Instantiate(TilePrefab,
                                                    new Vector3(i - Mathf.Floor(settings.Width / 2), 0, -j + Mathf.Floor(settings.Height / 2)), 
                                                    Quaternion.identity
                                                    )).GetComponent<Tile>();
                tile.GridPosition = new Vector2(i, j);
                row.Add(tile);
                tile.transform.parent = _grid;
            }

            _map.Add(row);
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

    
}


// SERIALIZE FIELD ???? DOESNT WORK???
[Serializable]
public class TileOptions
{
    public int a = 0;
    public float b = 0.5f;
    public bool c = false;
};
// ????????????????????????????????????

[Serializable]
public class GameSettings
{
    // static constant settings
    public static readonly GameSettings Beginner = new GameSettings(9, 9, 10);
    public static readonly GameSettings Intermediate = new GameSettings(16, 16, 40);
    public static readonly GameSettings Expert = new GameSettings(30, 16, 99);
    
    // fields
    [SerializeField]
    private int _height;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _mines;

    public int Height
    {
        get { return _height; }
    }

    public int Width
    {
        get { return _width; }
    }

    public int Mines
    {
        get { return _mines; }
    }


    public GameSettings(int w, int h, int m)
    {
        _width = w;
        _height = h;
        _mines = m;
    }

    public GameSettings()
    {
    }

    // member functions
    public void Set(int w, int h, int m)
    {
        _width = w;
        _height = h;
        _mines = m;
    }
}