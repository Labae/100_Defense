using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    /// <summary>
    /// 맵 클래스.
    /// </summary>
    private MapManager mMap;
    /// <summary>
    /// 웨이브 데이터.
    /// </summary>
    private Wave mWaveData;
    /// <summary>
    /// 적 데이터.
    /// </summary>
    private Enemy mEnemyData;
    /// <summary>
    /// 다음 적을 스폰할 대기 시간.
    /// </summary>
    private WaitForSeconds mWFSNextEnemySpawnTime;
    /// <summary>
    /// 웨이브가 시작되었는지.
    /// </summary>
    private bool mIsWaveStart;
    /// <summary>
    /// 웨이브 중인지.
    /// </summary>
    private bool mIsWaving;
    /// <summary>
    /// 경로를 업데이트 하였는지.
    /// </summary>
    private bool mIsUpdatePath;
    /// <summary>
    /// 웨이브 시작 전 플레이어 정보.
    /// </summary>
    private PlayerInformation mPrevPlayerInfo;

    #region Method
    /// <summary>
    /// 웨이브 매니저 초기화 함수.
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
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
        mPrevPlayerInfo = new PlayerInformation();
        mIsUpdatePath = true;

        return true;
    }

    /// <summary>
    /// 웨이브 루프 함수.
    /// </summary>
    public void Loop()
    {
        if (!GetIsWaving() && !mIsUpdatePath)
        {
            if (mMap.GetPathFinding().GetIsPathInitializeAnim())
            {
                mIsUpdatePath = true;
                mMap.GetPathFinding().PathFind();
            }
        }
    }

    /// <summary>
    /// 웨이브 시작 함수.
    /// </summary>
    /// <returns></returns>
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
            PlayerInformation info = GameManager.Instance.GetPlayerInfo();
            mPrevPlayerInfo.Gold = info.Gold;
            mPrevPlayerInfo.WaveIndex = info.WaveIndex;
            mPrevPlayerInfo.Life = info.Life;
            info.WaveIndex++;

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
    #endregion

    #region Coroutine
    /// <summary>
    /// 적 생성 코루틴.
    /// </summary>
    /// <param name="enemyKey"></param>
    /// <param name="path"></param>
    /// <param name="waveNumber"></param>
    /// <param name="enemyData"></param>
    /// <returns></returns>
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
    #endregion

    #region Get
    /// <summary>
    /// 웨이브 중인지를 가져옴.
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 웨이브 시작 전 플레이어 정보 가져오기.
    /// </summary>
    /// <returns></returns>
    public PlayerInformation GetPrevPlayerInfo()
    {
        return mPrevPlayerInfo;
    }
    #endregion
}
