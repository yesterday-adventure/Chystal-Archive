using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    static private int _mapWidth = 16;
    static private int _mapHeight = 8;

    [Header("°­")]
    [SerializeField] private int[] x;
    [SerializeField] private int[] y;

    [Header("¸Ê ÇÁ¸®ÆÕ")]

    public Map[,] zone;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : MapManager is Multiple running!");

        if (x.Length != y.Length)
            Debug.LogError($"{transform} : Other Length Map");

        InitMap();
        SpawnMap();
    }

    private void InitMap()
    {
        for(int i = 1; i <= _mapHeight; i++)
        {
            for(int j = 1; j <= _mapWidth; j++)
            {
                zone[i,j] = Map.Grass;
            }
        }

        for (int i = 0; i < x.Length; i++)
        {
            zone[x[i], y[i]] = Map.Water;
        }
    }

    private void SpawnMap()
    {

    }
}
