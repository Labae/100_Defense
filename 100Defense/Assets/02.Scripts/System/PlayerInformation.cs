using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DictionaryComparer<TKey, TValue> : IEqualityComparer<Dictionary<TKey, TValue>>
{
    private IEqualityComparer<TValue> valueComparer;
    public DictionaryComparer(IEqualityComparer<TValue> valueComparer = null)
    {
        this.valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
    }
    public bool Equals(Dictionary<TKey, TValue> x, Dictionary<TKey, TValue> y)
    {
        if (x.Count != y.Count)
            return false;
        if (x.Keys.Except(y.Keys).Any())
            return false;
        if (y.Keys.Except(x.Keys).Any())
            return false;
        foreach (var pair in x)
            if (!valueComparer.Equals(pair.Value, y[pair.Key]))
                return false;
        return true;
    }

    public int GetHashCode(Dictionary<TKey, TValue> obj)
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class PlayerInformation : IObservable
{
    private int mGold;
    private int mWaveIndex;
    private int mLife;
    private Dictionary<TowerData, int> mContainTowerData = new Dictionary<TowerData, int>();

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
                mLife = value;
                Notify();
            }
        }
    }

    public Dictionary<TowerData, int> ContainTowerData
    {
        get
        {
            return mContainTowerData;
        }
        set
        {
            DictionaryComparer<TowerData, int> dc = new DictionaryComparer<TowerData, int>();
            if(!dc.Equals(ContainTowerData, value))
            {
                mContainTowerData = value;
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
        if (observers.Contains(ob))
        {
            observers.Remove(ob);
        }
    }
}
