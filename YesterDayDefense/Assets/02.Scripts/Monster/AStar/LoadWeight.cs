using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWeight : MonoBehaviour
{
    public static LoadWeight Instance;

    public int[,] weight = new int[33, 33];
    public int[,] endVal = new int[33, 33];

    [SerializeField] bool isDebug = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : LoadWeight is multiple running!");
    }

    public void ChangeWeight(int x, int y, int val)
    {
        weight[x, y] += val;
        InitEndVal();
    }

    public void InitEndVal()
    {
        for (int i = 0; i < 33; i++)
        {
            for (int j = 0; j < 33; j++)
            {
                endVal[i, j] = 1;
            }
        }

        int centerX = MapManager.Instance.CoreX;
        int centerY = MapManager.Instance.CoreY;
        endVal[centerX, centerY] = 0;

        int xIdx;
        int yIdx;
        for (int i = 1; i <= 32; i++)
        {
            xIdx = i; // ¿ÞÂÊ ³¡¿¡¼­
            yIdx = 0;
            do
            {
                if (centerX - xIdx >= 0 && centerX - xIdx <= 32 && centerY - yIdx >= 0 && centerY - yIdx <= 32)
                {
                    if (yIdx == 0)
                    {
                        endVal[centerX - xIdx, centerY - yIdx] =
                            endVal[centerX - xIdx + 1, centerY - yIdx] +
                            weight[centerX - xIdx, centerY - yIdx];
                    }
                    else
                    {
                        endVal[centerX - xIdx, centerY - yIdx] =
                            Mathf.Min(endVal[centerX - xIdx + 1, centerY - yIdx],
                            endVal[centerX - xIdx, centerY - yIdx - 1]) +
                            weight[centerX - xIdx, centerY - yIdx];
                    }
                }
                xIdx--;
                yIdx--;
            } while (xIdx > 0);

            do
            {
                if (centerX - xIdx >= 0 && centerX - xIdx <= 32 && centerY - yIdx >= 0 && centerY - yIdx <= 32)
                {
                    if (xIdx == 0)
                    {
                        endVal[centerX - xIdx, centerY - yIdx] =
                            endVal[centerX - xIdx, centerY - yIdx - 1] +
                            weight[centerX - xIdx, centerY - yIdx];
                    }
                    else
                    {
                        endVal[centerX - xIdx, centerY - yIdx] =
                            Mathf.Min(endVal[centerX - xIdx - 1, centerY - yIdx],
                            endVal[centerX - xIdx, centerY - yIdx - 1]) +
                            weight[centerX - xIdx, centerY - yIdx];
                    }
                }
                xIdx--;
                yIdx++;
            } while (yIdx < 0);

            do
            {
                if (centerX - xIdx >= 0 && centerX - xIdx <= 32 && centerY - yIdx >= 0 && centerY - yIdx <= 32)
                {
                    if (yIdx == 0)
                    {
                        endVal[centerX - xIdx, centerY - yIdx] =
                            endVal[centerX - xIdx - 1, centerY - yIdx] +
                            weight[centerX - xIdx, centerY - yIdx];
                    }
                    else
                    {
                        endVal[centerX - xIdx, centerY - yIdx] =
                            Mathf.Min(endVal[centerX - xIdx - 1, centerY - yIdx],
                            endVal[centerX - xIdx, centerY - yIdx + 1]) +
                            weight[centerX - xIdx, centerY - yIdx];
                    }
                }
                xIdx++;
                yIdx++;
            } while (xIdx < 0);

            do
            {
                if (centerX - xIdx >= 0 && centerX - xIdx <= 32 && centerY - yIdx >= 0 && centerY - yIdx <= 32)
                {
                    if (xIdx == 0)
                    {
                        endVal[centerX - xIdx, centerY - yIdx] = endVal[centerX - xIdx, centerY - yIdx + 1]
                            + weight[centerX - xIdx, centerY - yIdx];
                    }
                    else
                    {
                        endVal[centerX - xIdx, centerY - yIdx] =
                            Mathf.Min(endVal[centerX - xIdx + 1, centerY - yIdx],
                            endVal[centerX - xIdx, centerY - yIdx + 1])
                             + weight[centerX - xIdx, centerY - yIdx];
                    }
                }
                xIdx++;
                yIdx--;
            } while (yIdx > 0);
        }

        if(isDebug)
        {
            MapDebug();
        }
    }

    public void MapDebug()
    {
        string s = "";
        for (int i = 0; i < 33; i++)
        {
            for (int j = 0; j < 33; j++)
            {
                if (endVal[i, j] < 10)
                    s += 0;
                s += endVal[i, j];
                s += "   ";
            }
            s += '\n';
        }

        Debug.Log(s);
    }

    public XY FindNextPos(int x, int y)
    {
        XY xy;

        xy.x = x; 
        xy.y = y;

        if (weight[xy.x, xy.y] > weight[x + 1, y])
        {
            xy.x = x;
            xy.y = y;
        }

        if (weight[xy.x, xy.y] > weight[x - 1, y])
        {
            xy.x = x;
            xy.y = y;
        }

        if (weight[xy.x, xy.y] > weight[x, y + 1])
        {
            xy.x = x;
            xy.y = y;
        }

        if (weight[xy.x, xy.y] > weight[x, y - 1])
        {
            xy.x = x;
            xy.y = y;
        }

        return xy;
    }
}