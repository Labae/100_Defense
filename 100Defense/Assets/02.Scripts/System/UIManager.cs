using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject mUIWidget;

    private void Start()
    {
        if(!Initialize())
        {
            return;
        }
    }

    public bool Initialize()
    {
        WaveManager mWave = GameManager.instance.GetWaveManager();
        if(!mWave)
        {
            Debug.Log("Failed Get WaveManager.");
            return false;
        }

        return true;
    }
}
