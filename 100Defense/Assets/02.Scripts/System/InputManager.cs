using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private MapManager mMap;
    private Camera mMainCamera;

    private int mCellLayerMask;

    public bool Initlaize(MapManager map)
    {
        mMap = map;
        mMainCamera = Camera.main;
        if (!mMainCamera)
        {
            Debug.Log("Failed Find Main Camera");
            return false;
        }

        mCellLayerMask = mMap.GetCellLayerMask();
        return true;
    }

    public void MouseEvent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickCell();
        }
    }

    public void KeyboardEvent()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DestroyTower();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mMap.Save();

        }
    }

    private void ClickCell()
    {
        if (mMap == null)
        {
            mMap = GetComponent<GameManager>().GetMap();
        }

        if (!mMap.GetIsFinishedApperanceMapAnim())
        {
            return;
        }

        if (!mMap.GetCanClick())
        {
            return;
        }

        if (UICamera.hoveredObject != null)
        {
            if (UICamera.hoveredObject.name != "UI Root")
            {
                return;
            }
        }

        Ray ray = mMainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mCellLayerMask))
        {
            if (hit.transform == null)
            {
                Debug.Log("hit is null");
                return;
            }
            hit.transform.GetComponent<CellClass>().Click();
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
