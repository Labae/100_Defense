using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UITitle : MonoBehaviour
{
    /// <summary>
    /// 뒷 터치 방지 패널.
    /// </summary>
    [SerializeField] private UIPanel mTouchGuard;
    /// <summary>
    /// 게임 시작 버튼.
    /// </summary>
    [SerializeField] private UIButton mStartButton;
    /// <summary>
    /// 제작자 패널.
    /// </summary>
    [SerializeField] private GameObject mCreditPanel;
    /// <summary>
    /// 음악 오디오 소스.
    /// </summary>
    [SerializeField] private AudioSource mMusicSource;
    /// <summary>
    /// 음악 오디오 소스.
    /// </summary>
    [SerializeField] private AudioSource mSfxSource;
    /// <summary>
    /// 버튼 클릭 소리.
    /// </summary>
    [SerializeField] private AudioClip mButtonClickSfx;
    /// <summary>
    /// 버튼 소환 소리.
    /// </summary>
    [SerializeField] private AudioClip mButtoSpawnSfx;
    /// <summary>
    /// 음악 조절 슬라이더.
    /// </summary>
    [SerializeField] private UISlider mMusicSoundSlider;
    /// <summary>
    /// 효과음 조절 슬라이더.
    /// </summary>
    [SerializeField] private UISlider mSfxSoundSlider;
    /// <summary>
    /// 설정 패널.
    /// </summary>
    [SerializeField] private GameObject mSettingPanel;

    #region Unity Event
    private void Start()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Title);
        mStartButton.onClick.Add(new EventDelegate(() => SceneMove.Instance.MoveGameScene()));
        mMusicSource.volume = GameManager.Instance.MusicVolume;
        mSfxSource.volume = GameManager.Instance.SfxVolume;
        mMusicSoundSlider.value = mMusicSource.volume;
        mSfxSoundSlider.value = mSfxSource.volume;
    }
    #endregion

    #region Button Event
    public void SpawnButtonSfx()
    {
        mMusicSource.PlayOneShot(mButtoSpawnSfx);
    }

    /// <summary>
    /// 제작자 패널 여는 버튼.
    /// </summary>
    public void OpenCredit()
    {
        mCreditPanel.transform.DOLocalMoveX(0.0f, 0.2f).SetEase(Ease.Linear);
        mTouchGuard.depth = mCreditPanel.GetComponent<UIPanel>().depth;
    }

    /// <summary>
    /// 제작자 패널을 닫는 버튼.
    /// </summary>
    public void CloseCredit()
    {
        mCreditPanel.transform.DOLocalMoveX(1000.0f, 0.2f).SetEase(Ease.Linear);
        mTouchGuard.depth = 0;
    }

    public void PlayButtonClickSfx()
    {
        mMusicSource.PlayOneShot(mButtonClickSfx);
    }

    public void MusicSoundVolumSlider()
    {
        mMusicSource.volume = mMusicSoundSlider.value * 0.1f;
        mMusicSource.volume = Mathf.Clamp(mMusicSource.volume, 0.0f, 0.1f);
        GameManager.Instance.MusicVolume = mMusicSource.volume;
    }

    public void SfxSoundVolumSlider()
    {
        mSfxSource.volume = mSfxSoundSlider.value * 0.1f;
        mSfxSource.volume = Mathf.Clamp(mSfxSource.volume, 0.0f, 0.1f);
        GameManager.Instance.SfxVolume = mSfxSource.volume;
    }

    public void OpenSettingPanel()
    {
        mSettingPanel.transform.DOLocalMoveX(0.0f, 0.2f).SetEase(Ease.Linear);
        mTouchGuard.depth = mSettingPanel.GetComponent<UIPanel>().depth;
    }

    public void CloseSettingPanel()
    {
        mSettingPanel.transform.DOLocalMoveX(1000.0f, 0.2f).SetEase(Ease.Linear);
        mTouchGuard.depth = 0;
    }
    #endregion
}
