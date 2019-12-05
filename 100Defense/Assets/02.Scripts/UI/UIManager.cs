using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mSettingPanel;
    [SerializeField] private UIPanel mTouchGuard;
    [SerializeField] private WaveLabel mWaveLabel;
    [SerializeField] private CoinLabel mCoinLabel;
    [SerializeField] private LifeManager mLifeSet;
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
        mPlayerInfo.AddObserver(mCoinLabel);

        mWaveManager = GameManager.Instance.GetWaveManager();
        if(!mWaveManager)
        {
            Debug.Log("Failed Get Wave Manager");
            return false;
        }

        mPlayerInfo.AddObserver(mWaveLabel);
        mPlayerInfo.AddObserver(mLifeSet);

        mTouchGuard.gameObject.SetActive(false);

        return true;
    }

    public void OpenSettingPanel()
    {
        mTouchGuard.gameObject.SetActive(true);
        mSettingPanel.transform.DOLocalMoveX(0.0f, 0.25f).SetEase(Ease.InCirc);
        mTouchGuard.depth = mSettingPanel.GetComponent<UIPanel>().depth;
    }

    public void CloseSettingPanel()
    {
        mTouchGuard.gameObject.SetActive(false);
        mSettingPanel.transform.DOLocalMoveX(1000.0f, 0.25f).SetEase(Ease.InCirc);
        mTouchGuard.depth = 0;
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
