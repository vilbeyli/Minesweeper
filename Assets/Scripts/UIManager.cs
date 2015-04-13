using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // handles
    public GameManager GM;
    public MusicManager MM;
    public Text TimerText;
    public Text FlagText;
    public Text TimeScaleText;
    public Text GameStateText;  // won/lost

    private InputField  _heightInput;
    private InputField  _widthInput;
    private InputField  _minesInput;
    private Button      _newGameButton;
    private Text        _sliderHeader;
    private Slider      _musicSlider;

    private bool _isCustom;
    private GameSettings _UI_GameSettings;

    // ----------------------------------------------
    // Functions
    void Awake()
    {
        // Get the handles manually
        _sliderHeader   = GameObject.Find("Difficulty_Value_Text").GetComponent<Text>();
        _widthInput     = GameObject.Find("Width_Input").GetComponent<InputField>();
        _heightInput    = GameObject.Find("Height_Input").GetComponent<InputField>();
        _minesInput     = GameObject.Find("Mines_Input").GetComponent<InputField>();
        _musicSlider    = GameObject.Find("Music_Slider").GetComponent<Slider>();
        _newGameButton = GameObject.Find("Button_NewGame").GetComponent<Button>();

        if (_sliderHeader == null || _widthInput == null || _heightInput == null || 
            _minesInput == null || _musicSlider == null || _newGameButton == null)
            Debug.Log("UIMANAGER:: ERROR! NULL HANDLES!");
    }

	// Use this for initialization
	void Start ()
    {
        // initial update
	    _UI_GameSettings = GM.Settings;
	    WriteSettingsToInputText(_UI_GameSettings);  
	}

    void Update()
    {
        TimeScaleText.text = Time.timeScale.ToString();
    }

    //-----------------------------------------------------------
    // Game Options Function Definitions
    public void OptionSliderUpdate(float val)
    {
        int sliderValue = (int) val;    // slider has whole numbers --> safe to cast int

        switch (sliderValue)
        {
            case 0:     // beginner settings
                _sliderHeader.text = "Beginner";
                _UI_GameSettings = GameSettings.Beginner;
                break;

            case 1:     // intermediate settings
                _sliderHeader.text = "Intermediate";
                _UI_GameSettings = GameSettings.Intermediate;
                break;

            case 2:     // expert settings
                _sliderHeader.text = "Expert";
                _UI_GameSettings = GameSettings.Expert;
                break;
            case 3:
                _sliderHeader.text = "Custom";
                break;
        }

        WriteSettingsToInputText(_UI_GameSettings);

        // Set Custom Settings according to slider value. Read funct description
        SetCustomSettings(sliderValue == 3);

        //Debug.Log("UIMANAGER::SLIDERUPDATE val=" + sliderValue);
    }

    // Sets the interactability of input fields: if(3) true, else false;
    void SetCustomSettings(bool isCustom)
    {
        // set interactability
        _widthInput.interactable = isCustom;
        _heightInput.interactable = isCustom;
        _minesInput.interactable = isCustom;
        _isCustom = isCustom;

        //Debug.Log("MinesInput: " + _minesInput.IsInteractable());
    }

    public GameSettings ReadSettings()
    {
        if (_isCustom)
        {
            int w = Int32.Parse(_widthInput.text);
            int h = Int32.Parse(_heightInput.text);
            int m = Int32.Parse(_minesInput.text);
            _UI_GameSettings = new GameSettings(w, h, m);
        }

        return _UI_GameSettings;
    }

    void WriteSettingsToInputText(GameSettings settings)
    {
        if (settings == null)
        {
            Debug.Log("Settings NULL");
            return;
        }
        _widthInput.text = settings.Width.ToString();
        _heightInput.text = settings.Height.ToString();
        _minesInput.text = settings.Mines.ToString();
    } // called from ReadSettings()

    //-----------------------------------------------------------
    // Music Settings Function Definitions
    public void MusicSliderUpdate(float val)
    {
        MM.SetVolume(val);
    }

    public void MusicToggle(bool val)
    {
        _musicSlider.interactable = val;
        MM.SetVolume(val ? _musicSlider.value : 0f);
    }

    //-----------------------
    // Debug UI
    public void RevealTiles()
    {
        GameObject.Find("Grid(Clone)").GetComponent<GridScript>().RevealAllTiles();
    }

    public void ConcealTiles()
    {
        GameObject.Find("Grid(Clone)").GetComponent<GridScript>().ConcealAllTiles();  
    }

    //-------------------------------------
    // Scores area

    public void UpdateTimeText(int time)
    {
        TimerText.text = "Timer: ";
        if (time < 10)
        {
            TimerText.text += "00" + time;
        }
        else if (time < 100)
        {
            TimerText.text += "0" + time;
        }
        else if (time < 1000)
        {
            TimerText.text += time.ToString();
        }
    }

    public void UpdateFlagText(int flagCount)
    {
        FlagText.text = "Flags: ";
        if (flagCount < 10)
        {
            FlagText.text += "00" + flagCount;
        }
        else if (flagCount < 100)
        {
            FlagText.text += "0" + flagCount;
        }

    }
}
