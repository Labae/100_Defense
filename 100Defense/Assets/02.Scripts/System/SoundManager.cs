using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource mSfxAudioSource;
    private AudioClip mUIClickSfx;
    private AudioClip mGetCoinSfx;
    private AudioClip mCellClickSfx;
    private AudioClip mTowerBuildSfx;
    private AudioClip mTowerDestroySfx;

    private const string mPath = "05.Sounds/";

    private AudioSource mMusicAudioSource;
    public AudioSource SfxAudioSource { get => mSfxAudioSource; set => mSfxAudioSource = value; }
    public AudioSource MusicAudioSource { get => mMusicAudioSource; set => mMusicAudioSource = value; }

    public bool Initialize()
    {
        MusicAudioSource = Camera.main.GetComponent<AudioSource>();
        if (MusicAudioSource == null)
        {
            Debug.Log("Audio source is null");
            return false;
        }

        MusicAudioSource.volume = GameManager.Instance.MusicVolume * 10.0f;
        MusicAudioSource.PlayDelayed(0.5f);

        SfxAudioSource = FindObjectOfType<GameInitializer>().GetComponent<AudioSource>();
        if(SfxAudioSource == null)
        {
            Debug.Log("Audio source is null");
            return false;
        }

        SfxAudioSource.volume = GameManager.Instance.SfxVolume * 10.0f;

        mUIClickSfx = Resources.Load(mPath + "UI_Click") as AudioClip;
        if(mUIClickSfx == null)
        {
            Debug.Log("mUIClickSfx is null");
            return false;
        }

        mGetCoinSfx = Resources.Load(mPath + "Get_Coin") as AudioClip;
        if(mGetCoinSfx == null)
        {
            Debug.Log("mGetCoinSfx is null");
            return false;
        }

        mCellClickSfx = Resources.Load(mPath + "CellClick") as AudioClip;
        if (mCellClickSfx == null)
        {
            Debug.Log("mCellClick is null");
            return false;
        }

        mTowerBuildSfx = Resources.Load(mPath + "TowerBuild") as AudioClip;
        if (mTowerBuildSfx == null)
        {
            Debug.Log("mTowerBuildSfx is null");
            return false;
        }

        mTowerDestroySfx = Resources.Load(mPath + "TowerDestroy") as AudioClip;
        if (mTowerDestroySfx == null)
        {
            Debug.Log("mTowerDestroySfx is null");
            return false;
        }

        return true;
    }

    public void PlayUIClickSfx()
    {
        SfxAudioSource.PlayOneShot(mUIClickSfx);
    }

    public void PlayGetCoinSfx()
    {
        SfxAudioSource.PlayOneShot(mGetCoinSfx);
    }

    public void PlayCellClickSfx()
    {
        SfxAudioSource.PlayOneShot(mCellClickSfx);
    }

    public void PlayTowerBuildSfx()
    {
        SfxAudioSource.PlayOneShot(mTowerBuildSfx);
    }

    public void PlayTowerDestroySfx()
    {
        SfxAudioSource.PlayOneShot(mTowerDestroySfx);
    }
}
