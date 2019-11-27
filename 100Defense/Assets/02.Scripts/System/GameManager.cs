using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private InputManager mInput;
    private GameObject mMapPrefab;
    private MapManager mMap;
    private CSVManager mCSV;
    private WaveManager mWave;

    private bool mInitializeSuccess;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(Initialize());
        if (!mInitializeSuccess)
        {
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

    private IEnumerator Initialize()
    {
        mInitializeSuccess = true;

        mCSV = gameObject.AddComponent<CSVManager>();
        if (!mCSV)
        {
            Debug.Log("Failed Get CSV Component.");
            mInitializeSuccess = false;
            yield break;
        }

        GameObject map = Resources.Load("01.Prefabs/Map/Map") as GameObject;
        if (!map)
        {
            Debug.Log("Failed Load Map Prefab.");
            mInitializeSuccess = false;
            yield break;
        }

        mMapPrefab = Instantiate<GameObject>(map, Vector3.zero, Quaternion.identity);
        if (!mMapPrefab)
        {
            Debug.Log("Failed Instantiate Map Prefab.");
            mInitializeSuccess = false;
            yield break;
        }

        mMap = mMapPrefab.GetComponent<MapManager>();
        if (!mMap)
        {
            Debug.Log("Failed GetComponent Map.");
            mInitializeSuccess = false;
            yield break;
        }

        if (!mMap.Initialize(mCSV))
        {
            Debug.Log("Failed Initialize Map Component.");
            mInitializeSuccess = false;
            yield break;
        }

        mWave = gameObject.AddComponent<WaveManager>();
        if (!mWave)
        {
            Debug.Log("Failed Add WaveManager Component");
            mInitializeSuccess = false;
            yield break;
        }

        if (!mWave.Initialize(mMap))
        {
            Debug.Log("Failed Initialize WaveManager Component");
            mInitializeSuccess = false;
            yield break;
        }

        //// UI Scene Load
        //AsyncOperation op = SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
        //op.allowSceneActivation = false;
        //while (!op.isDone)
        //{
        //    Debug.Log("Loading");
        //    yield return null;
        //}
        //op.allowSceneActivation = true;
        //Debug.Log("Load Complete");

        yield return SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);

        //SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);

        //yield return new WaitForSeconds(1.0f);

        mInput = gameObject.AddComponent<InputManager>();
        if (!mInput)
        {
            Debug.Log("Failed Add InputManager Component");
            mInitializeSuccess = false;
            yield break;
        }

        if (!mInput.Initlaize(mMap))
        {
            Debug.Log("Failed Initialize InputManager Component");
            mInitializeSuccess = false;
            yield break;
        }

        if (!mInitializeSuccess)
        {
            Debug.Log("Failed Initlaize GameManager");
            yield break;
        }
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

    public WaveManager GetWaveManager()
    {
        return mWave;
    }
}
