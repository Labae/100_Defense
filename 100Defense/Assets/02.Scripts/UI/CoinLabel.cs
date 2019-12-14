using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLabel : MonoBehaviour, IObserver
{
    /// <summary>
    /// 가지고 있는 코인을 보여주는 Label
    /// </summary>
    private UILabel mLabel;

    #region Unity Event
    private void Start()
    {
        mLabel = GetComponent<UILabel>();
        mLabel.text = GameManager.Instance.GetPlayerInfo().Gold.ToString();
    }
    #endregion

    #region IObserver
    public void OnNotify(IObservable ob)
    {
        PlayerInformation info = ob as PlayerInformation;

        if(info != null)
        {
            mLabel.text = info.Gold.ToString();
        }
    }
    #endregion
}
