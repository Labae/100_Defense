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
    [SerializeField] private UIPanel mTouchGuard = null;
    /// <summary>
    /// Wave Label
    /// </summary>
    [SerializeField] private WaveLabel mWaveLabel = null;
    /// <summary>
    /// Coin Label
    /// </summary>
    [SerializeField] private CoinLabel mCoinLabel = null;
    /// <summary>
    /// Life Manager
    /// </summary>
    [SerializeField] private LifeManager mLifeSet = null;
    /// <summary>
    /// Store Grid
    /// </summary>
    [SerializeField] private UIGrid mStoreGrid = null;
    /// <summary>
    /// UI Store Panel
    /// </summary>
    [SerializeField] private UIPanel mUIStorePanel = null;
    /// <summary>
    /// Tower Buy Panel Class
    /// </summary>
    [SerializeField] private TowerBuyPanel mTowerBuyPanel = null;
    /// <summary>
    /// Tower Store Button
    /// </summary>
    [SerializeField] private GameObject mTowerStoreButton;
    /// <summary>
    /// Tower Destroy Button
    /// </summary>
    [SerializeField] private GameObject mTowerDestoryButton;
    /// <summary>
    ///  Advertisement Button
    /// </summary>
    [SerializeField] private GameObject mAdvertisementButton;
    /// <summary>
    ///  ReturnToBasic Button
    /// </summary>
    [SerializeField] private GameObject mReturnToBasicButton;
    #endregion

    #region Private Value
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

    private BoxCollider2D mAdBtnCollider;
    private BoxCollider2D mReturnCollider;
    private Vector3 mDisapperRotation;
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
        if (mPlayerInfo == null)
        {
            Debug.Log("Failed Get PlayerInfo");
            return false;
        }

        mPlayerInfo = GameManager.Instance.GetPlayerInfo();
        mPlayerInfo.AddObserver(mCoinLabel);

        mWaveManager = GameManager.Instance.GetWaveManager();
        if (!mWaveManager)
        {
            Debug.Log("Failed Get Wave Manager");
            return false;
        }

        mPlayerInfo.AddObserver(mWaveLabel);
        mPlayerInfo.AddObserver(mLifeSet);
        mTouchGuard.gameObject.SetActive(false);

        Tower towerData = GameManager.Instance.GetObjectPool().GetTowerData();

        mStoreGrid.repositionNow = true;

        GameObject setTower = Resources.Load("01.Prefabs/UI/Set Tower") as GameObject;
        if (setTower == null)
        {
            return false;
        }
        for (int i = 0; i < towerData.dataArray.Length; i++)
        {
            GameObject uiSetTower = Instantiate(setTower, mStoreGrid.transform);

            string modelName = towerData.dataArray[i].Modelname;
            GameObject towerModel = Resources.Load("01.Prefabs/Tower/" + modelName) as GameObject;
            if (towerModel == null)
            {
                return false;
            }

            GameObject tower = Instantiate(towerModel, uiSetTower.transform);
            tower.transform.GetChild(0).localScale = tower.transform.GetChild(0).localScale * 250.0f;
            UITowerRotation towerRotation = tower.AddComponent<UITowerRotation>();
            uiTowerRotations.Add(towerRotation);
            Canon canon = tower.GetComponentInChildren<Canon>();
            if (canon == null)
            {
                break;
            }
            Destroy(canon);

            UIButton btn = uiSetTower.GetComponentInChildren<UIButton>();
            EventDelegate eventBtn = new EventDelegate(this, "OpenTowerBuyPanel");
            eventBtn.parameters[0].value = towerData.dataArray[i];
            eventBtn.parameters[1].value = uiTowerRotations[i].gameObject;
            btn.onClick.Add(eventBtn);

            GameManager.ChangeLayerMaskRecursively(tower.transform, "3D UI");
        }

        mStoreGrid.Reposition();

        for (int i = 0; i < 3; i++)
        {
            uiTowerRotations[i].RotateTower();
        }

        mAdBtnCollider = mAdvertisementButton.GetComponent<BoxCollider2D>();
        mReturnCollider = mReturnToBasicButton.GetComponent<BoxCollider2D>();
        mReturnCollider.enabled = false;

        mDisapperRotation = new Vector3(0.0f, 90.0f, 0.0f);
        mReturnToBasicButton.transform.DOLocalRotate(mDisapperRotation, 0.0f);

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

    private void FlipAnimation(GameObject apperObject, GameObject disapperObject)
    {
        apperObject.transform.DOLocalRotate(Vector3.zero, 0.25f).SetDelay(0.25f);
        disapperObject.transform.DOLocalRotate(mDisapperRotation, 0.25f);
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
        if(mWaveManager)
        {
            if (!mWaveManager.WaveStart())
            {
                Debug.Log("Failed Wave to start");
            }
        }
    }

    public void StoreOpenButton()
    {
        mStoreGrid.transform.DOLocalMoveX(0.0f, 0.3f).SetEase(Ease.OutBack);
        mTowerStoreButton.SetActive(false);
        mTowerDestoryButton.SetActive(false);

        FlipAnimation(mReturnToBasicButton, mAdvertisementButton);
        mReturnCollider.enabled = true;
        mAdBtnCollider.enabled = false;
    }

    public void DestoryButton()
    {
        CellClass selectedCell = GameManager.Instance.GetMap().GetSelectedCell();
       if (selectedCell == null)
        {
            return;
        }

        if(!selectedCell.DestoryTower())
        {
            return;
        }

        GameManager.Instance.GetMap().SetSelectedCell(null);
    }

    public void ReturnToBasic()
    {
        mStoreGrid.transform.DOLocalMoveX(1000.0f, 0.0f);
        mTowerStoreButton.SetActive(true);
        mTowerStoreButton.transform.DOScale(1.0f, 0.25f).SetEase(Ease.OutBack).SetDelay(0.25f).From(0.0f);
        mTowerDestoryButton.SetActive(true);
        mTowerDestoryButton.transform.DOScale(1.0f, 0.25f).SetEase(Ease.OutBack).SetDelay(0.25f).From(0.0f);

        FlipAnimation(mAdvertisementButton, mReturnToBasicButton);
        mAdBtnCollider.enabled = true;
        mReturnCollider.enabled = false;
    }

    public void OpenTowerBuyPanel(TowerData towerData, GameObject model)
    {
        mTouchGuard.gameObject.SetActive(true);
        mTowerBuyPanel.SetData(towerData, model, this);
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
