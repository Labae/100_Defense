using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    /// <summary>
    /// 로딩 퍼센트 글짜.
    /// </summary>
    [SerializeField] private UILabel mLoadingLabel;
    /// <summary>
    /// 로딩 프로그레스 파.
    /// </summary>
    [SerializeField] private UIProgressBar mLoadingProgressBar;

    private void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");
        while(!operation.isDone)
        {
            mLoadingLabel.text = (operation.progress * 100).ToString() + "%";
            float progress = Mathf.Clamp01(operation.progress / 1.0f);
            mLoadingProgressBar.value = progress;
            yield return null;
        }
    }
}
