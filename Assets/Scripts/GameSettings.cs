using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    private int _height;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _mines;


    public int Height
    {
        get { return _height; }
        set { _height = value; }
    }

    public int Width
    {
        get { return _width; }
        set { _width = value; }
    }

    public int Mines
    {
        get { return _mines; }
        set { _mines = value; }
    }
}
