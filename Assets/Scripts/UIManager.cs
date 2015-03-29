using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameManager GM;

	// Use this for initialization
	void Start ()
	{

        //Debug.Log("UIMANAGER: ON START()");
	}
	
	// Update is called once per frame
	void Update () 
    {
	    ScanForInput();

        // TODO: READ ABOUT HOW TO READ VALUES FROM UI SLIDER COMPONENT (ON VALUE CHANGE() ? )
        //       ARRANGE PLACEHOLDER FOR INPUT FIELDS
        //       CHANGE INPUT FIELD TEXT VALUE AND INTERACTABILITY
        switch ((int)GetComponentInChildren<Slider>().value)
	    {
            case 0:     // beginner settings
                GM.GetComponent<GameSettings>().Set(9, 9, 10);

	            break;

            case 1:     // intermediate settings
                GM.GetComponent<GameSettings>().Set(9, 9, 10);
	            break;

            case 2:     // expert settings
                GM.GetComponent<GameSettings>().Set(9, 9, 10);
	            break;

            case 3:     // custom settings

	            break;
	    }
	    
    }

    void ScanForInput()
    {
        if (Input.GetKeyDown("escape"))     GM.TogglePauseMenu();
    }

}
