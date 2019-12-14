using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Splash 애니메이션 텍스트 클래스.
/// </summary>
public class SplashText : MonoBehaviour
{
    /// <summary>
    /// 텍스트를 하얀색으로 만드는 함수.
    /// </summary>
    public void SetTextWhite()
    {
        StartCoroutine(TextColorCoroutine());
    }

    /// <summary>
    /// 텍스트를 천천히 하얗게 만드는 코루틴.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TextColorCoroutine()
    {
        UILabel uILabel = GetComponent<UILabel>();
        Color color = uILabel.color;
        while (color != Color.white)
        {
            color = Color.Lerp(color, Color.white, Time.deltaTime);
            uILabel.color = color;
            yield return null;
        }
    }
}
