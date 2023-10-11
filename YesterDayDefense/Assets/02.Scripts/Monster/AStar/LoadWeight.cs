using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

using Random = UnityEngine.Random;
public class LoadWeight : MonoBehaviour
{
    public static LoadWeight Instance;

    public int[,] weight = new int[33, 33];
    public int[,] endVal = new int[33, 33];

    public BuildAbleMono[,] isSetup = new BuildAbleMono[33, 33];

    [SerializeField] bool isDebug = false;

    XY xy;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : LoadWeight is multiple running!");
    }

    /// <summary>
    /// 가중치 값을 바꾼뒤 맵에 적용
    /// </summary>
    /// <param name="x">터렛의 X포지션</param>
    /// <param name="y">터렛의 y포지션</param>
    /// <param name="val">변경될 가중치 값</param>
    /// <param name="originVal">변경되기 전 가중치 값</param>
    public void ChangeWeight(int x, int y, int val, int originVal)
    {
        weight[x, y] -= originVal;
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
            xIdx = i; // 왼쪽 끝에서
            yIdx = 0;
            do
            {
                if (centerX - xIdx >= 1 && centerX - xIdx <= 31 && centerY - yIdx >= 1 && centerY - yIdx <= 31)
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
                if (centerX - xIdx >= 1 && centerX - xIdx <= 31 && centerY - yIdx >= 1 && centerY - yIdx <= 31)
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
                if (centerX - xIdx >= 1 && centerX - xIdx <= 31 && centerY - yIdx >= 1 && centerY - yIdx <= 31)
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
                if (centerX - xIdx >= 1 && centerX - xIdx <= 31 && centerY - yIdx >= 1 && centerY - yIdx <= 31)
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

        for (int i = 1; i <= 32; i++)
        {
            xIdx = i; // 왼쪽 끝에서
            yIdx = 0;
            do
            {
                if (centerX - xIdx >= 1 && centerX - xIdx <= 31 && centerY - yIdx >= 1 && centerY - yIdx <= 31)
                {
                    endVal[centerX - xIdx, centerY - yIdx] =
                        Mathf.Min(endVal[centerX - xIdx + 1, centerY - yIdx],
                        Mathf.Min(endVal[centerX - xIdx - 1, centerY - yIdx],
                        Mathf.Min(endVal[centerX - xIdx, centerY - yIdx + 1],
                        endVal[centerX - xIdx, centerY - yIdx - 1]))) +
                        weight[centerX - xIdx, centerY - yIdx];
                }
                xIdx--;
                yIdx--;
            } while (xIdx > 0);

            do
            {
                if (centerX - xIdx >= 1 && centerX - xIdx <= 31 && centerY - yIdx >= 1 && centerY - yIdx <= 31)
                {
                    endVal[centerX - xIdx, centerY - yIdx] =
                            Mathf.Min(endVal[centerX - xIdx + 1, centerY - yIdx],
                            Mathf.Min(endVal[centerX - xIdx - 1, centerY - yIdx],
                            Mathf.Min(endVal[centerX - xIdx, centerY - yIdx + 1],
                            endVal[centerX - xIdx, centerY - yIdx - 1]))) +
                            weight[centerX - xIdx, centerY - yIdx];
                }
                xIdx--;
                yIdx++;
            } while (yIdx < 0);

            do
            {
                if (centerX - xIdx >= 1 && centerX - xIdx <= 31 && centerY - yIdx >= 1 && centerY - yIdx <= 31)
                {
                    endVal[centerX - xIdx, centerY - yIdx] =
                            Mathf.Min(endVal[centerX - xIdx + 1, centerY - yIdx],
                            Mathf.Min(endVal[centerX - xIdx - 1, centerY - yIdx],
                            Mathf.Min(endVal[centerX - xIdx, centerY - yIdx + 1],
                            endVal[centerX - xIdx, centerY - yIdx - 1]))) +
                            weight[centerX - xIdx, centerY - yIdx];
                }
                xIdx++;
                yIdx++;
            } while (xIdx < 0);

            do
            {
                if (centerX - xIdx >= 1 && centerX - xIdx <= 31 && centerY - yIdx >= 1 && centerY - yIdx <= 31)
                {
                    endVal[centerX - xIdx, centerY - yIdx] =
                            Mathf.Min(endVal[centerX - xIdx + 1, centerY - yIdx],
                            Mathf.Min(endVal[centerX - xIdx - 1, centerY - yIdx],
                            Mathf.Min(endVal[centerX - xIdx, centerY - yIdx + 1],
                            endVal[centerX - xIdx, centerY - yIdx - 1]))) +
                            weight[centerX - xIdx, centerY - yIdx];
                }
                xIdx++;
                yIdx--;
            } while (yIdx > 0);


        }

        for (int i = 1; i < 32; i++)
        {
            endVal[i, 1] = 500;
            endVal[i, 2] = 500;
            endVal[i, 3] = 500;
            endVal[i, 31] = 500;
            endVal[i, 30] = 500;
            endVal[i, 29] = 500;
            endVal[1, i] = 500;
            endVal[2, i] = 500;
            endVal[3, i] = 500;
            endVal[31, i] = 500;
            endVal[30, i] = 500;
            endVal[29, i] = 500;
        }
        if (isDebug)
        {
            MapDebug();
        }
    }

    public void MapDebug()
    {
        string s = "";
        for (int i = 1; i < 32; i++)
        {
            for (int j = 1; j < 32; j++)
            {
                if (endVal[i, j] < 100)
                    s += 0;
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
        xy.x = x;
        xy.y = y;

        int randVal = Random.Range(0, 4);

        if (randVal == 0)
            CheckLeft(x, y, 0);
        else if (randVal == 1)
            CheckRight(x, y, 0);
        else if (randVal == 2)
            CheckUp(x, y, 0);
        else if (randVal == 3)
            CheckDown(x, y, 0);


        return xy;
    }

    private void CheckLeft(int x, int y, int idx)
    {
        if (idx == 4)
            return;

        if (endVal[xy.x, xy.y] > endVal[x - 1, y] && x - 1 > 0)
        {
            xy.x = x - 1;
            xy.y = y;
        }

        CheckRight(x, y, idx + 1);
    }

    private void CheckRight(int x, int y, int idx)
    {
        if (idx == 4)
            return;

        if (endVal[xy.x, xy.y] > endVal[x + 1, y] && x + 1 < 32)
        {
            xy.x = x + 1;
            xy.y = y;
        }

        CheckUp(x, y, idx + 1);
    }

    private void CheckUp(int x, int y, int idx)
    {
        if (idx == 4)
            return;

        if (endVal[xy.x, xy.y] > endVal[x, y + 1] && y + 1 < 32)
        {
            xy.x = x;
            xy.y = y + 1;
        }

        CheckDown(x, y, idx + 1);
    }

    private void CheckDown(int x, int y, int idx)
    {
        if (idx == 4)
            return;

        if (endVal[xy.x, xy.y] > endVal[x, y - 1] && y - 1 > 0)
        {
            xy.x = x;
            xy.y = y - 1;
        }

        CheckLeft(x, y, idx + 1);
    }
}