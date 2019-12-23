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

    /// <summary>
    /// 게임 상태.
    /// </summary>
    public enum GameState
    {
        Splash,
        Title,
        Game,
        Restart,
        GameOver
    }

    /// <summary>
    /// Input Manager
    /// </summary>
    private InputManager mInput;
    /// <summary>
    /// 맵 Prefab
    /// </summary>
    private GameObject mMapPrefab;
    /// <summary>
    /// 맵 클래스.
    /// </summary>
    private MapManager mMap;
    /// <summary>
    /// CSV 매니저 클래스.
    /// </summary>
    private CSVManager mCSV;
    /// <summary>
    /// 웨이브 매니저 클래스.
    /// </summary>
    private WaveManager mWave;
    /// <summary>
    /// 오브젝트 풀 클래스.
    /// </summary>
    private ObjectPool mObjectPool;

    /// <summary>
    /// 사운드 매니저 클래스.
    /// </summary>
    private SoundManager mSoundManager;
    public float SfxVolume
    {
        get;
        set;
    }
    public float MusicVolume
    {
        get;
        set;
    }

    ///Player Pref Key
    private const string mPrefMusicVolumeKey = "Music";
    private const string mPrefVolumeKey = "Volume";
    private const string mFirstKey = "First";

    /// <summary>
    /// 플레이어 정보.
    /// </summary>
    private PlayerInformation mPlayerInfo;

    /// <summary>
    /// 초기화 성공 여부.
    /// </summary>
    private bool mInitializeSuccess;
    /// <summary>
    /// 게임이 종료되었는지.
    /// </summary>
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

    /// <summary>
    /// 현재 게임 상태.
    /// </summary>
    [SerializeField] private GameState mGameState;
    /// <summary>
    /// 맵 크기.
    /// </summary>
    [Header("Max Size (10, 10)")]
    [SerializeField] private Vector2 mMapSize = new Vector2(7, 7);

    #region Unity Event
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

        Screen.SetResolution(900, 1600, true);
        mGameState = GameState.Splash;

        bool isFirst = PlayerPrefs.HasKey(mFirstKey);
        if (!isFirst)
        {
            SfxVolume = 0.05f;
            MusicVolume = 0.05f;
        }
        else
        {
            SfxVolume = PlayerPrefs.GetFloat(mPrefVolumeKey);
            MusicVolume = PlayerPrefs.GetFloat(mPrefMusicVolumeKey);
        }
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

    private void OnApplicationQuit()
    {
        if (mGameState == GameState.Game)
        {
            mMap.Save();
            mCSV.SavePlayerInfo(mPlayerInfo);
        }

        PlayerPrefs.SetFloat(mPrefVolumeKey, SfxVolume * 10.0f);
        PlayerPrefs.SetFloat(mPrefMusicVolumeKey, MusicVolume * 10.0f);
        PlayerPrefs.SetInt(mFirstKey, 1);

        if (mGameState != GameState.GameOver)
        {
            if (mMap != null)
            {
                mMap.Save();
            }
            if (mCSV != null)
            {
                mCSV.SavePlayerInfo(mPlayerInfo);
            }
        }
    }

    #endregion

    #region Method
    /// <summary>
    /// 게임 매니저 초기화.
    /// </summary>
    public void Initialize()
    {
        StartCoroutine(InitializeCoroutine());
        if (!mInitializeSuccess)
        {
            return;
        }
        StartCoroutine(InitializeAnim());
    }

    /// <summary>
    /// 게임 종료 함수.
    /// </summary>
    private void GameOver()
    {
        List<EnemyClass> enemise = mMap.GetEnemies();
        for (int i = 0; i < enemise.Count; i++)
        {
            enemise[i].DestroyEnemy();
        }
    }

    #endregion

    #region Coroutine
    /// <summary>
    /// 초기화 Coroutine
    /// </summary>
    /// <returns></returns>
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

        if(!mCSV.Initialize((int)mMapSize.x, (int)mMapSize.y))
        {
            Debug.Log("Failed Initialize CSV Component.");
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

        mSoundManager = gameObject.AddComponent<SoundManager>();
        if (!mSoundManager)
        {
            Debug.Log("Failed Add SoundManager Component");
            mInitializeSuccess = false;
            yield break;
        }

        if (!mSoundManager.Initialize())
        {
            Debug.Log("Failed Initialize mSoundManager Component");
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
    /// <summary>
    /// 초기화 애니메이션.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitializeAnim()
    {
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(mMap.MapAnimCoroutine());

        yield return new WaitForSeconds(0.5f);
        mMap.GetPathFinding().PathFind();

        yield return new WaitForSeconds(0.5f);
        mMap.SetCanClick(true);
    }
    #endregion

    #region Get Set
    /// <summary>
    /// 게임 종료 여부
    /// </summary>
    /// <returns></returns>
    public bool GetIsGameOver()
    {
        IsGameOver = false;

        if (mPlayerInfo != null)
        {
            if (mPlayerInfo.Life <= 0)
            {
                IsGameOver = true;
            }
        }
        return IsGameOver;
    }
    /// <summary>
    /// 맵 매니저 가져오기.
    /// </summary>
    /// <returns></returns>
    public MapManager GetMapManager()
    {
        return mMap;
    }
    /// <summary>
    /// 웨이브 매니저 가져오기.
    /// </summary>
    /// <returns></returns>
    public WaveManager GetWaveManager()
    {
        return mWave;
    }
    /// <summary>
    /// 플레이어 정보 가져오기.
    /// </summary>
    /// <returns></returns>
    public PlayerInformation GetPlayerInfo()
    {
        return mPlayerInfo;
    }
    /// <summary>
    /// 오브젝트 풀 가져오기.
    /// </summary>
    /// <returns></returns>
    public ObjectPool GetObjectPool()
    {
        return mObjectPool;
    }


    /// <summary>
    /// 사운드 매니저 가져오기.
    /// </summary>
    /// <returns></returns>
    public SoundManager GetSoundManager()
    {
        return mSoundManager;
    }

    /// <summary>
    /// 게임 상태 설정.
    /// </summary>
    /// <param name="state"></param>
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
                    mPlayerInfo = mCSV.LoadPlayerInfo();
                    break;
                default:
                    break;
            }
        }
    }

    #endregion

    #region Static
    /// <summary>
    /// 모든 Layer를 재설정.
    /// </summary>
    /// <param name="trans">부모 오브젝트</param>
    /// <param name="name">레이어 이름</param>
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
