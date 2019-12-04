using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInformation : IObservable
{
    private int mGold;
    private int mWaveIndex;
    private int mLife;

    public int Gold
    {
        get
        {
            return mGold;
        }
        set
        {
            if(mGold != value)
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
                mLife = value;
                Notify();
            }
        }
    }

    List<IObserver> observers = new List<IObserver>();

    public void AddObserver(IObserver ob)
    {
        observers.Add(ob);
    }

    public void Notify()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnNotify(this);
        }
    }

    public void RemoveObserver(IObserver ob)
    {
        if(observers.Contains(ob))
        {
            observers.Remove(ob);
        }
    }
}
