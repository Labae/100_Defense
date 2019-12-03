using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private MapManager mMap;
    private Wave mWaveData;
    private WaitForSeconds mWFSNextEnemySpawnTime;
    private int mWaveIndex;
    private bool mIsWaveEnd;
    private bool mIsWaveStart;
    private bool mIsWaving;

    public bool Initialize(MapManager map)
    {
        mWaveData = Resources.Load("03.Datas/Game/WaveData") as Wave;
        if (!mWaveData)
        {
            Debug.Log("Failed load wave data");
            return false;
        }

        mWaveIndex = 0;
        mMap = map;
        mIsWaveEnd = false;
        mWFSNextEnemySpawnTime = new WaitForSeconds(1.0f);

        return true;
    }

    public void WaveStart()
    {
        if (!mIsWaveStart)
        {
            if (mWaveIndex >= mWaveData.dataArray.Length)
            {
                return;
            }
            mMap.SetSelectedCell(null);

            string enemyKey = mWaveData.dataArray[mWaveIndex].Enemykey;
            int waveCount = mWaveData.dataArray[mWaveIndex].COUNT;
            mIsWaveStart = true;
            mWaveIndex++;

            List<TowerClass> towers = mMap.GetTowers();
            for (int i = 0; i < towers.Count; i++)
            {
                Canon canon = towers[i].GetComponentInChildren<Canon>();
                if (!canon)
                {
                    continue;
                }

                canon.SetAttackTimerZero();
            }

            StartCoroutine(CreateEnemyCoroutine(enemyKey, mMap.GetPathFinding().GetPath(), waveCount));
        }
    }

    private IEnumerator CreateEnemyCoroutine(string enemyKey, List<Vector3> path, int waveNumber)
    {
        for (int i = 0; i < waveNumber; i++)
        {
            GameObject enemyObject = new GameObject();
            EnemyClass enemy = enemyObject.AddComponent<EnemyClass>();

            enemy.Initialize(mMap, enemyKey, path);

            yield return mWFSNextEnemySpawnTime;
        }
        mIsWaveStart = false;
    }

    public bool GetIsWaving()
    {
        mIsWaveEnd = mMap.GetmEnemies().Count > 0 ? false : true;

        if(!mIsWaveStart && mIsWaveEnd)
        {
            mIsWaving = false;
        }

        if(mIsWaveStart && !mIsWaveEnd)
        {
            mIsWaving = true;
        }

        return mIsWaving;
    }

    public int GetWaveIndex()
    {
        return mWaveIndex;
    }

}
