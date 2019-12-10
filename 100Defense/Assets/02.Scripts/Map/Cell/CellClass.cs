using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellClass : MonoBehaviour, IHeapItem<CellClass>
{
    /// <summary>
    /// Cell의 상태들.
    /// </summary>
    public enum CellState
    {
        EDefault,
        EStart,
        EGoal,
        ERoad,
        ESelected,
        End
    };

    #region SerializeFiled Value
    /// <summary>
    /// 현재 Cell의 상태.
    /// </summary>
    [SerializeField]
    private CellState mState;
    #endregion

    #region Private Value
    /// <summary>
    /// Cell이 소환되었는지.
    /// </summary>
    private bool mApperance;
    /// <summary>
    /// Map Manager
    /// </summary>
    private MapManager mMap;
    /// <summary>
    /// Cell의 상태에 따른 Material들.
    /// </summary>
    private Material[] mMaterials;
    /// <summary>
    /// Cell의 MeshRenderer.
    /// </summary>
    private MeshRenderer mMeshRenderer;
    /// <summary>
    /// Cell이 가지고 있는 TowerClass. 없으면 null
    /// </summary>
    private TowerClass mTower;
    /// <summary>
    /// 이전 상태의 Cell 상태.
    /// </summary>
    private CellState mPrevState;
    /// <summary>
    /// Map에서 Cell Index값.
    /// </summary>
    private int mCellIndexX;
    private int mCellIndexY;
    /// <summary>
    /// AStar Algorithm Value
    /// </summary>
    private bool mWalkable;
    private CellClass mParent;
    private int mGCost;
    private int mHCost;
    private int mHeapIndex;

    #endregion

    #region Property
    public int HeapIndex
    {
        get
        {
            return mHeapIndex;
        }
        set
        {
            mHeapIndex = value;
        }
    }

    public int FCost
    {
        get
        {
            return mGCost + mHCost;
        }
    }
    #endregion

    #region Method
    /// <summary>
    /// Cell Class Initialize
    /// </summary>
    /// <param name="x"> cell index x</param>
    /// <param name="y"> cell index y</param>
    /// <param name="towerData">tower data</param>
    /// <returns></returns>
    public bool Initialize(int x, int y, string towerData)
    {
        mCellIndexX = x;
        mCellIndexY = y;
        mWalkable = true;

        mMap = GetComponentInParent<MapManager>();
        if (!mMap)
        {
            Debug.Log("Failed Initialize Map Component");
            return false;
        }

        if (x == 0 && y == 0)
        {
            mState = CellState.EStart;
        }
        else if (x == mMap.GetMapSizeX() - 1 && y == mMap.GetMapSizeY() - 1)
        {
            mState = CellState.EGoal;
        }
        else
        {
            mState = CellState.EDefault;
        }

        string matPath = "02.Materials/01.Cells/";

        mMaterials = new Material[(int)CellState.End];
        mMaterials[(int)CellState.EDefault] = Resources.Load(matPath + "CellDefault") as Material;
        if (!mMaterials[(int)CellState.EDefault])
        {
            Debug.Log("Failed Load CellDefault Material.");
            return false;
        }
        mMaterials[(int)CellState.EStart] = Resources.Load(matPath + "CellStart") as Material;
        if (!mMaterials[(int)CellState.EStart])
        {
            Debug.Log("Failed Load CellStart Material.");
            return false;
        }
        mMaterials[(int)CellState.EGoal] = Resources.Load(matPath + "CellGoal") as Material;
        if (!mMaterials[(int)CellState.EGoal])
        {
            Debug.Log("Failed Load CellGoal Material.");
            return false;
        }
        mMaterials[(int)CellState.ERoad] = Resources.Load(matPath + "CellRoad") as Material;
        if (!mMaterials[(int)CellState.ERoad])
        {
            Debug.Log("Failed Load CellRoad Material.");
            return false;
        }
        mMaterials[(int)CellState.ESelected] = Resources.Load(matPath + "CellSelected") as Material;
        if (!mMaterials[(int)CellState.ESelected])
        {
            Debug.Log("Failed Load CellSelected Material.");
            return false;
        }

        mMeshRenderer = GetComponent<MeshRenderer>();
        if (!mMeshRenderer)
        {
            Debug.Log("Failed Initialize mMeshRenderer Component");
            return false;
        }

        if (mMaterials[(int)mState] == null)
        {
            Debug.Log("Failed Found Material in mMaterials Array.");
            return false;
        }
        mMeshRenderer.material = mMaterials[(int)mState];

        if (towerData == "0")
        {
            mTower = null;
        }
        else
        {
            mTower = CreateTower(towerData);
            if (!mTower.Initialize(towerData))
            {
                Debug.Log("Failed Tower Initialize");
                return false;
            }
            mWalkable = false;
        }

        return true;
    }
    /// <summary>
    /// 출현 애니메이션.
    /// </summary>
    public void ApperanceAnimation()
    {
        if (!mApperance)
        {
            mApperance = true;
            StopCoroutine(ApperanceAnimationCoroutine());
            StartCoroutine(ApperanceAnimationCoroutine());
        }
    }
    /// <summary>
    /// Cell 애니메이션.
    /// </summary>
    public void Anim()
    {
        StartCoroutine(ApperanceAnimationCoroutine());
    }
    /// <summary>
    /// Cell Click Event Method
    /// </summary>
    public void Click()
    {
        if (mState == CellState.EStart || mState == CellState.EGoal)
        {
            return;
        }

        if (GameManager.Instance.GetWaveManager().GetIsWaving() && mState == CellState.ERoad)
        {
            return;
        }

        if (mState == CellState.ESelected)
        {
            mMap.SetSelectedCell(null);
        }
        else
        {
            mPrevState = mState;
            mState = CellState.ESelected;
            mMeshRenderer.material = mMaterials[(int)CellState.ESelected];
            mMap.SetSelectedCell(this);
        }

        StopCoroutine(ApperanceAnimationCoroutine());
        StartCoroutine(ApperanceAnimationCoroutine());
    }
    /// <summary>
    /// 선택된 Cell상태를 해제함.
    /// </summary>
    public void ReleaseSelected()
    {
        if (mState == CellState.ESelected)
        {
            mState = mPrevState;
            mMeshRenderer.material = mMaterials[(int)mPrevState];
        }
    }

    /// <summary>
    /// Create Tower Class
    /// </summary>
    /// <param name="towerName"></param>
    /// <returns></returns>
    private TowerClass CreateTower(string towerName)
    {
        GameObject towerObject = GameManager.Instance.GetObjectPool().SpawnTowerFromPool(towerName, transform);
        TowerClass tower = GetComponent<TowerClass>();
        if (tower == null)
        {
            tower = towerObject.AddComponent<TowerClass>();
        }
        mMap.AddTower(tower);

        return tower;
    }

    /// <summary>
    /// Build Tower
    /// </summary>
    /// <param name="type"></param>
    public bool BuildTower(string type)
    {
        if (mTower == null)
        {
            mWalkable = false;

            if(!GameManager.Instance.GetWaveManager().GetIsWaving())
            {
                mMap.GetPathFinding().PathFind();
                if (!mMap.GetPathFinding().GetPathSuccess())
                {
                    mWalkable = true;
                    return false;
                }
            }

            mTower = CreateTower(type);
            if (!mTower.Build(this, type))
            {
                Debug.Log("Failed Tower Initialize.");
                return false;
            }

            GameManager.Instance.GetPlayerInfo().Gold -= mTower.GetPrice();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Destory Tower
    /// </summary>
    /// <returns></returns>
    public bool DestoryTower()
    {
        if (mTower == null)
        {
            return false;
        }
        else
        {
            mTower.DestroyTower(this);
            mWalkable = true;
            mMap.SetSelectedCell(null);
            mTower = null;

            if(!GameManager.Instance.GetWaveManager().GetIsWaving())
            {
                mMap.GetPathFinding().PathFind();
            }
            return true;
        }
    }

    /// <summary>
    ///  Coroutine 출현 애니메이션
    /// </summary>
    /// <returns></returns>
    private IEnumerator ApperanceAnimationCoroutine()
    {
        float angle = 180.0f;
        float speed = 1000.0f;

        while (angle <= 540.0f)
        {
            angle += Time.deltaTime * speed;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * 0.3f;

            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
    }

    public int CompareTo(CellClass cellToCompare)
    {
        int compare = FCost.CompareTo(cellToCompare.FCost);
        if (compare == 0)
        {
            compare = mHCost.CompareTo(cellToCompare.mHCost);
        }

        return -compare;
    }

    #endregion

    #region Get Set
    /// <summary>
    /// Get Cell index X
    /// </summary>
    public int GetCellX()
    {
        return mCellIndexX;
    }

    /// <summary>
    /// Get Cell index y
    /// </summary>
    public int GetCellY()
    {
        return mCellIndexY;
    }

    /// <summary>
    /// Get Tower Class
    /// </summary>
    public TowerClass GetTower()
    {
        return mTower;
    }

    /// <summary>
    /// Get Map Class
    /// </summary>
    public MapManager GetMap()
    {
        return mMap;
    }

    public bool GetWalkable()
    {
        return mWalkable;
    }

    public int GetGCost()
    {
        return mGCost;
    }

    public void SetGCost(int cost)
    {
        mGCost = cost;
    }

    public void SetHCost(int cost)
    {
        mHCost = cost;
    }

    public void SetParent(CellClass parent)
    {
        mParent = parent;
    }

    public CellClass GetParent()
    {
        return mParent;
    }

    public void SetWalkable(bool walkable)
    {
        mWalkable = walkable;
    }

    public CellState GetState()
    {
        return mState;
    }

    public void SetState(CellState state)
    {
        if (state == CellState.EStart || state == CellState.EGoal)
        {
            return;
        }
        mState = state;
        mMeshRenderer.material = mMaterials[(int)state];
    }
    #endregion
}
