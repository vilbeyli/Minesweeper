using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager GM;
    public MusicManager MM;

    private InputField _heightInput;
    private InputField _widthInput;
    private InputField _minesInput;
    private Text _sliderHeader;
    private Slider _musicSlider;

	// Use this for initialization
	void Start ()
	{
        //--------------------------------------------------------------------------
        // Game Settings Related Code
        // Get the handles manually
        _sliderHeader   = GameObject.Find("Difficulty_Text").GetComponent<Text>();
        _widthInput     = GameObject.Find("Width_Input" ).GetComponent<InputField>();
        _heightInput    = GameObject.Find("Height_Input").GetComponent<InputField>();
        _minesInput     = GameObject.Find("Mines_Input" ).GetComponent<InputField>();
	    WriteSettingsToInputText(GM.GetComponent<GameSettings>());

        if(_sliderHeader == null || _widthInput == null || _heightInput == null || _minesInput == null)
            Debug.Log("UIMANAGER:: ERROR! NULL HANDLES!");

        //Debug.Log("UIMANAGER: ON START()");

        OptionSliderUpdate(1);    // initial update

        //--------------------------------------------------------------------------
        // Music Settings Related Code
        _musicSlider = GameObject.Find("Music_Slider").GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    ScanForKeyStroke();
    }

    void ScanForKeyStroke()
    {
        if (Input.GetKeyDown("escape"))     GM.TogglePauseMenu();
    }

    //-----------------------------------------------------------
    // Game Options Function Definitions
    public void OptionSliderUpdate(float val)
    {
        int sliderValue = (int) val;    // slider has whole numbers --> safe to cast int

        // Custom Settings if sliderValue == 3
        SetCustomSettings(sliderValue == 3);    // interactability
        switch (sliderValue)
        {
            case 0:     // beginner settings
                _sliderHeader.text = "Beginner";
                GM.GetComponent<GameSettings>().Set(GameSettings.beginner);
                WriteSettingsToInputText(GameSettings.beginner);
                break;

            case 1:     // intermediate settings
                _sliderHeader.text = "Intermediate";
                GM.GetComponent<GameSettings>().Set(GameSettings.intermediate);
                WriteSettingsToInputText(GameSettings.intermediate);
                break;

            case 2:     // expert settings
                _sliderHeader.text = "Expert";
                GM.GetComponent<GameSettings>().Set(GameSettings.expert);
                WriteSettingsToInputText(GameSettings.expert);
                break;

            case 3:     // custom settings
                _sliderHeader.text = "Custom";
                break;
            default:
                Debug.Log("UIMANAGER::SLIDERUPDATE INVALID SLIDER VALUE: " + sliderValue);
                break;
        }

        //Debug.Log("UIMANAGER::SLIDERUPDATE val=" + sliderValue);
    }

    void SetCustomSettings(bool val)
    {
        _widthInput.interactable = val;
        _heightInput.interactable = val;
        _minesInput.interactable = val;
    }

    void WriteSettingsToInputText(GameSettings settings)
    {
        _widthInput.text = settings.Width.ToString();
        _heightInput.text = settings.Height.ToString();
        _minesInput.text = settings.Mines.ToString();
    }

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
}
