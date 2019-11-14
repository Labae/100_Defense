using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private InputManager mInput;
    private GameObject mMapPrefab;
    private MapManager mMap;
    private CSVManager mCSV;
    private WaveManager mWave;

    private void Start()
    {
        if(!Initialize())
        {
            Debug.Log("Failed GameManager Initialize.");
            return;
        }

        StartCoroutine(InitializeAnim());
    }

    private void Update()
    {
        if (mInput)
        {
            mInput.MouseEvent();
            mInput.KeyboardEvent();
        }
        if (mMap)
        {
            mMap.TowerUpdate();
        }
    }

    private bool Initialize()
    {
        mCSV = gameObject.AddComponent<CSVManager>();
        if (!mCSV)
        {
            Debug.Log("Failed Get CSV Component.");
            return false;
        }

        GameObject map = Resources.Load("01.Prefabs/Map") as GameObject;
        if(!map)
        {
            Debug.Log("Failed Load Map Prefab.");
            return false;
        }

        mMapPrefab = Instantiate<GameObject>(map, Vector3.zero, Quaternion.identity);
        if(!mMapPrefab)
        {
            Debug.Log("Failed Instantiate Map Prefab.");
            return false;
        }

        mMap = mMapPrefab.GetComponent<MapManager>();
        if(!mMap)
        {
            Debug.Log("Failed GetComponent Map.");
            return false;
        }

        if (!mMap.Initialize(mCSV))
        {
            Debug.Log("Failed Initialize Map Component.");
            return false;
        }

        mInput = gameObject.AddComponent<InputManager>();
        if (!mInput)
        {
            Debug.Log("Failed Add InputManager Component");
            return false;
        }

        if (!mInput.Initlaize(mMap))
        {
            Debug.Log("Failed Initialize InputManager Component");
            return false;
        }

        mWave = gameObject.AddComponent<WaveManager>();
        if(!mWave)
        {
            Debug.Log("Failed Add WaveManager Component");
            return false;
        }

        if(!mWave.Initialize(mMap))
        {
            Debug.Log("Failed Initialize WaveManager Component");
            return false;
        }

        return true;
    }

    private IEnumerator InitializeAnim()
    {
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(mMap.MapAnimCoroutine());

        yield return new WaitForSeconds(0.5f);
        mMap.GetPathFinding().PathFind();

        yield return new WaitForSeconds(0.5f);
        mMap.SetCanClick(true);
    }

    public MapManager GetMap()
    {
        return mMap;
    }
}
