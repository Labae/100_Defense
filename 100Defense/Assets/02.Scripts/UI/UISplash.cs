using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI Splash 클래스.
/// </summary>
public class UISplash : MonoBehaviour
{
    /// <summary>
    /// SplashText를 갖고있는 부모 Set.
    /// </summary>
    [SerializeField] private GameObject TextSet = null;
    [SerializeField] private AudioSource mAudioSource;
    [SerializeField] private AudioClip mSplashSfx;

    private void Start()
    {
        mAudioSource.volume = GameManager.Instance.SfxVolume;
    }

    /// <summary>
    /// UISplashText를 하얗게 하기 위한 함수.
    /// </summary>
    public void SetTextColorWhite()
    {
        SplashText[] splashTexts = TextSet.GetComponentsInChildren<SplashText>();
        for (int i = 0; i < splashTexts.Length; i++)
        {
            splashTexts[i].SetTextWhite();
        }
    }

    public void PlaySplashSfx()
    {
        mAudioSource.PlayOneShot(mSplashSfx);
    }
}
