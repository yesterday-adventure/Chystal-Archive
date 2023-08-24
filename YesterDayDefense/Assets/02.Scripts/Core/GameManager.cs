using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private List<PoolableMono> _poolingList = new List<PoolableMono>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : GameManager is Multiple running!");

        PoolManager.Instance = new PoolManager(transform);
        foreach(var pool in _poolingList)
            PoolManager.Instance.CreatePool(pool);

    }
}