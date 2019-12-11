using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameInit : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.Initialize();
    }
}
