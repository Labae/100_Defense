using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    /// <summary>
    /// 맵 클래스.
    /// </summary>
    private MapManager mMap;
    /// <summary>
    /// 메인 카메라.
    /// </summary>
    private Camera mMainCamera;
    /// <summary>
    /// Cell의 LayerMask
    /// </summary>
    private int mCellLayerMask;

    #region Method
    /// <summary>
    /// InputManager 초기화 함수,
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 마우스 이벤트 처리 함수.
    /// </summary>
    public void MouseEvent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickCell();
        }
    }

    /// <summary>
    /// Cell 클릭.
    /// </summary>
    private void ClickCell()
    {
        if (mMap == null)
        {
            mMap = GetComponent<GameManager>().GetMapManager();
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

    #region Test Method
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

    private void DestroyTower()
    {
        if (mMap.GetSelectedCell() != null)
        {
            mMap.GetSelectedCell().DestoryTower();
        }
    }
    #endregion
    #endregion
}
