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
        SceneManager.LoadScene("TitleScene");
    }
}
