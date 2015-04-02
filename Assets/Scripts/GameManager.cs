using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   
    //----------------------------------------
    // handles
    public GameObject GridPrefab;
    public UIManager UI;
    private Grid _grid;


    //-----------------------------------------
    // private variables
    private Transform _gridtf;
    public GameSettings Settings { get; set; }

    //-----------------------------------------
    // function definitions

    // unity functions
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

    // member functions
    public void StartNewGame()
    {
        // delete current scene & instantiate new grid
        Destroy(GameObject.Find("Grid(Clone)"));
        _gridtf = ((GameObject)Instantiate(GridPrefab, new Vector3(0, 0, 0), Quaternion.identity)).transform;
        _grid = _gridtf.GetComponent<Grid>();
        if(_grid == null) Debug.Log("_grid IS NULL!!");

        // build new scene with new settings
        UI.ReadSettings();              // updates the Settings property
        _grid.GenerateMap(Settings);    // grid manager "_grid" generates the map with given settings
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

        Debug.Log("GAMEMANAGER:: TimeScale: " + Time.timeScale);
    }
  
}


// SERIALIZE FIELD ???? DOESNT WORK???
[System.Serializable]
public class TileOptions
{
    [SerializeField]
    public int a;
    [SerializeField]
    public float b = 0.5f;
    [SerializeField]
    public bool c = false;
};
// ????????????????????????????????????

[System.Serializable]
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