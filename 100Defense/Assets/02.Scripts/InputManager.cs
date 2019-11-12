using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mCamera;
    private MapManager mMap;

    private void Start()
    {
        mCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CellClick();
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            BuildTower();
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            DestoryTower();
        }
    }

    private void CellClick()
    {
        if (mMap == null)
        {
            mMap = GetComponent<GameManager>().GetMap();
        }

        if(!mMap.GetIsFinishedCellsAnim())
        {
            return;
        }

        RaycastHit hit;
        Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);

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

    private void DestoryTower()
    {
        if (mMap.GetSelectedCell() != null)
        {
            mMap.GetSelectedCell().DestoryTower();
        }
    }
}
