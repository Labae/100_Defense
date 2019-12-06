using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    #region SerializeField Vale
    /// <summary>
    /// Setting Panel GameObject
    /// </summary>
    [SerializeField] private GameObject mSettingPanel;
    /// <summary>
    /// TouchGuard Panel
    /// </summary>
    [SerializeField] private UIPanel mTouchGuard;
    /// <summary>
    /// Wave Label
    /// </summary>
    [SerializeField] private WaveLabel mWaveLabel;
    /// <summary>
    /// Coin Label
    /// </summary>
    [SerializeField] private CoinLabel mCoinLabel;
    /// <summary>
    /// Life Manager
    /// </summary>
    [SerializeField] private LifeManager mLifeSet;
    /// <summary>
    /// Store Grid
    /// </summary>
    [SerializeField] private UIGrid mStoreGrid;
    /// <summary>
    /// UI Store Panel
    /// </summary>
    [SerializeField] private UIPanel mUIStorePanel;
    /// <summary>
    /// Tower Buy Panel Class
    /// </summary>
    [SerializeField] private TowerBuyPanel mTowerBuyPanel;
    #endregion

    #region Private Value
    /// <summary>
    /// Store ScrollView
    /// </summary>
    private UIScrollView mStoreScrollView;
    /// <summary>
    /// Wave Manager
    /// </summary>
    private WaveManager mWaveManager;
    /// <summary>
    /// Player Information
    /// </summary>
    private PlayerInformation mPlayerInfo;
    /// <summary>
    /// 회전하는 3d 타워 오브젝트들.
    /// </summary>
    private readonly List<UITowerRotation> uiTowerRotations = new List<UITowerRotation>();
    /// <summary>
    /// 이 값은 상점에서 3개의 타워 UI만 회전하게 하기 위한 값.
    /// </summary>
    private const float mOffsetX = 350.0f;
    /// <summary>
    /// Store Panel Offset X 값.
    /// </summary>
    private float mPanelOffsetX;
    #endregion

    #region Unity Function
    private void Start()
    {
        if(!Initialize())
        {
            return;
        }
    }

    private void Update()
    {
        VisibleTowerUIRotation();
    }
    #endregion

    #region Method
    /// <summary>
    /// UI manager 초기화 함수.
    /// </summary>
    /// <returns></returns>
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

        Tower towerData = GameManager.Instance.GetMap().GetTowerData();

        mStoreGrid.repositionNow = true;
        for (int i = 0; i < towerData.dataArray.Length; i++)
        {
            GameObject setTower = Resources.Load("01.Prefabs/UI/Set Tower") as GameObject;
            if(setTower == null)
            {
                return false;
            }
            GameObject uiSetTower = Instantiate(setTower, mStoreGrid.transform);

            TowerLabel towerLabel = uiSetTower.GetComponentInChildren<TowerLabel>();
            towerLabel.Initialize(towerData.dataArray[i]);

            mPlayerInfo.AddObserver(towerLabel);

            string modelName = towerData.dataArray[i].Modelname;
            GameObject towerSet = Resources.Load("01.Prefabs/UI/3D_Model/" + modelName) as GameObject;
            if(towerSet == null)
            {
                return false;
            }

            GameObject tower = Instantiate(towerSet, uiSetTower.transform);
            tower.transform.GetChild(0).localScale = tower.transform.GetChild(0).localScale * 250.0f;
            uiTowerRotations.Add(tower.GetComponent<UITowerRotation>());
        }

        mStoreGrid.Reposition();
        mStoreScrollView = mUIStorePanel.GetComponent<UIScrollView>();
        if(mStoreScrollView == null)
        {
            Debug.Log("Failed Get UIScrollView");
            return false;
        }

        for (int i = 0; i < 3; i++)
        {
            uiTowerRotations[i].RotateTower();
        }

        UIButton[] buyButtons = mStoreGrid.GetComponentsInChildren<UIButton>();

        for (int i = 0; i < buyButtons.Length; i++)
        {
            EventDelegate eventBtn = new EventDelegate(this, "OpenTowerBuyPanel");
            eventBtn.parameters[0].value = towerData.dataArray[i];
            buyButtons[i].onClick.Add(eventBtn);
        }

        mTouchGuard.gameObject.SetActive(false);

        return true;
    }

    /// <summary>
    /// 현재 보이는 타워 UI만 회전하게 만드는 함수.
    /// </summary>
    private void VisibleTowerUIRotation()
    {
        if (mPanelOffsetX != mUIStorePanel.clipOffset.x)
        {
            mPanelOffsetX = mUIStorePanel.clipOffset.x;
            int value = Mathf.RoundToInt(mPanelOffsetX / mOffsetX) - 1;
            for (int i = 0; i < uiTowerRotations.Count; i++)
            {
                if (i >= value && i < value + 3)
                {
                    uiTowerRotations[i].RotateTower();
                }
                else
                {
                    uiTowerRotations[i].StopRotateTower();
                }
            }
        }
    }
    #endregion

    #region Button Event Method
    /// <summary>
    /// Setting Panel 열기.
    /// </summary>
    public void OpenSettingPanel()
    {
        mTouchGuard.gameObject.SetActive(true);
        mSettingPanel.transform.DOLocalMoveX(0.0f, 0.25f).SetEase(Ease.InCirc);
        mTouchGuard.depth = mSettingPanel.GetComponent<UIPanel>().depth;
    }

    /// <summary>
    /// Setting Panel 닫기.
    /// </summary>
    public void CloseSettingPanel()
    {
        mTouchGuard.gameObject.SetActive(false);
        mSettingPanel.transform.DOLocalMoveX(1000.0f, 0.25f).SetEase(Ease.InCirc);
        mTouchGuard.depth = 0;
    }

    /// <summary>
    /// Wave Start 버튼.
    /// </summary>
    public void WaveStart()
    {
        if(!mWaveManager)
        {
            return;
        }

        if(!mWaveManager.WaveStart())
        {
            Debug.Log("Failed Wave to start");
            return;
        }
    }

    public void OpenTowerBuyPanel(TowerData towerData)
    {
        mTouchGuard.gameObject.SetActive(true);
        mTowerBuyPanel.SetData(towerData);
        mTowerBuyPanel.transform.DOLocalMoveX(0.0f, 0.25f).SetEase(Ease.InCirc);
        mTouchGuard.depth = mTowerBuyPanel.GetComponent<UIPanel>().depth;
    }

    public void CloseTowerBuyPanel()
    {
        mTouchGuard.gameObject.SetActive(false);
        mTowerBuyPanel.transform.DOLocalMoveX(1000.0f, 0.25f).SetEase(Ease.InCirc);
        mTouchGuard.depth = 0;
    }
    #endregion
}
