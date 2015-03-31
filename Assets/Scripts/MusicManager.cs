using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public void SetVolume(float val)
    {
        GetComponent<AudioSource>().volume = val;
    }
}
