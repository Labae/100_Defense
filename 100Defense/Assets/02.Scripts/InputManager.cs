using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mCamera;
    private Map mMap;

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

            hit.transform.GetComponent<Cell>().Click();
        }
    }
}
