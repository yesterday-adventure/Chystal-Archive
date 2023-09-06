using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Transform _gameUIPanel;

    [SerializeField]
    private List<PoolableMono> _poolingList = new List<PoolableMono>();

    [field:SerializeField]
    public int Money { get; private set; } = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : GameManager is Multiple running!");

        if(_gameUIPanel != null)
            UIManager.Instance = new UIManager(_gameUIPanel);

        PoolManager.Instance = new PoolManager(transform);
        foreach(var pool in _poolingList)
            PoolManager.Instance.CreatePool(pool);

    }

    public void PlusMoney(int plus)     => Money += plus;
    public void SpentMoney(int spent)   => Money -= spent;
}