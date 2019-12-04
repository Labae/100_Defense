using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveLabel : MonoBehaviour, IObserver
{
    private UILabel mLabel;
    private void Start()
    {
        mLabel = GetComponent<UILabel>();
        mLabel.text = "WAVE " + GameManager.Instance.GetPlayerInfo().WaveIndex.ToString();
    }

    public void OnNotify(IObservable ob)
    {
        PlayerInformation info = ob as PlayerInformation;

        if (info != null)
        {
            mLabel.text =  "WAVE " + info.WaveIndex.ToString();
        }
    }
}
