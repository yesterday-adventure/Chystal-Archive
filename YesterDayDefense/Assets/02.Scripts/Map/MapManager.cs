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

    [Header("���� ������")]
    [SerializeField] PoolableMono[] _deco;

    [Header("��� ������")]
    [SerializeField] private int _tileX;// Ÿ�� ũ�� X
    [SerializeField] private int _tileY;// Ÿ�� ũ�� Y

    [Header("�ھ�")]//�ظ��ؼ� �Ȱǵ�°� ����
    [SerializeField] private GameObject _core;
    [SerializeField] private int _coreX = 16;
    [SerializeField] private int _coreY = 16; 
    public int CoreX => _coreX;
    public int CoreY => _coreY;

    [Header("�� ����")]
    public Map[,] zone = new Map[_mapHeight + 1,_mapWidth + 1]; // 0���ε����� �� �� ����
    public int[,] monterCnt = new int[_mapHeight + 1,_mapWidth + 1]; // ���� ��� �ִ���
    public bool[,] isturret = new bool[_mapHeight + 1, _mapWidth + 1]; // ��ž�� ��� �ִ���

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : MapManager is Multiple running!");

        if (_x.Length != _y.Length)
            Debug.LogError($"{transform} : Other Length Map");

    }

    private void Start()
    {
        InitMap();
        SpawnMap();
        LoadWeight.Instance.InitEndVal();
        SetPostion(_core,_coreX, _coreY);
    }

    private void InitMap()
    {
        for(int i = 1; i < _mapHeight; i++)
        {
            for(int j = 1; j < _mapWidth; j++)
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
        PoolableMono deco = null;
        for(int i = 1; i < _mapHeight; i++)
        {
            for(int j = 1; j < _mapWidth; j++)
            {
                if (zone[i, j] == Map.Grass)
                {
                    tile = PoolManager.Instance.Pop("Grass");
                    tile.transform.position = new Vector3((j - 1) * _tileX, 0, (i - 1) * _tileY);
                }
                else if (zone[i, j] == Map.Water)
                {
                    tile = PoolManager.Instance.Pop("Water");
                    tile.transform.position = new Vector3((j - 1) * _tileX, 2, (i - 1) * _tileY); // �� plane�̶� ������ �ʿ�
                }

                if(i <= 3 || i >= 29 || j <= 3 || j >= 29)
                {
                    deco = PoolManager.Instance.Pop(_deco[Random.Range(0, _deco.Length)].name);
                    deco.transform.position = new Vector3((j - 1) * _tileX, 2, (i - 1) * _tileY);
                }
            }
        }
    }

    public void SetPostion(GameObject obj, int x, int y)
    {
        obj.transform.position = new Vector3((x - 1) * _tileX, 2, (y - 1) * _tileY);
    }

    public XY GetPostion(int x, int y)
    {
        XY xy;
        x = (x - 1) * _tileX;
        y = (y - 1) * _tileY;

        xy.x = x; 
        xy.y = y;

        return xy;
    }

    public XY GetMapPos(int x, int y)
    {
        XY xy;
        xy.x = x / 2;
        xy.y = y / 2;
        return xy;
    }
}