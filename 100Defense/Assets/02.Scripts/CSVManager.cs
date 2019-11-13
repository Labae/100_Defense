using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVManager : MonoBehaviour
{
    public string[,] LoadMap(string mapPath)
    {
        TextAsset mapData = Resources.Load(mapPath) as TextAsset;
        if(!mapData)
        {
            Debug.Log("Failed Load mapData");
            return null;
        }

        string[,] retVal = new string[10, 10];

        string[] data = mapData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (row[1] != "")
            {
                Vector2 mapIndex = GetMapIndex(int.Parse(row[0]));
                row[1] = row[1].Remove(row[1].Length - 1);
                retVal[(int)mapIndex.x, (int)mapIndex.y] = row[1];
            }
        }

        return retVal;
    }

    public void BuildTower(int x, int y, TowerType type)
    {

    }

    private Vector2 GetMapIndex(int index)
    {
        return new Vector2(index / 10, index % 10);
    }
}
