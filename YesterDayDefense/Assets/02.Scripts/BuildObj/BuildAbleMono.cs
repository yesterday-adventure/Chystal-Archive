using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class BuildObjInfo
{
    public GameObject Obj;
    public int Health; //ü��
    //public 
}

public class BuildAbleMono : PoolableMono
{
    [Header("�ǹ� �Ӽ���")]
    [SerializeField]
    protected int _defaultPrice;                // �⺻ ����
    protected int _currentEnhancementPrice;     // ���� ��ȭ ���
    [SerializeField]
    protected int _enhancementValue; // ���� ��ȭ��

    [SerializeField]
    protected int _currentHealth;
    protected int _maxHealth;

    private int _spentToBuildPrice = 0; // ���±��� ���µ� ����� ��
    // �Ǹ��� ���� �������� �ǹ��� �� ���� 50%
    public int SellPrice => _spentToBuildPrice / 2;
    public int RepairPrice => (int)(_spentToBuildPrice * (1.0f-(float)_currentHealth / _maxHealth) * 0.5f);

    [Header("�ǹ� ��ȭ ������Ʈ ����")]
    [SerializeField]
    protected List<BuildObjInfo> _objects;
    [SerializeField]
    private Vector3 _infoUIOffset;

    protected GameObject _activeObject;

    public void Enhancement()
    {
        if (GameManager.Instance.Money < _currentEnhancementPrice)
            return;

        UIManager.Instance.ShowOffBuildInfoPanel();
        GameManager.Instance.SpentMoney(_currentEnhancementPrice);
        _spentToBuildPrice += _currentEnhancementPrice;
        _currentEnhancementPrice += (int)(_defaultPrice * 0.5f);
        _enhancementValue++;
        
        ShowObject(_enhancementValue);
    }
    protected void ShowObject(int i)
    {
        if (i < 0 || i >= _objects.Count)
            return;

        _currentHealth = _objects[i].Health;
        _maxHealth = _objects[i].Health;

        if(_activeObject != null)
            _activeObject.SetActive(false);

        _activeObject = _objects[i].Obj;
        _activeObject.SetActive(true);
    }

    private void Awake()
    {
        Reset();
    }

    public override void Reset()
    {
        _spentToBuildPrice = _defaultPrice;
        _currentEnhancementPrice = (int)(_defaultPrice * 1.5f);
        _enhancementValue = 0;
        ShowObject(0);
    }
    private void OnMouseDown()
    {
        UIManager.Instance.OpenBuildObjShopPanel(transform.position + _infoUIOffset,
            null, null, null);
    }
    private void OnMouseEnter()
    {
        UIManager.Instance.ShowBuildInfoPanel(transform.position + _infoUIOffset, 
            _currentEnhancementPrice.ToString(), 
            RepairPrice.ToString(), 
            SellPrice.ToString());
    }
    private void OnMouseExit()
    {
        UIManager.Instance.ShowOffBuildInfoPanel();
    }
}
