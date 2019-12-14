using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    /// <summary>
    /// 길찾기 요청 큐.
    /// </summary>
    private Queue<PathRequset> mPathRequestQueue = new Queue<PathRequset>();
    /// <summary>
    /// 현재 길찾기 요청.
    /// </summary>
    private PathRequset mCurrentPathRequest;

    private static PathRequestManager instance;
    private PathFinding mPathFinding;

    private bool mIsProcessPath;

    public void Initialize()
    {
        instance = this;
        mPathFinding = GetComponent<PathFinding>();
    }

    public static void RequestPath(CellClass pathStart, CellClass pathEnd, Action<CellClass[], bool> callback)
    {
        PathRequset newRequest = new PathRequset(pathStart, pathEnd, callback);
        instance.mPathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!mIsProcessPath && mPathRequestQueue.Count > 0)
        {
            mCurrentPathRequest = mPathRequestQueue.Dequeue();
            mIsProcessPath = true;
            mPathFinding.StartFindPath(mCurrentPathRequest.gPathStart, mCurrentPathRequest.gPathEnd);
        }
    }

    public void FinishedProcessingPath(CellClass[] path, bool success)
    {
        mCurrentPathRequest.gCallback(path, success);
        mIsProcessPath = false;
        TryProcessNext();
    }

    struct PathRequset
    {
        public CellClass gPathStart;
        public CellClass gPathEnd;
        public Action<CellClass[], bool> gCallback;

        public PathRequset(CellClass _start, CellClass _end, Action<CellClass[], bool> _callback)
        {
            gPathStart = _start;
            gPathEnd = _end;
            gCallback = _callback;
        }
    };
}
