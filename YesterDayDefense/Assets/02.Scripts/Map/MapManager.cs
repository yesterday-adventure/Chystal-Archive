using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    static private int _mapWidth = 32;// �� ����
    static private int _mapHeight = 32;// �� ����

    [Header("��")]
    [SerializeField] private int[] _x;// �� x�ε���
    [SerializeField] private int[] _y;// �� y�ε���
    //�߰��ϱ� ���϶�� ������ �� ������ �ٸ��� ������

    [Header("�� ������")]
    [SerializeField] PoolableMono _grass;
    [SerializeField] PoolableMono _water;

    [Header("��� ������")]
    [SerializeField] private int _tileX;// Ÿ�� ũ�� X
    [SerializeField] private int _tileY;// Ÿ�� ũ�� Y

    [Header("�ھ� ��ġ")]//�ظ��ؼ� �Ȱǵ�°� ����
    [SerializeField] private int _coreX = 16;
    [SerializeField] private int _coreY = 16; 
    public int CoreX => _coreX;
    public int CoreY => _coreY;

    public Map[,] zone = new Map[_mapHeight + 1,_mapWidth + 1]; // 0���ε����� �� �� ����

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