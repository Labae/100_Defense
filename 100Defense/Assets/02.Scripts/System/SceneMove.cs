using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    #region Singleton
    public static SceneMove instance;
    public static SceneMove Instance
    {
        get
        {
            if (instance == null)
            {
                SceneMove[] objs = FindObjectsOfType<SceneMove>();
                if (objs.Length > 0)
                {
                    instance = objs[0];
                }

                if (objs.Length > 1)
                {
                    Debug.LogError("SceneMove Error");
                }

                if (instance == null)
                {
                    GameObject obj = new GameObject("SceneMove");
                    obj.AddComponent<SceneMove>();
                }
            }

            return instance;
        }
    }
    #endregion

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

    public void MoveGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MoveTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
