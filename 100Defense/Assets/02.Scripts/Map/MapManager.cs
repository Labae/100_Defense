using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private CellClass[,] mMap;
    private string[,] mMapData;
    private CellClass mSelectedCell;
    private CellClass mStartCell;
    private CellClass mGoalCell;

    private List<TowerClass> mTowers;
    private List<EnemyClass> mEnemies;

    private const int mMapSizeX = 10;
    private const int mMapSizeY = 10;

    private WaitForSeconds mWFSCellApperanceSquareAnimTime;
    private WaitForSeconds mWFSCellStartGoalAnimTime;
    private int mLayerMask;
    private bool mIsFinishedCellAnim;

    private PathFinding mPathFind;
    private CSVManager mCSV;

    private bool mCanClick = false;

    public bool Initialize(CSVManager csv)
    {
        mMap = new CellClass[mMapSizeX, mMapSizeY];
        if (mMap.Length <= 0)
        {
            Debug.Log("Failed Initialize mCell.");
            return false;
        }

        mCSV = csv;

        mMapData = mCSV.LoadMap();
        if (mMapData == null)
        {
            Debug.Log("Failed Load MapData.csv");
            return false;
        }

        GameObject cell = Resources.Load("01.Prefabs/Map/Cell") as GameObject;
        if (!cell)
        {
            Debug.Log("Faield Load Cell prefab");
            return false;
        }

        mTowers = new List<TowerClass>();
        mEnemies = new List<EnemyClass>();

        float offsetXY = cell.transform.localScale.x * 0.1f;
        int index = mMapSizeX / 2;

        for (int y = 0; y < mMapSizeY; y++)
        {
            GameObject line = new GameObject("Line" + y.ToString());
            line.transform.SetParent(transform);
            line.transform.localPosition = Vector3.zero;
            for (int x = 0; x < mMapSizeX; x++)
            {
                GameObject cellObj = Instantiate(cell, transform.position, Quaternion.identity);
                cellObj.name = "Cell" + "(" + x.ToString() + ", " + y.ToString() + ")";
                cellObj.transform.SetParent(line.transform);
                float posX = (x - index) * cellObj.transform.localScale.x + (x * offsetXY);
                float posY = (y - index) * cellObj.transform.localScale.y + (y * offsetXY);
                cellObj.transform.position = new Vector3(posX, 0.0f, posY);

                mMap[x, y] = cellObj.GetComponent<CellClass>();
                if (mMap[x, y] == null)
                {
                    Debug.Log("Failed GetComponent Cell.");
                    return false;
                }

                if (!mMap[x, y].Initialize(x, y, mMapData[x, y]))
                {
                    Debug.Log("Failed Initialize Cell Component.");
                    return false;
                }
            }
        }

        transform.position = transform.position + Vector3.forward * -0.5f;
        mStartCell = mMap[0, 0];
        mGoalCell = mMap[mMapSizeX - 1, mMapSizeY - 1];

        mWFSCellApperanceSquareAnimTime = new WaitForSeconds(0.05f);
        mWFSCellStartGoalAnimTime = new WaitForSeconds(0.5f);

        mPathFind = gameObject.AddComponent<PathFinding>();
        if (!mPathFind)
        {
            Debug.Log("Failed Add PathFinding Component.");
            return false;
        }

        if (!mPathFind.Initialize(this))
        {
            Debug.Log("Failed Initialize PathFind.");
        }

        return true;
    }


    #region Coroutine
    public IEnumerator MapAnimCoroutine()
    {
        yield return StartCoroutine(CellApperanceAnimationCoroutine());
        for (int x = 0; x < mMapSizeX; x++)
        {
            for (int y = 0; y < mMapSizeY; y++)
            {
                if (mMap[x, y].GetTower() != null)
                {
                    StartCoroutine(mMap[x, y].GetTower().ApperanceAnim());
                }
            }
        }
        yield return mWFSCellStartGoalAnimTime;
        yield return StartCoroutine(CellStartGoalAnimationCoroutine());
    }
    private IEnumerator CellApperanceAnimationCoroutine()
    {
        for (int x = 4; x < 6; x++)
        {
            for (int y = 4; y < 6; y++)
            {
                mMap[y, x].ApperanceAnimation();
                mMap[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 3; x < 7; x++)
        {
            for (int y = 3; y < 7; y++)
            {
                mMap[y, x].ApperanceAnimation();
                mMap[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 2; x < 8; x++)
        {
            for (int y = 2; y < 8; y++)
            {
                mMap[y, x].ApperanceAnimation();
                mMap[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 1; x < 9; x++)
        {
            for (int y = 1; y < 9; y++)
            {
                mMap[y, x].ApperanceAnimation();
                mMap[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                mMap[y, x].ApperanceAnimation();
                mMap[x, y].ApperanceAnimation();
            }
        }
        yield return mWFSCellApperanceSquareAnimTime;

        mIsFinishedCellAnim = true;
    }

    private IEnumerator CellStartGoalAnimationCoroutine()
    {
        mStartCell.Anim();
        mGoalCell.Anim();
        yield return mWFSCellStartGoalAnimTime;
    }

    #endregion

    public void SetSelectedCell(CellClass cell)
    {
        if (mSelectedCell != null)
        {
            mSelectedCell.ReleaseSelected();
        }

        mSelectedCell = cell;
    }

    public CellClass GetSelectedCell()
    {
        return mSelectedCell;
    }

    #region Get Map Size
    public int GetMapSizeX()
    {
        return mMapSizeX;
    }
    public int GetMapSizeY()
    {
        return mMapSizeY;
    }

    public int GetMapMaxSize()
    {
        return mMapSizeX * mMapSizeY;
    }

    #endregion

    public int GetLayerMask()
    {
        return 1 << LayerMask.NameToLayer("Cell");
    }

    #region Get Set CanClick

    public bool GetCanClick()
    {
        return mCanClick;
    }

    public void SetCanClick(bool click)
    {
        mCanClick = click;
    }

    #endregion

    public bool GetIsFinishedCellsAnim()
    {
        return mIsFinishedCellAnim;
    }

    #region Path Find Function
    public PathFinding GetPathFinding()
    {
        return mPathFind;
    }

    public List<CellClass> GetNeighbours(CellClass node)
    {
        List<CellClass> neighbours = new List<CellClass>();

        // Left
        if (node.GetCellX() - 1 >= 0)
        {
            neighbours.Add(mMap[node.GetCellX() - 1, node.GetCellY()]);
        }
        // Right
        if (node.GetCellX() + 1 < mMapSizeX)
        {
            neighbours.Add(mMap[node.GetCellX() + 1, node.GetCellY()]);
        }
        // Up
        if (node.GetCellY() - 1 >= 0)
        {
            neighbours.Add(mMap[node.GetCellX(), node.GetCellY() - 1]);
        }
        // Down
        if (node.GetCellY() + 1 < mMapSizeY)
        {
            neighbours.Add(mMap[node.GetCellX(), node.GetCellY() + 1]);
        }

        return neighbours;
    }

    #endregion


    #region Get Cell Function
    public CellClass[,] GetCells()
    {
        return mMap;
    }

    public CellClass GetCell(int x, int y)
    {
        return mMap[x, y];
    }

    #endregion

    #region Get Set MapData
    public void SetMapData(int x, int y, string data)
    {
        if (data == null)
        {
            data = "0";
        }
        mMapData[x, y] = data;
    }

    public string[,] GetMapData()
    {
        return mMapData;
    }

    #endregion

    public void Save()
    {
        mCSV.MapSave(mMapData);
    }


    #region Tower Function
    public void AddTower(TowerClass tower)
    {
        mTowers.Add(tower);
    }

    public void RemoveTower(TowerClass tower)
    {
        mTowers.Remove(tower);
    }

    public List<TowerClass> GetTowers()
    {
        return mTowers;
    }

    public void TowerUpdate()
    {
        for (int i = 0; i < mTowers.Count; i++)
        {
            mTowers[i].Loop(this);
        }
    }

    #endregion

    #region Enemy Function

    public void AddEnemy(EnemyClass enemy)
    {
        mEnemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyClass enemy)
    {
        mEnemies.Remove(enemy);
    }

    public List<EnemyClass> GetmEnemies()
    {
        return mEnemies;
    }


    #endregion
}
