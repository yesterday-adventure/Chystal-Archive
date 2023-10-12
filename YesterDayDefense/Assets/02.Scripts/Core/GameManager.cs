using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Transform _gameUIPanel;
    [SerializeField]
    private GameObject _clearPanel;
    [SerializeField]
    private GameObject _gameOverPanel;

    [SerializeField]
    private List<PoolableMono> _poolingList = new List<PoolableMono>();

    [SerializeField]
    private List<PoolableMono> _monsterpoolingList = new List<PoolableMono>();

    [field:SerializeField]
    public int Money { get; private set; } = 0;

    [field: SerializeField]
    public int TurretCount { get; private set; } = 0;
    public float TurretVat => (1f + (0.201f * TurretCount));

    public Transform monsterParentTrm;
    private UIManager _uiManager;

    public bool isGameClear = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : GameManager is Multiple running!");

        if(_gameUIPanel != null)
            _uiManager = UIManager.Instance = new UIManager(_gameUIPanel);
        if (_uiManager == null)
            Debug.LogError("NoUIManager");
        _uiManager.SetMoneyText(Money);

        PoolManager.Instance = new PoolManager(transform);
        foreach(var pool in _poolingList)
            PoolManager.Instance.CreatePool(pool);
        foreach (var pool in _monsterpoolingList)
            PoolManager.Instance.CreatePool(pool,0);
    }

    public void PlusMoney(int plus)
    {
        Money += plus;
        _uiManager.SetMoneyText(Money);
    }
    public void SpentMoney(int spent)
    {
        Money -= spent;
        _uiManager.SetMoneyText(Money, true);
    }

    public void PlusTurret()
    {
        TurretCount++;
    }
    public void MinusTurret()
    {
        TurretCount--;
    }

    public void GameOver()
    {
        Monster[] monsters = monsterParentTrm.GetComponentsInChildren<Monster>();
        foreach(Monster monster in monsters)
        {
            monster.GameOver();
        }

        _gameOverPanel.SetActive(true);
    }

    private void Update()
    {
        if (isGameClear && _clearPanel.activeSelf == false)
            _clearPanel.SetActive(true);

    }
}