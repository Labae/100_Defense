using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject SettingPanel;
    public UIPanel TouchGuard;
    public WaveLabel WaveLabel;
    public CoinLabel CoinLabel;
    public LifeManager LifeSet;
    private WaveManager mWaveManager;

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

        mPlayerInfo = GameManager.Instance.GetPlayerInfo();
        mPlayerInfo.AddObserver(CoinLabel);

        mWaveManager = GameManager.Instance.GetWaveManager();
        if(!mWaveManager)
        {
            Debug.Log("Failed Get Wave Manager");
            return false;
        }

        mPlayerInfo.AddObserver(WaveLabel);
        mPlayerInfo.AddObserver(LifeSet);

        TouchGuard.gameObject.SetActive(false);

        return true;
    }

    public void OpenSettingPanel()
    {
        TouchGuard.gameObject.SetActive(true);
        SettingPanel.transform.DOLocalMoveX(0.0f, 0.25f).SetEase(Ease.InCirc);
        TouchGuard.depth = SettingPanel.GetComponent<UIPanel>().depth;
    }

    public void CloseSettingPanel()
    {
        TouchGuard.gameObject.SetActive(false);
        SettingPanel.transform.DOLocalMoveX(1000.0f, 0.25f).SetEase(Ease.InCirc);
        TouchGuard.depth = 0;
    }

    public void WaveStart()
    {
        if(!mWaveManager)
        {
            return;
        }

        mWaveManager.WaveStart();
    }
}
