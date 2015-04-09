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

    void Awake()
    {
        // Get the handles manually
        _sliderHeader   = GameObject.Find("Difficulty_Text").GetComponent<Text>();
        _widthInput     = GameObject.Find("Width_Input").GetComponent<InputField>();
        _heightInput    = GameObject.Find("Height_Input").GetComponent<InputField>();
        _minesInput     = GameObject.Find("Mines_Input").GetComponent<InputField>();
        _musicSlider    = GameObject.Find("Music_Slider").GetComponent<Slider>();

        if (_sliderHeader == null || _widthInput == null || _heightInput == null || _minesInput == null || _musicSlider == null)
            Debug.Log("UIMANAGER:: ERROR! NULL HANDLES!");
    }

	// Use this for initialization
	void Start ()
    {
        // initial update
	    WriteSettingsToInputText(GM.Settings);  
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
                GM.Settings = GameSettings.Beginner;
                break;

            case 1:     // intermediate settings
                _sliderHeader.text = "Intermediate";
                GM.Settings = GameSettings.Intermediate;
                break;

            case 2:     // expert settings
                _sliderHeader.text = "Expert";
                GM.Settings = GameSettings.Expert;
                break;
            case 3:
                _sliderHeader.text = "Custom";
                break;
        }

        WriteSettingsToInputText(GM.Settings);

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
    }

    public void ReadSettings()
    {
        int w = System.Int32.Parse(_widthInput.text);
        int h = System.Int32.Parse(_heightInput.text);
        int m = System.Int32.Parse(_minesInput.text);
        GM.Settings.Set(w, h, m);
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

    public void RevealTiles()
    {
        GameObject.Find("Grid(Clone)").GetComponent<GridScript>().RevealAllTiles();
    }

    public void ConcealTiles()
    {
        GameObject.Find("Grid(Clone)").GetComponent<GridScript>().ConcealAllTiles();  
    }
}
