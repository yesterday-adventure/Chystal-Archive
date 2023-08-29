using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWeight : MonoBehaviour
{
    public static LoadWeight Instance;

    public int[,] weight = new int[33, 33];
    public int[,] endVal = new int[33, 33];

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : LoadWeight is multiple running!");
    }

    public void ChangeWeight(int x, int y, int val)
    {
        weight[x,y] += val;
    }

    public void InitEndVal()
    {
        for(int i = 0; i < 33; i++)
        {
            for(int j = 0; j < 33; j++)
            {
                endVal[i,j] = 1;
            }
        }

        int centerX = MapManager.Instance.CoreX;
        int centerY = MapManager.Instance.CoreY;
        endVal[centerX, centerY] = 0;

        int xIdx = 0;
        int yIdx = 0;
        for(int i = 1; i <= 32; i++)
        {
            yIdx = 0;
            xIdx = i; // ¿ÞÂÊ ³¡¿¡¼­
            do
            {
                if (yIdx == 0)
                {
                    endVal[xIdx,yIdx] = endVal[xIdx + 1,yIdx] + weight[xIdx, yIdx];
                }
                else
                {
                    endVal[xIdx, yIdx] = 
                        Mathf.Min(endVal[xIdx + 1, yIdx], endVal[xIdx, yIdx - 1]);
                }
                yIdx++;
                xIdx--;
            }while (xIdx > 0) ;
            
            do
            {
                if (yIdx == i)
                {
                    endVal[xIdx, yIdx] = endVal[xIdx + 1, yIdx] + weight[xIdx, yIdx];
                }
                else
                {
                    endVal[xIdx, yIdx] =
                        Mathf.Min(endVal[xIdx + 1, yIdx], endVal[xIdx, yIdx - 1]);
                }
                yIdx++;
                xIdx--;
            } while (xIdx > 0);
            
            do
            {
                if (yIdx == 0)
                {
                    endVal[xIdx, yIdx] = endVal[xIdx + 1, yIdx] + weight[xIdx, yIdx];
                }
                else
                {
                    endVal[xIdx, yIdx] =
                        Mathf.Min(endVal[xIdx + 1, yIdx], endVal[xIdx, yIdx - 1]);
                }
                yIdx++;
                xIdx--;
            } while (xIdx > 0);
            
            do
            {
                if (yIdx == 0)
                {
                    endVal[xIdx, yIdx] = endVal[xIdx + 1, yIdx] + weight[xIdx, yIdx];
                }
                else
                {
                    endVal[xIdx, yIdx] =
                        Mathf.Min(endVal[xIdx + 1, yIdx], endVal[xIdx, yIdx - 1]);
                }
                yIdx++;
                xIdx--;
            } while (xIdx > 0);

        }
    }
}
