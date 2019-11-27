using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject mUIScreen;
    private GameObject mUIContainer;

    private GameObject mGameQuitBtn; 

    private void Start()
    {
        if(!Initialize())
        {
            return;
        }
    }

    public bool Initialize()
    {
        WaveManager mWave = GameManager.instance.GetWaveManager();
        if (!mWave)
        {
            Debug.Log("Failed Get WaveManager.");
            return false;
        }

        mUIScreen = GameObject.Find("UIScreen");
        mUIContainer = GameObject.Find("UIContainer");

        if(mUIScreen == null)
        {
            Debug.Log("Failed Find UIScreen");
            return false;
        }
        if (mUIContainer == null)
        {
            Debug.Log("Failed Find UIContainer");
            return false;
        }

        UIWidget screenWidget = mUIScreen.GetComponent<UIWidget>();
        UIWidget containerWidget = mUIContainer.GetComponent<UIWidget>();

        mGameQuitBtn = UICreate.CreateButton(screenWidget.localSize * 0.5f, new Vector2(100.0f, 100.0f), 1, new EventDelegate(Test), mUIScreen.transform, FlexibleUIButton.ButtonType.Exit, UIWidget.Pivot.TopRight, "Exit Btn");

        return true;
    }

    public void Test()
    {
        Debug.Log("TEST");
    }
}
