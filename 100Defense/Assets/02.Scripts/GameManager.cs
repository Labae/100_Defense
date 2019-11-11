using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject mMapPrefab;
    private MapManager mMap;

    private void Start()
    {
        if(!Initialize())
        {
            Debug.Log("Failed GameManager Initialize.");
            return;
        }

        StartCoroutine(InitializeAnim());
    }

    private bool Initialize()
    {
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

        if(!mMap.Initialize())
        {
            Debug.Log("Failed Initialize Map Component.");
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
