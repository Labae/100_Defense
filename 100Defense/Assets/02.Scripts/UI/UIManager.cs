using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
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
    /// <summary>
    ///  GameOver Panel
    /// </summary>
    [SerializeField] private GameObject mGameOverPanel;
    /// <summary>
    /// 게임 종료시 뒷 배경 색깔.
    /// </summary>
    [SerializeField] private Color mTouchGuardGameoverColor;
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
    /// 구하는 방법은 max offset - min offset / (max tower count - 3)
    /// </summary>
    private const float mOffsetX = 343.6f;
    /// <summary>
    /// Store Panel Offset X 값.
    /// </summary>
    private float mPanelOffsetX;

    /// <summary>
    /// 광고(하트추가) 버튼 콜라이더.
    /// </summary>
    private BoxCollider2D mAdBtnCollider;
    /// <summary>
    /// 구매, 파괴 버튼으로 돌아가게하는 버튼 콜라이더.
    /// </summary>
    private BoxCollider2D mReturnCollider;
    /// <summary>
    /// 광고버튼과, 돌아가는 버튼의 사라질 Rotation.
    /// </summary>
    private Vector3 mDisapperRotation;
    /// <summary>
    /// 터치 가드의 원색.
    /// </summary>
    private Color mTouchGuardOriginColor;
    /// <summary>
    /// 게임 오버 패널이 보였는지.
    /// </summary>
    private bool mIsShowGameOverPanel;
    /// <summary>
    /// UI TowrModel의 y좌표.
    /// </summary>
    private readonly float mUITowerModelYPos = -45.0f;
    #endregion

    #region Unity Function

    private void Start()
    {
        if (!Initialize())
        {
            return;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetIsGameOver() && !mIsShowGameOverPanel)
        {
            ShowGameOverPanel();
            mIsShowGameOverPanel = true;
        }

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
        mStoreGrid.gameObject.SetActive(false);

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
            tower.transform.localPosition = new Vector3(tower.transform.localPosition.x, mUITowerModelYPos, tower.transform.localPosition.z);
            UITowerRotation towerRotation = tower.AddComponent<UITowerRotation>();
            uiTowerRotations.Add(towerRotation);
            Canon canon = tower.GetComponentInChildren<Canon>();
            if (canon != null)
            {
                Destroy(canon);
            }

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

        mTouchGuardOriginColor = mTouchGuard.GetComponentInChildren<UISprite>().color;
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
                if (i >= value && i < value + 4)
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

    /// <summary>
    /// 뒤집는 애니메이션.
    /// </summary>
    /// <param name="apperObject"></param>
    /// <param name="disapperObject"></param>
    /// <param name="callback"> 완료될 때 실행할 함수 </param>
    private void FlipAnimation(GameObject apperObject, GameObject disapperObject, TweenCallback callback)
    {
        disapperObject.transform.DOLocalRotate(mDisapperRotation, 0.25f);
        apperObject.transform.DOLocalRotate(Vector3.zero, 0.25f).SetDelay(0.25f).OnComplete(callback);
    }

    /// <summary>
    /// 게임 오버 패널 보여주는 함수.
    /// </summary>
    public void ShowGameOverPanel()
    {
        mGameOverPanel.transform.DOLocalMoveX(0.0f, 1.0f);
        mTouchGuard.gameObject.SetActive(true);
        mTouchGuard.depth = mGameOverPanel.GetComponent<UIPanel>().depth - 1;
        mTouchGuard.GetComponentInChildren<UISprite>().color = mTouchGuardGameoverColor;
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
        mTouchGuard.depth = mSettingPanel.GetComponent<UIPanel>().depth - 1;
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
        if (mWaveManager)
        {
            if (!mWaveManager.WaveStart())
            {
                Debug.Log("Failed Wave to start");
            }
        }
    }

    /// <summary>
    /// 상점을 여는 버튼.
    /// </summary>
    public void StoreOpenButton()
    {
        mTowerStoreButton.SetActive(false);
        mTowerDestoryButton.SetActive(false);

        mStoreGrid.gameObject.SetActive(true);
        mUIStorePanel.transform.DOLocalMoveX(-296.0f, 0.5f).SetEase(Ease.OutBack).From(1000.0f);
        mUIStorePanel.clipOffset = new Vector2(296.0f, 0.0f);
        FlipAnimation(mReturnToBasicButton, mAdvertisementButton, delegate { mReturnCollider.enabled = true; mAdBtnCollider.enabled = false; });
    }

    /// <summary>
    /// 타워 파괴 버튼.
    /// </summary>
    public void DestoryButton()
    {
        CellClass selectedCell = GameManager.Instance.GetMapManager().GetSelectedCell();
        if (selectedCell == null)
        {
            return;
        }

        if (!selectedCell.DestoryTower())
        {
            return;
        }

        GameManager.Instance.GetMapManager().SetSelectedCell(null);
    }

    /// <summary>
    /// 기본(구매, 파괴)화면으로 돌아가는 함수.
    /// </summary>
    public void ReturnToBasic()
    {
        mStoreGrid.gameObject.SetActive(false);
        mTowerStoreButton.SetActive(true);
        mTowerStoreButton.transform.DOScale(1.0f, 0.25f).SetEase(Ease.OutBack).SetDelay(0.25f).From(0.0f);
        mTowerDestoryButton.SetActive(true);
        mTowerDestoryButton.transform.DOScale(1.0f, 0.25f).SetEase(Ease.OutBack).SetDelay(0.25f).From(0.0f);

        FlipAnimation(mAdvertisementButton, mReturnToBasicButton, delegate { mReturnCollider.enabled = false; mAdBtnCollider.enabled = true; });
    }

    /// <summary>
    /// 타워 구매 패널 여는 버튼.
    /// </summary>
    /// <param name="towerData"></param>
    /// <param name="model"></param>
    public void OpenTowerBuyPanel(TowerData towerData, GameObject model)
    {
        mTouchGuard.gameObject.SetActive(true);
        mTowerBuyPanel.SetData(towerData, model, this);
        mTowerBuyPanel.transform.DOLocalMoveX(0.0f, 0.25f).SetEase(Ease.InCirc);
        mTouchGuard.depth = mTowerBuyPanel.GetComponent<UIPanel>().depth - 1;
    }

    /// <summary>
    /// 타워 구매 패널 닫는 버튼.
    /// </summary>
    public void CloseTowerBuyPanel()
    {
        mTouchGuard.gameObject.SetActive(false);
        mTowerBuyPanel.transform.DOLocalMoveX(1000.0f, 0.25f).SetEase(Ease.InCirc);
        mTouchGuard.depth = 0;
    }

    /// <summary>
    /// 게임 종료시 다시 시작하는 버튼.
    /// </summary>
    public void RestartBtn()
    {
        if (Advertisement.IsReady())
        {
            mTouchGuard.gameObject.SetActive(true);
            mTouchGuard.depth = 999;
            var option = new ShowOptions { resultCallback = RestartResultHandle };
            Advertisement.Show(option);
            mTouchGuard.GetComponentInChildren<UISprite>().color = mTouchGuardOriginColor;
        }
    }

    /// <summary>
    /// 게임 종료 버튼.
    /// </summary>
    public void GameEndBtn()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
        SceneMove.Instance.MoveTitleScene();
    }

    /// <summary>
    /// 광고 버튼(하트 추가).
    /// </summary>
    public void AdvertisementBtn()
    {
        if(GameManager.instance.GetPlayerInfo().Life == 3)
        {
            return;
        }

        if (Advertisement.IsReady("rewardedVideo"))
        {
            mTouchGuard.gameObject.SetActive(true);
            mTouchGuard.depth = 999;
            var option = new ShowOptions { resultCallback = AdvertisementRewardHandle };
            Advertisement.Show("rewardedVideo", option);
        }
    }

    /// <summary>
    /// 광고를 보고 하트를 얻는 Handle.
    /// </summary>
    /// <param name="result"></param>
    private void AdvertisementRewardHandle(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                Debug.Log("ad was failed");
                break;
            case ShowResult.Skipped:
                mTouchGuard.gameObject.SetActive(false);
                mTouchGuard.depth = 0;
                Debug.Log("ad was skipped");
                break;
            case ShowResult.Finished:
                mTouchGuard.gameObject.SetActive(false);
                mTouchGuard.depth = 0;
                GameManager.Instance.GetPlayerInfo().Life++;
                Debug.Log("ad was finished");
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 광고를 보고 다시 시작하는 Handle.
    /// </summary>
    /// <param name="result"></param>
    private void RestartResultHandle(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                Debug.Log("ad was failed");
                break;
            case ShowResult.Skipped:
                Debug.Log("ad was skipped");
                mTouchGuard.depth = 100;
                mTouchGuard.GetComponentInChildren<UISprite>().color = Color.black;
                GameManager.Instance.SetGameState(GameManager.GameState.Restart);
                break;
            case ShowResult.Finished:
                Debug.Log("ad was finished");
                mTouchGuard.depth = 100;
                mTouchGuard.GetComponentInChildren<UISprite>().color = Color.black;
                GameManager.Instance.SetGameState(GameManager.GameState.Restart);
                break;
            default:
                break;
        }
    }
    #endregion
}
