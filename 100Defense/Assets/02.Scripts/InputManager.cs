using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mCamera;
    private MapManager mMap;
    private EnemyClass mEnemy;

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

        if(Input.GetKeyDown(KeyCode.E))
        {
            CreateEnemy(0, 1);
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

    private void CreateEnemy(int enemyIndex, int waveNumber)
    {
        if(!mMap)
        {
            mMap = GetComponent<GameManager>().GetMap();
        }
        StartCoroutine(CreateEnemyCoroutine(enemyIndex, mMap.GetPathFinding().GetPath(), waveNumber));
    }

    private IEnumerator CreateEnemyCoroutine(int enemyIndex, List<Vector3> path, int waveNumber)
    {
        WaitForSeconds wfsTime = new WaitForSeconds(1.0f);

        for (int i = 0; i < waveNumber; i++)
        {
            GameObject enemy = new GameObject();
            mEnemy = enemy.AddComponent<EnemyClass>();

            mEnemy.Initialize(enemyIndex, path);

            yield return wfsTime;
        }
    }
}
