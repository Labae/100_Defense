using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellClass : MonoBehaviour
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
    private bool mCanClick;

    private MapManager mMap;
    private CellState mState;
    private Material[] mMaterials;
    private MeshRenderer mMeshRenderer;
    private TowerClass mTower;

    private int mCellIndexX;
    private int mCellIndexY;

    private string mCellData;

    public bool Initialize(int x, int y, MapData data)
    {
        mCellIndexX = x;
        mCellIndexY = y;

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

        mMaterials = new Material[(int)CellState.End];
        mMaterials[(int)CellState.EDefault] = Resources.Load("02.Materials/CellDefault") as Material;
        if (!mMaterials[(int)CellState.EDefault])
        {
            Debug.Log("Failed Load CellDefault Material.");
            return false;
        }
        mMaterials[(int)CellState.EStart] = Resources.Load("02.Materials/CellStart") as Material;
        if (!mMaterials[(int)CellState.EStart])
        {
            Debug.Log("Failed Load CellStart Material.");
            return false;
        }
        mMaterials[(int)CellState.EGoal] = Resources.Load("02.Materials/CellGoal") as Material;
        if (!mMaterials[(int)CellState.EGoal])
        {
            Debug.Log("Failed Load CellGoal Material.");
            return false;
        }
        mMaterials[(int)CellState.ESelected] = Resources.Load("02.Materials/CellSelected") as Material;
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

        mCellData = GetCellData(data);

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

    public void Click()
    {
        if (mState == CellState.EStart || mState == CellState.EGoal)
        {
            return;
        }

        if (mCanClick)
        {
            if (mState == CellState.ESelected)
            {
                mMap.SetSelectedCell(null);
            }
            else
            {
                mState = CellState.ESelected;
                mMeshRenderer.material = mMaterials[(int)CellState.ESelected];
                mMap.SetSelectedCell(this);
            }

            mCanClick = false;
            StopCoroutine(ApperanceAnimationCoroutine());
            StartCoroutine(ApperanceAnimationCoroutine());
        }
    }

    public void ReleaseSelected()
    {
        if (mState == CellState.ESelected)
        {
            mState = CellState.EDefault;
            mMeshRenderer.material = mMaterials[(int)CellState.EDefault];
        }
    }

    private string GetCellData(MapData data)
    {
        if (mCellIndexX == 0)
        {
            return data.X0;
        }
        else if (mCellIndexX == 1)
        {
            return data.X1;
        }
        else if (mCellIndexX == 2)
        {
            return data.X2;
        }
        else if (mCellIndexX == 3)
        {
            return data.X3;
        }
        else if (mCellIndexX == 4)
        {
            return data.X4;
        }
        else if (mCellIndexX == 5)
        {
            return data.X5;
        }
        else if (mCellIndexX == 6)
        {
            return data.X6;
        }
        else if (mCellIndexX == 7)
        {
            return data.X7;
        }
        else if (mCellIndexX == 8)
        {
            return data.X8;
        }
        else if (mCellIndexX == 9)
        {
            return data.X9;
        }
        else
        {
            return string.Empty;
        }
    }

    private TowerClass CreateTower(string towerName)
    {
        GameObject towerObject = new GameObject(towerName);
        towerObject.transform.position = Vector3.zero;
        towerObject.AddComponent<TowerClass>();

        return towerObject.GetComponent<TowerClass>();
    }

    private IEnumerator ApperanceAnimationCoroutine()
    {
        float angle = 180.0f;
        float speed = 600.0f;

        while (angle <= 540.0f)
        {
            angle += Time.deltaTime * speed;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * 0.3f;

            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        mCanClick = true;

        if (mCellData == "0")
        {
            mTower = null;
        }
        else
        {
            mTower = CreateTower(mCellData);
            if (!mTower.Initialize(this, mCellData))
            {
                Debug.Log("Failed Tower Initialize");
            }
        }
    }
}
