using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITowerRotation : MonoBehaviour
{
    /// <summary>
    /// 타워 회전 속도.
    /// </summary>
    private readonly float mRotateSpeed = 50.0f;
    /// <summary>
    /// 타워가 회전하는지.
    /// </summary>
    private bool mIsRotate;

    #region Unity Event
    private void Update()
    {
        if (mIsRotate)
        {
            transform.Rotate(Vector3.down, mRotateSpeed * Time.deltaTime);
        }
    }
    #endregion

    #region Method
    /// <summary>
    /// 타워를 회전시키는 함수.
    /// </summary>
    public void RotateTower()
    {
        if(!mIsRotate)
        {
            mIsRotate = true;
        }
    }

    /// <summary>
    /// 타워 회전을 정지시키는 함수.
    /// </summary>
    public void StopRotateTower()
    {
        if(mIsRotate)
        {
            mIsRotate = false;
        }
    }
    #endregion
}
