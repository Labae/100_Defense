using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private MapManager mMap;
    private WaveManager mWave;

    public bool Initlaize(MapManager map)
    {
        mMap = map;
        return true;
    }

    public void MouseEvent()
    {
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
