using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum CellState
    {
        EDefault,
        EStart,
        EGoal,
        ERoad
    };

    private bool gApperance;

    private Map mMap;
    private CellState mState;
    private Material[] mMaterials;
    private MeshRenderer mMeshRenderer;

    public bool Initialize(int x, int y)
    {
        mMap = GetComponentInParent<Map>();
        if(!mMap)
        {
            Debug.Log("Failed Initialize Map Component");
            return false;
        }

        if(x == 0 && y == 0)
        {
            mState = CellState.EStart;
        }
        else if(x == mMap.GetMapSizeX() - 1 && y == mMap.GetMapSizeY() - 1)
        {
            mState = CellState.EGoal;
        }
        else
        {
            mState = CellState.EDefault;
        }

        mMaterials = new Material[(int)CellState.ERoad];
        mMaterials[(int)CellState.EDefault] = Resources.Load("02.Materials/CellDefault") as Material;
        if(!mMaterials[(int)CellState.EDefault])
        {
            Debug.Log("Failed Load CellDefault Material.");
            return false;
        }
        mMaterials[(int)CellState.EStart] = Resources.Load("02.Materials/CellStart") as Material;
        if (!mMaterials[(int)CellState.EStart])
        {
            Debug.Log("Failed Load CellStart Material.");
            return false;
        }
        mMaterials[(int)CellState.EGoal] = Resources.Load("02.Materials/CellGoal") as Material;
        if (!mMaterials[(int)CellState.EGoal])
        {
            Debug.Log("Failed Load CellGoal Material.");
            return false;
        }

        mMeshRenderer = GetComponent<MeshRenderer>();
        if(!mMeshRenderer)
        {
            Debug.Log("Failed Initialize mMeshRenderer Component");
            return false;
        }

        if (mMaterials[(int)mState] == null)
        {
            Debug.Log("Failed Found Material in mMaterials Array.");
            return false;
        }
        mMeshRenderer.material = mMaterials[(int)mState];
          
        return true;
    }

    public void ApperanceAnimation()
    {
        if (!gApperance)
        {
            gApperance = true;
            StopCoroutine(ApperanceAnimationCoroutine());
            StartCoroutine(ApperanceAnimationCoroutine());
        }
    }

    private IEnumerator ApperanceAnimationCoroutine()
    {
        float angle = 180.0f;
        float speed = 250.0f;

        while(angle <= 540.0f)
        {
            angle += Time.deltaTime * speed;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
    }
}
