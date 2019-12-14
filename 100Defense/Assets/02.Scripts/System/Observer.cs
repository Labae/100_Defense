using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 관찰자 interface
/// </summary>
public interface IObservable
{
    void Notify();
    void AddObserver(IObserver ob);
    void RemoveObserver(IObserver ob);
}

/// <summary>
/// 관찰 대상 interface
/// </summary>
public interface IObserver
{
    void OnNotify(IObservable ob);
}
