using System;
using UnityEngine;


public class Tile : MonoBehaviour
{
    private Vector2 _gridPosition = Vector2.zero;
    private bool _revealed;

    public Vector2 GridPosition
    {
        get { return _gridPosition; }
        set { _gridPosition = value; }
    }

    private static bool _rightAndLeftIssued;


    void OnMouseOver()
    {
        //---------------
        // Mouse logic:
        // - Left&Right:    if(tile is revealed)        Reveal Neighbors
        // - Left click:    if(not after Left&Right)    Reveal Tile
        // - Right click:   if(not after Left&Right)    Flag Tile

        // left click released
        if (Input.GetMouseButtonUp(0))
        {
            if (Input.GetMouseButton(1))    // if right click is held as well
            {
                if (_revealed)
                {
                    // CARE FLAG LOGIC HERE!!!
                    Debug.Log("TILE:: Reveal Neightbors of Tile " + Vec2toText(_gridPosition));
                }
                else
                {
                    Debug.Log("TILE:: Ineffective Simultaneous click(0up) on Unrevealed Tile " + Vec2toText(_gridPosition));
                }
                _rightAndLeftIssued = true;
            }
            else  // if only left click is released
            {
                if (_rightAndLeftIssued)
                    _rightAndLeftIssued = false;
                else
                {
                    Debug.Log("TILE:: Reveal Tile " + Vec2toText(_gridPosition));
                }

            }
        }

        // right click released
        if (Input.GetMouseButtonUp(1))
        {
            if (Input.GetMouseButton(0)) // if left click is held as well
            {
                _rightAndLeftIssued = true;
                if (_revealed)
                {
                    // CARE FLAG LOGIC HERE!!!
                    Debug.Log("TILE:: Reveal Neightbors of Tile " + Vec2toText(_gridPosition));
                }
                else
                {
                    Debug.Log("TILE:: Ineffective Simultaneous click(1up) on Unrevealed Tile " + Vec2toText(_gridPosition));
                }

            }
            else // if only right click is released
            {
                if (_rightAndLeftIssued)
                    _rightAndLeftIssued = false;
                else
                {
                    Debug.Log("TILE:: Flag the Tile " + Vec2toText(_gridPosition));
                }
            }
        }
    }


    string Vec2toText(Vector2 v)
    {
        return String.Format("(" + v.x + ", " + v.y + ")");
    }

    public void Reveal()
    {
        _revealed = true;
        renderer.material.color = Color.cyan;
    }
}
