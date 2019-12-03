using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellClass : MonoBehaviour, IHeapItem<CellClass>
{
    public enum CellState
    {
        EDefault,
        EStart,
        EGoal,
        ERoad,
        ESelected,
        End
    };

    private bool mApperance;

    private MapManager mMap;
    [SerializeField]
    private CellState mState;
    private Material[] mMaterials;
    private MeshRenderer mMeshRenderer;
    private TowerClass mTower;
    private CellState mPrevState;

    private int mCellIndexX;
    private int mCellIndexY;

    private bool mWalkable;
    private CellClass mParent;
    private int mGCost;
    private int mHCost;
    private int mHeapIndex;

    public bool Initialize(int x, int y, string data)
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

        if (data == "0")
        {
            mTower = null;
        }
        else
        {
            mTower = CreateTower(data);
            if (!mTower.Initialize(this, data))
            {
                Debug.Log("Failed Tower Initialize");
                return false;
            }
            mWalkable = false;
        }

        return true;
    }

    public void ApperanceAnimation()
    {
        if (!mApperance)
        {
            mApperance = true;
            StopCoroutine(ApperanceAnimationCoroutine());
            StartCoroutine(ApperanceAnimationCoroutine());
        }
    }

    public void Anim()
    {
        StartCoroutine(ApperanceAnimationCoroutine());
    }

    private void OnClick()
    {
        if (mState == CellState.EStart || mState == CellState.EGoal)
        {
            return;
        }

        if (mMap.GetCanClick())
        {
            if (GameManager.Instance.GetWaveManager().GetIsWaveEnd() && mState == CellState.ERoad)
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
    }

    public void ReleaseSelected()
    {
        if (mState == CellState.ESelected)
        {
            mState = mPrevState;
            mMeshRenderer.material = mMaterials[(int)mPrevState];
        }
    }

    public int GetCellX()
    {
        return mCellIndexX;
    }

    public int GetCellY()
    {
        return mCellIndexY;
    }

    public TowerClass GetTower()
    {
        return mTower;
    }

    public MapManager GetMap()
    {
        return mMap;
    }

    private TowerClass CreateTower(string towerName)
    {
        GameObject towerObject = new GameObject(towerName);
        towerObject.transform.position = Vector3.zero;
        TowerClass tower = towerObject.AddComponent<TowerClass>();
        mMap.AddTower(tower);

        return tower;
    }

    public bool BuildTower(TowerType type)
    {
        if (mTower != null)
        {
            return false;
        }
        else
        {
            mTower = CreateTower(GetTowerName(type));
            if (!mTower.Build(this, type))
            {
                Debug.Log("Failed Tower Initialize.");
                return false;
            }

            mWalkable = false;

            mMap.GetPathFinding().PathFind();
            if (!mMap.GetPathFinding().GetPathSuccess())
            {
                mTower.Destroyimmediately(this);
                mWalkable = true;
                return false;
            }

            return true;
        }
    }

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
            mMap.GetPathFinding().PathFind();
            return true;
        }
    }

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

    public int CompareTo(CellClass cellToCompare)
    {
        int compare = FCost.CompareTo(cellToCompare.FCost);
        if (compare == 0)
        {
            compare = mHCost.CompareTo(cellToCompare.mHCost);
        }

        return -compare;
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

    public string GetTowerName(TowerType type)
    {
        switch (type)
        {
            case TowerType.ID_TOWER01:
                return "ID_TOWER01";
            case TowerType.ID_TOWER02:
                return "ID_TOWER02";
            default:
                return string.Empty;
        }
    }
}
