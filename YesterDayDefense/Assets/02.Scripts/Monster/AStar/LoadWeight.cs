using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWeight : MonoBehaviour
{
    public static LoadWeight Instance;

    static public int[,] weight = new int[33, 33];

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : LoadWeight is multiple running!");

        InitWeight();
    }

    private void InitWeight()
    {
        for(int i = 0; i < 33; i++)
        {
            for(int j = 0; j < 33; j++)
            {
                weight[i, j] = 0;
            }
        }
    }

    public void ChangeWeight(int x, int y, int val)
    {
        weight[x,y] += val;
    }
}
