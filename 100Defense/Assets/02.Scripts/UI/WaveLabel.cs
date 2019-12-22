using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

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
        StringBuilder sb = new StringBuilder();
        sb.Append("WAVE : ");
        sb.Append(GameManager.Instance.GetPlayerInfo().WaveIndex.ToString());
        mLabel.text = sb.ToString();
    }
    #endregion

    #region IObserver
    public void OnNotify(IObservable ob)
    {
        PlayerInformation info = ob as PlayerInformation;

        if (info != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("WAVE : ");
            sb.Append(info.WaveIndex.ToString());
            mLabel.text = sb.ToString();
        }
    }
    #endregion
}
