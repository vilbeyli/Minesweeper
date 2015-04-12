using UnityEngine;

public class GameManager : MonoBehaviour
{
   
    //----------------------------------------
    // Variable Declarations
    
    // handles
    public GameObject GridPrefab;
    public UIManager UI;
    private GridScript _grid;

    // private variables
    private Transform _gridtf;
    private GameSettings _settings;

    // score variables
    private float _startTime;
    private float _endTime;
    private int _flagCount;

    private Score score;

    


    //-----------------------------------------
    // Function Definitions

    // getters & setters
    public GameSettings Settings
    {
        get { return _settings; }
        set { _settings = value; }
    }

    // unity functions
    void Awake()
    {
        _settings = new GameSettings();
        _settings = GameSettings.Intermediate;

    }

    void Start ()
	{
        StartNewGame(_settings);
    }

    private void Update()
    {
        UI.UpdateFlagText(_flagCount);
        if (PlayerInput.InitialClickIssued)
        {
            UI.UpdateTimeText((int) (Time.time - _startTime));
        }
    }

    // member functions
    public void NewGameButton()
    {
        GameSettings settings = UI.ReadSettings();
        
        if(settings.isValid())
            StartNewGame(settings);
        else
        {
            Debug.Log("INVALID SETTINGS!");
            return;
        }
    }

    public void RestartButton()
    {
        StartNewGame(_settings);
    }

    public void StartNewGame(GameSettings settings)
    {
        // delete current grid in the scene & instantiate new grid
        Destroy(GameObject.Find("Grid(Clone)"));
        _gridtf = ((GameObject)Instantiate(GridPrefab, new Vector3(0, 0, 0), Quaternion.identity)).transform;
        _grid = _gridtf.GetComponent<GridScript>();
        if (_grid == null) Debug.Log("_grid IS NULL!!");

        _settings = settings;
        _grid.GenerateMap(_settings);    // grid manager "_grid" generates the map with given settings


        // update handles
        GetComponent<PlayerInput>().Grid = _grid;

        // close menu
        UI.GetComponentInChildren<Canvas>().enabled = false;
        PlayerInput.IsGamePaused = false;
        Time.timeScale = 1f;
        //Debug.Log("PLAYERINPUT:: TimeScale=" + Time.timeScale);

        // reset stats and UI
        PlayerInput.InitialClickIssued = false;
        _flagCount = _settings.Mines;
        UI.UpdateFlagText(_flagCount);
        UI.UpdateTimeText(0);
    }

    public void StartTimer()
    {
        _startTime = Time.time;
    }

    public void GameOver(bool win)
    {
        _endTime = Time.time - _startTime;
        Debug.Log("GAME ENDED IN " + (_endTime - _startTime) + " SECONDS. GAME WON:" + win);
        
        // TODO: GAMEOVER STATE - MENU?
        GetComponent<PlayerInput>().TogglePauseMenu();

        if (win)
        {
            int timePassed = (int)(_endTime - _startTime);
            score = new Score(timePassed, Settings);

            // TODO: HIGHSCORES if score in top 10, ask user input, put on leaderboard
        }
    }

    public void PutFlag()
    {
        _flagCount--;
    }

    public void RemoveFlag()
    {
        _flagCount++;
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

    public bool isValid()
    {
        if (_width <= 0 || _height <= 0 || _mines <= 0)
            return false;
        if (_mines >= _width*_height)
            return false;
        return true;
    }
}

[System.Serializable]
public class Score
{
    private GameSettings _settings;
    private int _timePassed;
    private string _name;

    public GameSettings Settings
    {
        get { return _settings; }
    }

    public int TimePassed
    {
        get { return _timePassed; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Score(int timePassed, GameSettings settings)
    {
        _timePassed = timePassed;
        _settings = settings;
    }

    public Score(GameSettings settings, int timePassed, string name)
    {
        _settings = settings;
        _timePassed = timePassed;
        _name = name;
    }

}