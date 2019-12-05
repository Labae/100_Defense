using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashText : MonoBehaviour
{
    public void SetTextWhite()
    {
        StartCoroutine(TextColorCoroutine());
    }

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
