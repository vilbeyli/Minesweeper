using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIDebug : MonoBehaviour {

    // member functions
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

    public void IntensitySliderUpdate(float val)
    {
        GameObject.Find("intensity_text").GetComponent<Text>().text = "Intensity: " + val;
        GridScript grid = GameObject.Find("Grid(Clone)").GetComponent<GridScript>();

        foreach (List<Tile> row in grid.Map)
        {
            foreach (Tile tile in row)
            {
                if (tile.GetComponentInChildren<Light>() != null)
                    tile.GetComponentInChildren<Light>().intensity = val;
                else
                {
                    Debug.Log("NULL LIGHT");
                    return;
                }
            }
        }


    }

    public void LightRangeSliderUpdate(float val)
    {
        GameObject.Find("range_text").GetComponent<Text>().text = "Range: " + val;
        GridScript grid = GameObject.Find("Grid(Clone)").GetComponent<GridScript>();

        foreach (List<Tile> row in grid.Map)
        {
            foreach (Tile tile in row)
            {
                if (tile.GetComponentInChildren<Light>() != null)
                    tile.GetComponentInChildren<Light>().range = val;
                else
                {
                    Debug.Log("NULL LIGHT");
                    return;
                }
            }

        }

    }
}
