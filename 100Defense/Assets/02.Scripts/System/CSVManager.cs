﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using static System.IO.Directory;

public class CSVManager : MonoBehaviour
{
    /// <summary>
    /// 맵의 크기.
    /// </summary>
    private int mMapX, mMapY;

    #region Method
    public bool Initialize(int mapX, int mapY)
    {
        mMapX = mapX;
        mMapY = mapY;

        string dirPath = getPath("/CSV");
        if(!Exists(dirPath))
        {
            CreateDirectory(dirPath);
        }

        string mapSavePath = getPath("/CSV/MapData.csv");
        FileInfo mapInfo = new FileInfo(mapSavePath);
        if(!mapInfo.Exists)
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
                    rowDataTemp[1] = "0";
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

            string filePath = getPath("/CSV/MapData.csv");

            StreamWriter outStream = File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        string playerSavePath = getPath("/CSV/PlayerInformation.csv");
        FileInfo playerInfo = new FileInfo(playerSavePath);
        if(!playerInfo.Exists)
        {
            List<string[]> rowData = new List<string[]>();
            string[] rowDataTemp = new string[2];
            rowDataTemp[0] = "Key";
            rowDataTemp[1] = "Value";
            rowData.Add(rowDataTemp);

            rowDataTemp = new string[2];
            string key = "Gold";
            rowDataTemp[0] = key;
            rowDataTemp[1] = "100000";
            rowData.Add(rowDataTemp);

            rowDataTemp = new string[2];
            key = "WaveIndex";
            rowDataTemp[0] = key;
            rowDataTemp[1] = "0";
            rowData.Add(rowDataTemp);

            rowDataTemp = new string[2];
            key = "Life";
            rowDataTemp[0] = key;
            rowDataTemp[1] = "3";
            rowData.Add(rowDataTemp);

            string[][] output  = new string[rowData.Count][];

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

            string filePath = getPath("/CSV/PlayerInformation.csv");

            StreamWriter outStream = File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }
        return true;
    }
    #endregion

    #region Load Save Map
    /// <summary>
    /// 맵 데이터 로드.
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <returns></returns>
    public string[,] LoadMap(int sizeX, int sizeY)
    {
        mMapX = sizeX;
        mMapY = sizeY;
        StreamReader strReader = new StreamReader(getPath("/CSV/MapData.csv"));
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

        strReader.Dispose();
        return retval;
    }
    /// <summary>
    /// 맵 데이터 저장.
    /// </summary>
    /// <param name="mapData"></param>
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

        string filePath = getPath("/CSV/MapData.csv");

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
    #endregion

    #region Load Save PlayerInfo
    /// <summary>
    /// 플레이어 정보 로드.
    /// </summary>
    /// <returns></returns>
    public PlayerInformation LoadPlayerInfo()
    {
        StreamReader strReader = new StreamReader(getPath("/CSV/PlayerInformation.csv"));
        bool endOfFile = false;
        bool first = true;

        PlayerInformation playerInfo = new PlayerInformation();
        int dataLength = 3;
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

        strReader.Dispose();
        return playerInfo;
    }
    /// <summary>
    /// 플레이어 정보 저장.
    /// </summary>
    /// <param name="info"></param>
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

        rowDataTemp = new string[2];
        key = "WaveIndex";
        rowDataTemp[0] = key;
        rowDataTemp[1] = info.WaveIndex.ToString();
        rowData.Add(rowDataTemp);

        rowDataTemp = new string[2];
        key = "Life";
        rowDataTemp[0] = key;
        rowDataTemp[1] = info.Life.ToString();
        rowData.Add(rowDataTemp);

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

        string filePath = getPath("/CSV/PlayerInformation.csv");

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
    #endregion

    /// <summary>
    /// 모든 CSV File을 기본으로 설정.
    /// </summary>
    public void ClearCSVFiles()
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
                string mapIndex = keyValue.ToString();
                rowDataTemp[0] = mapIndex;
                rowDataTemp[1] = "0";
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

        string filePath = getPath("/CSV/MapData.csv");

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();

        rowData = new List<string[]>();
        rowDataTemp = new string[2];
        rowDataTemp[0] = "Key";
        rowDataTemp[1] = "Value";
        rowData.Add(rowDataTemp);

        rowDataTemp = new string[2];
        string key = "Gold";
        rowDataTemp[0] = key;
        rowDataTemp[1] = "100";
        rowData.Add(rowDataTemp);

        rowDataTemp = new string[2];
        key = "WaveIndex";
        rowDataTemp[0] = key;
        rowDataTemp[1] = "0";
        rowData.Add(rowDataTemp);

        rowDataTemp = new string[2];
        key = "Life";
        rowDataTemp[0] = key;
        rowDataTemp[1] = "3";
        rowData.Add(rowDataTemp);

        output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        length = output.GetLength(0);
        delimiter = ",";

        sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        filePath = getPath("/CSV/PlayerInformation.csv");

        outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    /// <summary>
    /// 파일 경로 가져오기.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private string getPath(string path)
    {
#if UNITY_EDITOR
        return Application.dataPath + path;
#elif UNITY_ANDROID
        return Application.persistentDataPath + path;
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+path;
#else
        return Application.dataPath +"/"+"path;
#endif
    }
}
