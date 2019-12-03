using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveLabel : MonoBehaviour
{
    private UILabel mLabel;
    void Start()
    {
        mLabel = GetComponent<UILabel>();
        mLabel.text = "WAVE : " + GameManager.Instance.GetWaveManager().GetWaveIndex().ToString();
    }

    public void UpdateWaveIndex()
    {
        mLabel.text = "WAVE : " + GameManager.Instance.GetWaveManager().GetWaveIndex().ToString();
    }
}
