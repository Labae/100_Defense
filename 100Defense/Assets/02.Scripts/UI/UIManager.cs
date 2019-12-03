using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject SettingPanel;
    public UIPanel TouchGuard;
    public UILabel GoldLabel;
    private CoinLabel mCoinLabel;

    private PlayerInformation mPlayerInfo;

    private void Start()
    {
        if(!Initialize())
        {
            return;
        }
    }

    private bool Initialize()
    {
        mPlayerInfo = GameManager.Instance.GetPlayerInfo();
        if(mPlayerInfo == null)
        {
            Debug.Log("Failed Get PlayerInfo");
            return false;
        }

        mCoinLabel = GoldLabel.GetComponent<CoinLabel>();
        if(mCoinLabel == null)
        {
            Debug.Log("Failed Get CoinLabel");
            return false;
        }

        mPlayerInfo = GameManager.Instance.GetPlayerInfo();
        mPlayerInfo.AddObserver(mCoinLabel);

        return true;
    }

    public void OpenSettingPanel()
    {
        SettingPanel.transform.DOLocalMoveX(0.0f, 0.25f).SetEase(Ease.InCirc);
        TouchGuard.depth = SettingPanel.GetComponent<UIPanel>().depth;
    }

    public void CloseSettingPanel()
    {
        SettingPanel.transform.DOLocalMoveX(1000.0f, 0.25f).SetEase(Ease.InCirc);
        TouchGuard.depth = 0;
    }
}
