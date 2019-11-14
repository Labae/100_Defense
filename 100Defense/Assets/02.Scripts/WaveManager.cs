using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private MapManager mMap;
    private Wave mWaveData;
    private WaitForSeconds mWFSNextEnemySpawnTime;
    private int mWaveIndex;
    private bool mIsWave;

    public bool Initialize(MapManager map)
    {
        mWaveData = Resources.Load("03.Datas/WaveData") as Wave;
        if(!mWaveData)
        {
            Debug.Log("Failed load wave data");
            return false;
        }

        mWaveIndex = 0;
        mMap = map;
        mIsWave = false;
        mWFSNextEnemySpawnTime = new WaitForSeconds(1.0f);

        return true;
    }

    public void WaveStart()
    {
        if (!mIsWave)
        {
            string enemyKey = mWaveData.dataArray[mWaveIndex].Enemykey;
            int waveCount = mWaveData.dataArray[mWaveIndex].COUNT;
            mIsWave = true;
            mWaveIndex++;
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
        mIsWave = false;
    }
}
