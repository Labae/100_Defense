using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 정보 클래스.
/// </summary>
[System.Serializable]
public class PlayerInformation : IObservable
{
    /// <summary>
    /// 가지고 있는 골드.
    /// </summary>
    private int mGold;
    /// <summary>
    /// 현재 웨이브 단계.
    /// </summary>
    private int mWaveIndex;
    /// <summary>
    /// 현재 라이프 지수.
    /// </summary>
    private int mLife;

    #region Property
    public int Gold
    {
        get
        {
            return mGold;
        }
        set
        {
            if (mGold != value)
            {
                mGold = value;
                Notify();
            }
        }
    }

    public int WaveIndex
    {
        get
        {
            return mWaveIndex;
        }
        set
        {
            if (mWaveIndex != value)
            {
                mWaveIndex = value;
                Notify();
            }
        }
    }

    public int Life
    {
        get
        {
            return mLife;
        }
        set
        {
            if (mLife != value)
            {
                if (value > 3)
                {
                    mLife = 3;
                }
                else
                {
                    mLife = value;
                    Notify();
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// 관찰 대상 List
    /// </summary>
    List<IObserver> observers = new List<IObserver>();

    /// <summary>
    /// 관찰 대상 추가.
    /// </summary>
    /// <param name="ob"></param>
    public void AddObserver(IObserver ob)
    {
        observers.Add(ob);
    }

    /// <summary>
    /// 관찰 대상들에게 변화 알려주기.
    /// </summary>
    public void Notify()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnNotify(this);
        }
    }

    /// <summary>
    /// 관찰 대상 제거.
    /// </summary>
    /// <param name="ob"></param>
    public void RemoveObserver(IObserver ob)
    {
        if (observers.Contains(ob))
        {
            observers.Remove(ob);
        }
    }
}
