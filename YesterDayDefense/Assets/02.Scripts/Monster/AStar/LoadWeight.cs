using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWeight : MonoBehaviour
{
    public static LoadWeight Instance;

    public int[,] weight = new int[33, 33];

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
}
