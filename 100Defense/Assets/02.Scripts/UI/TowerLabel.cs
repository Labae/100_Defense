using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLabel : MonoBehaviour, IObserver
{
    private TowerData mTowerData;
    private UILabel mLabel;

    public void Initialize(TowerData towerData)
    {
        mLabel = GetComponent<UILabel>();
        mTowerData = towerData;
        if(!GameManager.Instance.GetPlayerInfo().ContainTowerData.ContainsKey(mTowerData))
        {
            Debug.Log("Cannot find mTowerData in TowerData");
            return;
        }
        mLabel.text = GameManager.Instance.GetPlayerInfo().ContainTowerData[mTowerData].ToString();
    }

    public void OnNotify(IObservable ob)
    {
        PlayerInformation info = ob as PlayerInformation;
        if (info != null)
        {
            mLabel.text = info.ContainTowerData[mTowerData].ToString();
        }
    }
}
