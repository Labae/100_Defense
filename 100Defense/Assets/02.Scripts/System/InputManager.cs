using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mCamera;
    private Camera mUICamera;
    private MapManager mMap;
    private WaveManager mWave;

    public bool Initlaize(MapManager map)
    {
        mMap = map;
        mCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (!mCamera)
        {
            Debug.Log("Failed Get Camera Component");
            return false;
        }

        mUICamera = FindObjectOfType<UICamera>().GetComponent<Camera>();
        if (mUICamera == null)
        {
            Debug.Log("Failed Get UICamera Component");
            return false;
        }

        return true;
    }

    public void MouseEvent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CellClick();
        }
    }

    public void KeyboardEvent()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuildTower();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DestroyTower();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mMap.Save();
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (!mWave)
            {
                mWave = GetComponent<WaveManager>();
            }

            if(mMap.GetPathFinding().GetPathSuccess() == false)
            {
                return;
            }

            mWave.WaveStart();
        }
    }

    private void CellClick()
    {
        if (mMap == null)
        {
            mMap = GetComponent<GameManager>().GetMap();
        }

        if (!mMap.GetIsFinishedCellsAnim())
        {
            return;
        }

        if(!mMap.GetCanClick())
        {
            return;
        }

        RaycastHit hit;
        Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
        Ray ray2 = mUICamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray2, out hit, Mathf.Infinity, LayerMask.NameToLayer("UI")))
        {
            if(hit.transform != null)
            {
                return;
            } 
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mMap.GetLayerMask()))
        {
            if (hit.transform == null)
            {
                Debug.Log("Cell Click is null");
                return;
            }

            hit.transform.GetComponent<CellClass>().Click();
        }
    }

    private void BuildTower()
    {
        if (mMap.GetSelectedCell() != null)
        {
            mMap.GetSelectedCell().BuildTower(TowerType.ID_TOWER01);
        }
    }

    private void DestroyTower()
    {
        if (mMap.GetSelectedCell() != null)
        {
            mMap.GetSelectedCell().DestoryTower();
        }
    }
}
