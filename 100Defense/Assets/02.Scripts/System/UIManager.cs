using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private WaveManager mWave;

    private GameObject mSettingBtn;
    private UIPanel mScreenPanel;

    private GameObject mSettingBackground;
    private UIPanel mSettingPanel;

    private GameObject mCancelClick;
    private UIPanel mCancelClickPanel;

    private void Start()
    {
        if (!Initialize())
        {
            return;
        }
    }

    public bool Initialize()
    {
        mWave = GameManager.instance.GetWaveManager();
        if (!mWave)
        {
            Debug.Log("Failed Get WaveManager.");
            return false;
        }

        if (!UICreate.Initialize())
        {
            Debug.Log("Failed UICreate Initialize");
            return false;
        }


        Vector2 screenSize = new Vector2(GetComponent<UIRoot>().manualWidth, GetComponent<UIRoot>().manualHeight);

        // 스크린 패널
        GameObject screenPanel = UICreate.CreatePanel(transform, "Screen Panel");
        mScreenPanel = screenPanel.GetComponent<UIPanel>();
        mScreenPanel.depth = 1;
        mSettingBtn = UICreate.CreateButton(
            new Vector2(screenSize.x * -0.5f, screenSize.y * 0.5f),
            new Vector2(100, 100),
            0,
            new EventDelegate(OpenSettingPanel),
            screenPanel.transform,
            FlexibleUIButton.ButtonType.Setting,
            UIWidget.Pivot.TopLeft,
            "Setting Btn");
        mSettingBtn.GetComponent<UISprite>().color = Color.black;

        // Wave Start UI
        GameObject waveStartBtn = UICreate.CreateButton(
            new Vector2(screenSize.x * 0.5f, -screenSize.y * 0.5f),
            new Vector2(450, 150),
            0,
            new EventDelegate(WaveStart),
            screenPanel.transform,
            FlexibleUIButton.ButtonType.Default,
            UIWidget.Pivot.BottomRight,
            "WaveStart Btn");
        waveStartBtn.GetComponent<UISprite>().color = Color.cyan;

        GameObject waveStartLabel = UICreate.CreateLabel(
            Vector2.zero,
            waveStartBtn.GetComponent<UISprite>().localSize,
            1,
            waveStartBtn.transform,
            "Wave Start",
            UIWidget.Pivot.Center,
            "WaveStart Label");

        // 셋팅 패널
        GameObject settingPanel = UICreate.CreatePanel(transform, "Setting Panel");
        mSettingPanel = settingPanel.GetComponent<UIPanel>();
        mSettingPanel.depth = 2;
        mSettingBackground = UICreate.CreateBackground(
            Vector2.zero,
            new Vector2(700, 600),
            0,
            settingPanel.transform,
            UIWidget.Pivot.Center,
            "Setting Background"
            );
        mSettingBackground.SetActive(false);
        UISprite settingBackgroundSprite = mSettingBackground.GetComponent<UISprite>();
        GameObject settingExitBtn = UICreate.CreateButton(
            settingBackgroundSprite.localSize * 0.5f,
            new Vector2(100, 100),
            1,
            new EventDelegate(CloseSettingPanel),
            mSettingBackground.transform,
            FlexibleUIButton.ButtonType.Exit,
            UIWidget.Pivot.TopRight,
            "Setting Exit Btn");
        settingExitBtn.GetComponent<UISprite>().color = Color.black;


        // 터치 되는 부분을 설정하는 panel
        mCancelClick = UICreate.CreatePanel(transform, "Cancel Click");
        mCancelClickPanel = mCancelClick.GetComponent<UIPanel>();
        mCancelClickPanel.depth = 0;
        BoxCollider2D cancelCollider = mCancelClick.AddComponent<BoxCollider2D>();
        cancelCollider.isTrigger = true;
        cancelCollider.size = screenSize;
        GameObject cancelBackground = UICreate.CreateBackground(Vector2.zero, screenSize, 1, mCancelClick.transform);
        cancelBackground.GetComponent<UISprite>().color = Color.clear;
        cancelBackground.GetComponent<UISprite>().depth = 999;

        return true;
    }

    private void OpenSettingPanel()
    {
        mSettingBackground.SetActive(true);
        mCancelClickPanel.depth = mSettingPanel.depth - 1;
    }

    private void CloseSettingPanel()
    {
        mSettingBackground.SetActive(false);
        mCancelClickPanel.depth = 0;
    }

    private void WaveStart()
    {
        mWave.WaveStart();
    }
}
