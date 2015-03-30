using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour
{
    public static GameSettings beginner = new GameSettings(9, 9, 10);
    public static GameSettings intermediate = new GameSettings(16, 16, 40);
    public static GameSettings expert = new GameSettings(16, 30, 99);
    
    [SerializeField]
    private int _height;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _mines;

    public GameSettings(int h, int w, int m)
    {
        _height = h;
        _width = w;
        _mines = m;
    }

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

    public void Set(GameSettings settings)
    {
        _height = settings.Height;
        _width = settings.Width;
        _mines = settings.Mines;
    }

    public void Set(int h, int w, int m)
    {
        _height = h;
        _width = w;
        _mines = m;
    }
}
