using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameManager[] objs = FindObjectsOfType<GameManager>();
                if (objs.Length > 0)
                {
                    instance = objs[0];
                }

                if (objs.Length > 1)
                {
                    Debug.LogError("GameManager Error");
                }

                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    obj.AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }
    #endregion

    public enum GameState
    {
        Splash,
        Title,
        Game,
        Restart,
        GameOver
    }

    private InputManager mInput;
    private GameObject mMapPrefab;
    private MapManager mMap;
    private CSVManager mCSV;
    private WaveManager mWave;
    private ObjectPool mObjectPool;

    private PlayerInformation mPlayerInfo;

    private bool mInitializeSuccess;
    private bool mIsGameover;
    public bool IsGameOver
    {
        get
        {
            return mIsGameover;
        }
        set
        {
            if (value == true)
            {
                GameOver();
            }

            mIsGameover = value;
        }
    }

    [SerializeField]
    private GameState mGameState;
    [Header("Max Size (10, 10)")]
    [SerializeField]
    private Vector2 mMapSize = new Vector2(7, 7);

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

    private void Update()
    {
        if (mGameState == GameState.Game)
        {
            if (mInitializeSuccess)
            {
                if (!GetIsGameOver())
                {
                    if (mInput != null)
                    {
                        mInput.MouseEvent();
                        mInput.KeyboardEvent();
                    }
                    if (mMap != null)
                    {
                        mMap.TowerUpdate();
                    }
                }
            }
        }
    }
    public void Initialize()
    {
        StartCoroutine(InitializeCoroutine());
        if (!mInitializeSuccess)
        {
            return;
        }
        StartCoroutine(InitializeAnim());
    }

    private void GameOver()
    {
        List<EnemyClass> enemise = mMap.GetEnemies();
        for (int i = 0; i < enemise.Count; i++)
        {
            enemise[i].DestroyEnemy();
        }
    }

    public bool GetIsGameOver()
    {
        IsGameOver = false;

        if (mPlayerInfo.Life <= 0)
        {
            IsGameOver = true;
        }

        return IsGameOver;
    }

    private IEnumerator InitializeCoroutine()
    {
        mInitializeSuccess = true;

        mCSV = gameObject.AddComponent<CSVManager>();
        if (!mCSV)
        {
            Debug.Log("Failed Get CSV Component.");
            mInitializeSuccess = false;
            yield break;
        }

        mPlayerInfo = mCSV.LoadPlayerInfo();
        if (mPlayerInfo == null)
        {
            Debug.Log("Failed Load PlayerInfo");
            yield break;
        }

        mObjectPool = gameObject.AddComponent<ObjectPool>();
        if (!mObjectPool)
        {
            Debug.Log("Failed AddComponent ObjectPool Component.");
            mInitializeSuccess = false;
            yield break;
        }

        if (!mObjectPool.Initialize(mMapSize))
        {
            Debug.Log("Failed Initialize ObjectPool Component.");
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

        mMapPrefab = Instantiate(map, Vector3.zero, Quaternion.identity) as GameObject;
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

        if (!mMap.Initialize(mCSV, mMapSize))
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

        yield return SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);

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

    public MapManager GetMapManager()
    {
        return mMap;
    }

    public WaveManager GetWaveManager()
    {
        return mWave;
    }

    public PlayerInformation GetPlayerInfo()
    {
        return mPlayerInfo;
    }

    public ObjectPool GetObjectPool()
    {
        return mObjectPool;
    }

    public CSVManager GetCSVManager()
    {
        return mCSV;
    }

    public void SetGameState(GameState state)
    {
        if (mGameState != state)
        {
            mGameState = state;

            switch (mGameState)
            {
                case GameState.Splash:
                    break;
                case GameState.Title:
                    break;
                case GameState.Game:
                    Initialize();
                    mIsGameover = false;
                    break;
                case GameState.Restart:
                    PlayerInformation prevInfo = mWave.GetPrevPlayerInfo();
                    mMap.Save();
                    mCSV.SavePlayerInfo(prevInfo);
                    SceneMove.Instance.MoveGameScene();
                    break;
                case GameState.GameOver:
                    mCSV.ClearCSVFiles();
                    break;
                default:
                    break;
            }
        }
    }

    #region Static
    public static void ChangeLayerMaskRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            ChangeLayerMaskRecursively(child, name);
        }
    }
    #endregion
}
