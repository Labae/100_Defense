using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable
{
    void Notify();
    void AddObserver(IObserver ob);
    void RemoveObserver(IObserver ob);
}

public interface IObserver
{
    void OnNotify(IObservable ob);
}
