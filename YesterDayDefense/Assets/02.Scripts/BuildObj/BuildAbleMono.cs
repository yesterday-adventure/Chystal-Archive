using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class BuildObjInfo
{
    public GameObject Obj;
    public int Health; //체력
    //public 
}

public class BuildAbleMono : PoolableMono
{
    [Header("건물 속성값")]
    [SerializeField]
    protected int _defaultPrice;                // 기본 가격
    protected int _currentEnhancementPrice;     // 현재 강화 비용
    [SerializeField]
    protected int _enhancementValue; // 현재 강화값

    [SerializeField]
    protected int _currentHealth;
    protected int _maxHealth;

    private int _spentToBuildPrice = 0; // 여태까지 짓는데 사용한 돈
    // 판매할 때는 여때까지 건물에 쓴 돈의 50%
    public int SellPrice => _spentToBuildPrice / 2;
    public int RepairPrice => (int)(_spentToBuildPrice * (1.0f-(float)_currentHealth / _maxHealth) * 0.5f);

    [Header("건물 강화 오브젝트 정보")]
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
