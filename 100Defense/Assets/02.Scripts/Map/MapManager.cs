using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region Private Value
    /// <summary>
    /// CellClass로 만든 맵.
    /// </summary>
    private CellClass[,] mMap;
    /// <summary>
    /// Read Csv Map Data
    /// </summary>
    private string[,] mMapData;
    /// <summary>
    /// 선택된 Cell
    /// </summary>
    private CellClass mSelectedCell;
    /// <summary>
    /// 시작 Cell
    /// </summary>
    private CellClass mStartCell;
    /// <summary>
    /// 골인 지점 Cell
    /// </summary>
    private CellClass mGoalCell;

    /// <summary>
    /// 현재 맵에 있는 타워 List
    /// </summary>
    private List<TowerClass> mTowers;
    /// <summary>
    /// 현재 맵에 있는 적 List
    /// </summary>
    private List<EnemyClass> mEnemies;
    /// <summary>
    /// 맵 사이즈
    /// </summary>
    private int mMapSizeX;
    private int mMapSizeY;

    /// <summary>
    /// Cell 출현 애니메이션 대기 시간.
    /// </summary>
    private WaitForSeconds mWFSCellApperanceSquareAnimTime;
    /// <summary>
    /// Cell Start Goal 애니메이션 시작 시간.
    /// </summary>
    private WaitForSeconds mWFSCellStartGoalAnimTime;
    /// <summary>
    /// Cell의 애니메이션이 끝났는지 확인.
    /// </summary>
    private bool mIsFinishedApperanceMapAnim;

    /// <summary>
    /// Path Finding Class
    /// </summary>
    private PathFinding mPathFind;
    /// <summary>
    /// Csv Class
    /// </summary>
    private CSVManager mCSV;

    /// <summary>
    /// Cell을 클릭할 수 있는지.
    /// </summary>
    private bool mCanClick;
    #endregion

    #region Method
    public bool Initialize(CSVManager csv, Vector2 mapSize)
    {
        mMapSizeX = (int)mapSize.x;
        mMapSizeY = (int)mapSize.y;
        mMap = new CellClass[mMapSizeX, mMapSizeY];
        if (mMap.Length <= 0)
        {
            Debug.Log("Failed Initialize mCell.");
            return false;
        }

        mCSV = csv;
        mMapData = mCSV.LoadMap(mMapSizeX, mMapSizeY);
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

        // Make Map
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

        // Set Start, End Cell
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

    public void Save()
    {
        mCSV.MapSave(mMapData);
        mCSV.SavePlayerInfo(GameManager.Instance.GetPlayerInfo());
    }
    #endregion

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
        // 짝수
        if(mMapSizeX % 2 == 0)
        {
            for (int animCount = 0; animCount < mMapSizeX / 2; animCount++)
            {
                int index = mMapSizeX / 2 + -(animCount + 1);
                int maxIndex = mMapSizeX / 2 + animCount + 1;

                if (index < 0)
                {
                    yield break;
                }
                if (maxIndex > mMapSizeX)
                {
                    yield break;
                }

                for (int x = index; x < maxIndex; x++)
                {
                    for (int y = index; y < maxIndex; y++)
                    {
                        mMap[x, y].ApperanceAnimation();
                        mMap[y, x].ApperanceAnimation();
                    }
                }
                yield return mWFSCellApperanceSquareAnimTime;
            }
        }
        // 홀수
        else
        {
            // Center
            mMap[mMapSizeX / 2, mMapSizeY / 2].ApperanceAnimation();

            yield return mWFSCellApperanceSquareAnimTime;

            for (int animCount = 0; animCount < mMapSizeX / 2; animCount++)
            {
                int index = mMapSizeX / 2 + - (animCount + 1);
                int maxIndex = mMapSizeX / 2 + animCount + 2;

                if (index < 0)
                {
                    yield break;
                }
                if (maxIndex > mMapSizeX)
                {
                    yield break;
                }

                for (int x = index; x < maxIndex; x++)
                {
                    for (int y = index; y < maxIndex; y++)
                    {
                        mMap[x, y].ApperanceAnimation();
                        mMap[y, x].ApperanceAnimation();
                    }
                }
                yield return mWFSCellApperanceSquareAnimTime;
            }
        }

        mIsFinishedApperanceMapAnim = true;
    }

    private IEnumerator CellStartGoalAnimationCoroutine()
    {
        mStartCell.Anim();
        mGoalCell.Anim();
        yield return mWFSCellStartGoalAnimTime;
    }

    #endregion

    #region Get Set SelectedCell
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
    #endregion

    #region Get Map Size
    public int GetMapSizeX()
    {
        return mMapSizeX;
    }
    public int GetMapSizeY()
    {
        return mMapSizeY;
    }

    public int GetMaxSize()
    {
        return mMapSizeX * mMapSizeY;
    }

    #endregion

    public int GetCellLayerMask()
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

    public bool GetIsFinishedApperanceMapAnim()
    {
        return mIsFinishedApperanceMapAnim;
    }

    #region Get PathFinding
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
}
