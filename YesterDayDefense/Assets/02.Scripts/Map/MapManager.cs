using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    static private int _mapWidth = 32;// 맵 가로
    static private int _mapHeight = 32;// 맵 세로

    [Header("강")]
    [SerializeField] private int[] _x;// 강 x인덱스
    [SerializeField] private int[] _y;// 강 y인덱스
    //추가하기 편하라고 일케함 둘 사이즈 다르면 오류남

    [Header("맵 프리팹")]
    [SerializeField] PoolableMono _grass;
    [SerializeField] PoolableMono _water;

    [Header("블록 사이즈")]
    [SerializeField] private int _tileX;// 타일 크기 X
    [SerializeField] private int _tileY;// 타일 크기 Y

    [Header("코어 위치")]//왠만해선 안건드는게 좋음
    [SerializeField] private int _coreX = 16;
    [SerializeField] private int _coreY = 16; 
    public int CoreX => _coreX;
    public int CoreY => _coreY;

    public Map[,] zone = new Map[_mapHeight + 1,_mapWidth + 1]; // 0번인덱스는 안 쓸 예정

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : MapManager is Multiple running!");

        if (_x.Length != _y.Length)
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
                LoadWeight.Instance.weight[i,j] = 1;
            }
        }

        for (int i = 0; i < _x.Length; i++)
        {
            zone[_x[i], _y[i]] = Map.Water;
        }
    }

    private void SpawnMap()
    {
        PoolableMono tile = null;
        for(int i = 1; i <= _mapHeight; i++)
        {
            for(int j = 1; j <= _mapWidth; j++)
            {
                if (zone[i, j] == Map.Grass)
                    tile = PoolManager.Instance.Pop("Grass");
                else if (zone[i, j] == Map.Water)
                    tile = PoolManager.Instance.Pop("Water");

                tile.transform.position = new Vector3((j - 1) * _tileX, 0, (i - 1) * _tileY);
            }
        }
    }
}