using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 초기화 클래스.
/// </summary>
public class GameInitializer : MonoBehaviour
{
    /// <summary>
    /// 게임 씬이 로드되면 게임 매니저의 상태를 게임으로 설정.
    /// </summary>
    void Start()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Game);
    }
}
