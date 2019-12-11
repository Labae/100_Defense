using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private MapManager mMap;
    private Wave mWaveData;
    private Enemy mEnemyData;
    private WaitForSeconds mWFSNextEnemySpawnTime;
    private bool mIsWaveStart;
    private bool mIsWaving;
    private bool mIsUpdatePath;

    public bool Initialize(MapManager map)
    {
        mWaveData = Resources.Load("03.Datas/Game/WaveData") as Wave;
        if (!mWaveData)
        {
            Debug.Log("Failed load wave data");
            return false;
        }

        mEnemyData = Resources.Load("03.Datas/Game/EnemyData") as Enemy;
        if (!mEnemyData)
        {
            Debug.Log("Enemy data not load");
            return false;
        }

        mMap = map;
        mWFSNextEnemySpawnTime = new WaitForSeconds(1.0f);

        return true;
    }

    public void Loop()
    {
        if (!GetIsWaving() && !mIsUpdatePath)
        {
            mIsUpdatePath = true;
            mMap.GetPathFinding().PathFind();
        }
    }

    public bool WaveStart()
    {
        if (!mMap.GetPathFinding().GetPathSuccess())
        {
            return false;
        }

        if (!mIsWaveStart)
        {
            int waveIndex = GameManager.Instance.GetPlayerInfo().WaveIndex;
            if (waveIndex >= mWaveData.dataArray.Length)
            {
                return false;
            }
            mMap.SetSelectedCell(null);

            string enemyKey = mWaveData.dataArray[waveIndex].Enemykey;
            int waveCount = mWaveData.dataArray[waveIndex].COUNT;
            mIsWaveStart = true;
            GameManager.Instance.GetPlayerInfo().WaveIndex++;

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

            StartCoroutine(CreateEnemyCoroutine(enemyKey, mMap.GetPathFinding().GetPath(), waveCount, mEnemyData.dataArray[waveIndex]));
            mIsUpdatePath = false;

            return true;
        }

        return false;
    }

    private IEnumerator CreateEnemyCoroutine(string enemyKey, List<Vector3> path, int waveNumber, EnemyData enemyData)
    {
        if (path == null)
        {
            yield break;
        }

        for (int i = 0; i < waveNumber; i++)
        {
            if (GameManager.Instance.GetIsGameOver())
            {
                mIsWaveStart = false;
                yield break;
            }
            GameObject enemyObject = new GameObject();
            EnemyClass enemy = enemyObject.AddComponent<EnemyClass>();

            enemy.Initialize(mMap, enemyKey, path, enemyData);

            yield return mWFSNextEnemySpawnTime;
        }
        mIsWaveStart = false;
    }

    public bool GetIsWaving()
    {
        bool isWaveEnd = mMap.GetEnemies().Count > 0 ? false : true;

        if (!mIsWaveStart && isWaveEnd)
        {
            mIsWaving = false;
        }

        if (mIsWaveStart && !isWaveEnd)
        {
            mIsWaving = true;
        }

        return mIsWaving;
    }
}
