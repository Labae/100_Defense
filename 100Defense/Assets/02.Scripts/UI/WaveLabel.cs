using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveLabel : MonoBehaviour, IObserver
{
    /// <summary>
    /// 웨이브를 나타내는 텍스트.
    /// </summary>
    private UILabel mLabel;

    #region Unity Event
    private void Start()
    {
        mLabel = GetComponent<UILabel>();
        mLabel.text = "WAVE " + GameManager.Instance.GetPlayerInfo().WaveIndex.ToString();
    }
    #endregion

    #region IObserver
    public void OnNotify(IObservable ob)
    {
        PlayerInformation info = ob as PlayerInformation;

        if (info != null)
        {
            mLabel.text =  "WAVE " + info.WaveIndex.ToString();
        }
    }
    #endregion
}
