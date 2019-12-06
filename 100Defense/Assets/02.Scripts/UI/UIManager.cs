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
    [SerializeField] private UIGrid mStoreGrid;
    [SerializeField] private UIPanel mUIStorePanel;
    private UIScrollView mStoreScrollView;
    private WaveManager mWaveManager;

    private PlayerInformation mPlayerInfo;

    private readonly List<UITowerRotation> uiTowerRotations = new List<UITowerRotation>();
    private const float mOffsetX = 350.0f;

    private void Start()
    {
        if(!Initialize())
        {
            return;
        }
    }

    private void Update()
    {
        if(mStoreScrollView.isDragging)
        {
            float offsetX = mUIStorePanel.clipOffset.x;
            int value = Mathf.RoundToInt(offsetX / mOffsetX) - 1;
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

        Tower towerData = Resources.Load("03.Datas/Game/TowerData") as Tower;
        if (!towerData)
        {
            Debug.Log("Tower data not load");
            return false;
        }


        mStoreGrid.repositionNow = true;
        for (int i = 0; i < towerData.dataArray.Length; i++)
        {
            GameObject setTower = Resources.Load("01.Prefabs/UI/Set Tower") as GameObject;
            if(setTower == null)
            {
                return false;
            }
            GameObject uiSetTower = Instantiate(setTower, mStoreGrid.transform);

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

        if(!mWaveManager.WaveStart())
        {
            Debug.Log("Failed Wave to start");
            return;
        }
    }
}
