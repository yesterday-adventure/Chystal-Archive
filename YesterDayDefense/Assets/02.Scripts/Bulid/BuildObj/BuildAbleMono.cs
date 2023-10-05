using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class BuildObjInfo
{
    public GameObject Obj;
    public int Health; //ü��
    public Vector3 InfoUIOffset;
    //public 
}

public class BuildAbleMono : PoolableMono
{
    [SerializeField]
    protected bool useInfoUI = true;

    [Header("����ġ")]
    [SerializeField] private int _weight;
    public int Weight => _weight;

    [Header("�ǹ� �Ӽ���")]
    [SerializeField]
    private BlockInfo _blockinfo;
    public BlockInfo BlockInfo => _blockinfo;

    [SerializeField]
    protected int _defaultPrice;                // �⺻ ����
    protected int _currentEnhancementPrice;     // ���� ��ȭ ���
    [SerializeField]
    protected int _enhancementValue; // ���� ��ȭ��

    [SerializeField]
    protected int _currentHealth;
    protected int _maxHealth;

    public int SlotUIHealth => _objects[0].Health;

    private int _spentToBuildPrice = 0; // ���±��� ���µ� ����� ��
    // �Ǹ��� ���� �������� �ǹ��� �� ���� 50%
    public int SellPrice => _spentToBuildPrice / 2;
    public int RepairPrice => (int)(_spentToBuildPrice * (1.0f-(float)_currentHealth / _maxHealth) * 0.5f);
    public int DefaultPrice => _defaultPrice;

    public bool FullEnhancement => _enhancementValue == _objects.Count - 1;
    private bool _isShowBuildInfo = false;

    [Header("�ǹ� ��ȭ ������Ʈ ����")]
    [SerializeField]
    protected List<BuildObjInfo> _objects;

    protected BuildObjInfo _activeObject;

    private int x;
    private int y;
    private void DestroyObject() //�ı���
    {
        if (_isShowBuildInfo == false)
            return;

        if (UIManager.Instance.GetBuildShopPanelShowed == true)
            CloseShop();
        else if (UIManager.Instance.GetBuildInfoPanelShowed == true)
            CloseInfo();

        //����ġ ����
        LoadWeight.Instance.ChangeWeight(x, y, 0, _weight);


        PoolManager.Instance.Push(this);
    }
    public void Enhancement()
    {
        if (GameManager.Instance.Money < _currentEnhancementPrice)
            return;

        UIManager.Instance.CloseBuildObjShopPanel();
        GameManager.Instance.SpentMoney(_currentEnhancementPrice);
        _spentToBuildPrice += _currentEnhancementPrice;
        _currentEnhancementPrice += (int)(_defaultPrice * 0.5f);
        _enhancementValue++;
        
        ShowObject(_enhancementValue);
    }
    public void Repair()
    {
        if (GameManager.Instance.Money < RepairPrice)
            return;

        GameManager.Instance.SpentMoney(RepairPrice);
        _currentHealth = _maxHealth;
        UIManager.Instance.BuildInfoUpdate(
            RepairPrice.ToString(), (float)_currentHealth / _maxHealth);
    }
    public void Sell()
    {
        //���߿� ����ġ ���� ����
        GameManager.Instance.PlusMoney(SellPrice);
        DestroyObject();
    }

    public virtual void Damage(int damage)
    {
        _currentHealth -= damage;

        if(_isShowBuildInfo == true)
        {
            UIManager.Instance.BuildInfoUpdate(
                RepairPrice.ToString(), (float)_currentHealth / _maxHealth);
        }
        
        if (_currentHealth <= 0)
        {
            PoolManager.Instance.Push(this);
            LoadWeight.Instance.isSetup[x, y] = null;
        }
    }
    protected void ShowObject(int i)
    {
        if (i < 0 || i >= _objects.Count)
            return;

        _currentHealth = _objects[i].Health;
        _maxHealth = _objects[i].Health;

        if(_activeObject != null)
            _activeObject.Obj.SetActive(false);

        _activeObject = _objects[i];
        _activeObject.Obj.SetActive(true);
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

    public void SetXY(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
    public void ShowInfo(bool show)
    {
        if (show)
            OpenInfo();
        else
            CloseInfo();
    }
    public void ShowShop(bool show)
    {
        if (show)
            OpenShop();
        else
            CloseShop();
    }
    private void OpenInfo()
    {
        if (useInfoUI == false)
            return;

        if (CameraMove.IsMoveScreen == true || SlotUI.IsDrag == true)
            return;

        _isShowBuildInfo = true;
        UIManager.Instance.OpenBuildInfoPanel(transform.position + _activeObject.InfoUIOffset,
            !FullEnhancement ? _currentEnhancementPrice.ToString() : "X",
            RepairPrice.ToString(),
            SellPrice.ToString(),
            (float)_currentHealth / _maxHealth);
    }
    private void CloseInfo()
    {
        _isShowBuildInfo = false;
        UIManager.Instance.CloseBuildInfoPanel();
    }
    private void OpenShop()
    {
        if (useInfoUI == false)
            return;

        if (UIManager.Instance.GetBuildInfoPanelShowed == false)
        {
            OpenInfo();
        }
        UIManager.Instance.OpenBuildObjShopPanel(transform.position + _activeObject.InfoUIOffset,
        !FullEnhancement ? Enhancement : null,
        Repair, Sell);
    }
    private void CloseShop()
    {
        UIManager.Instance.CloseBuildObjShopPanel();
    }
}
