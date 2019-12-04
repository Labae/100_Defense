using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void MoveGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MoveTitleScene()
    {
        StartCoroutine(MoveTitleSceneCoroutine());
    }

    private IEnumerator MoveTitleSceneCoroutine()
    {
        float waitTime = 1.0f;
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("TitleScene");
    }
}
