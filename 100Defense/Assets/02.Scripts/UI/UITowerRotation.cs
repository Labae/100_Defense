using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITowerRotation : MonoBehaviour
{
    private readonly float mRotateSpeed = 50.0f;
    private bool mIsRotate;

    public void RotateTower()
    {
        if(!mIsRotate)
        {
            mIsRotate = true;
        }
    }

    private void Update()
    {
        if(mIsRotate)
        {
            transform.Rotate(Vector3.down, mRotateSpeed * Time.deltaTime);
        }
    }

    public void StopRotateTower()
    {
        if(mIsRotate)
        {
            mIsRotate = false;
        }
    }
}
