using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLabel : MonoBehaviour, IObserver
{
    private UILabel mLabel;
    private void Start()
    {
        mLabel = GetComponent<UILabel>();
        mLabel.text = GameManager.Instance.GetPlayerInfo().Gold.ToString();
    }

    public void OnNotify(IObservable ob)
    {
        PlayerInformation info = ob as PlayerInformation;

        if(info != null)
        {
            mLabel.text = info.Gold.ToString();
        }
    }
}
