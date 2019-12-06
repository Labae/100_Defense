﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class CSVManager : MonoBehaviour
{
    /// <summary>
    /// 맵의 크기.
    /// </summary>
    private int mMapX, mMapY;
    private Tower mTowerData;

    #region Load Save Map
    public string[,] LoadMap(int sizeX, int sizeY)
    {
        mMapX = sizeX;
        mMapY = sizeY;
        StreamReader strReader = new StreamReader(getPath("/Resources/03.Datas/Game/MapData.csv"));
        bool endOfFile = false;

        string[,] retval = new string[mMapX, mMapY];

        int x = 0;
        int y = 0;
        bool first = true;

        while (!endOfFile)
        {
            string data_string = strReader.ReadLine();
            if (data_string == null)
            {
                break;
            }

            if (first)
            {
                first = false;
                continue;
            }

            var data_value = data_string.Split(',');
            retval[x, y] = data_value[1].ToString();

            x++;
            if (x >= mMapX)
            {
                y++;
                x = 0;
            }

            if (y >= mMapY)
            {
                break;
            }
        }

        return retval;
    }

    public void MapSave(string[,] mapData)
    {
        List<string[]> rowData = new List<string[]>();
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = "Key";
        rowDataTemp[1] = "TowerName";
        rowData.Add(rowDataTemp);

        for (int x = 0; x < mMapX; x++)
        {
            for (int y = 0; y < mMapY; y++)
            {
                rowDataTemp = new string[2];
                int keyValue = x * mMapX + y % mMapY;
                string key = keyValue.ToString();
                rowDataTemp[0] = key;
                rowDataTemp[1] = mapData[y, x];
                rowData.Add(rowDataTemp);
            }
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        string filePath = getPath("/Resources/03.Datas/Game/MapData.csv");

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
    #endregion

    #region Load Save PlayerInfo
    public PlayerInformation LoadPlayerInfo()
    {
        StreamReader strReader = new StreamReader(getPath("/Resources/03.Datas/Game/PlayerInformation.csv"));
        bool endOfFile = false;
        bool first = true;

        PlayerInformation playerInfo = new PlayerInformation();
        mTowerData = Resources.Load("03.Datas/Game/TowerData") as Tower;
        if (!mTowerData)
        {
            Debug.Log("Tower data not load");
        }

        int dataLength = mTowerData.dataArray.Length + 3;
        string[] datas = new string[dataLength];

        int x = 0;
        int y = 0;
        while (!endOfFile)
        {
            string data_string = strReader.ReadLine();
            if (data_string == null)
            {
                break;
            }

            if (first)
            {
                first = false;
                continue;
            }

            var data_value = data_string.Split(',');

            datas[y] = data_value[1];
            x++;
            if (x >= 1)
            {
                y++;
                x = 0;
            }

            if (y >= dataLength)
            {
                break;
            }
        }

        playerInfo.Gold = int.Parse(datas[0]);
        playerInfo.WaveIndex = int.Parse(datas[1]);
        playerInfo.Life = int.Parse(datas[2]);

        for (int i = 0; i < mTowerData.dataArray.Length; i++)
        {
            if (i + 3 > dataLength)
            {
                break;
            }
            playerInfo.ContainTowerData.Add(mTowerData.dataArray[i], int.Parse(datas[i + 3]));
        }

        return playerInfo;
    }

    public void SavePlayerInfo(PlayerInformation info)
    {
        List<string[]> rowData = new List<string[]>();
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = "Key";
        rowDataTemp[1] = "Value";
        rowData.Add(rowDataTemp);

        rowDataTemp = new string[2];
        string key = "Gold";
        rowDataTemp[0] = key;
        rowDataTemp[1] = info.Gold.ToString();
        rowData.Add(rowDataTemp);

        key = "WaveIndex";
        rowDataTemp[0] = key;
        rowDataTemp[1] = info.WaveIndex.ToString();
        rowData.Add(rowDataTemp);

        key = "Life";
        rowDataTemp[0] = key;
        rowDataTemp[1] = info.Life.ToString();
        rowData.Add(rowDataTemp);

        if (!mTowerData)
        {
            mTowerData = Resources.Load("03.Datas/Game/TowerData") as Tower;
            Debug.Log("Tower data not load");
        }

        for (int i = 0; i < mTowerData.dataArray.Length; i++)
        {
            key = mTowerData.dataArray[i].Modelname;
            rowDataTemp[0] = key;
            rowDataTemp[1] = info.ContainTowerData[mTowerData.dataArray[i]].ToString();
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        string filePath = getPath("/Resources/03.Datas/Game/PlayerInformation.csv");

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
    #endregion

    private string getPath(string path)
    {
#if UNITY_EDITOR
        return Application.dataPath + path;
#elif UNITY_ANDROID
        return Application.persistentDataPath+path;
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+path;
#else
        return Application.dataPath +"/"+"path;
#endif
    }

    public Tower GetTowerData()
    {
        return mTowerData;
    }
}
