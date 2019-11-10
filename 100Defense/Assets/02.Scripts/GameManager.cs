using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject mMapPrefab;
    private Map mMap;
    private void Start()
    {
        if(!Initialize())
        {
            Debug.Log("Failed GameManager Initialize.");
            return;
        }
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

        mMap = mMapPrefab.GetComponent<Map>();
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

    public Map GetMap()
    {
        return mMap;
    }
}
