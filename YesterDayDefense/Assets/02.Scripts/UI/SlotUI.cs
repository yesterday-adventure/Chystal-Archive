using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct BlockInfo
{
    public string name;
    public int health;
    public int attackDamage;
    public float attackDist;
}

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("슬롯 정보")]
    private int _price;

    [SerializeField]
    private GameObject _slotInfoUI;
    private Text _priceTextUI;

    [SerializeField]
    private BuildAbleMono _object;

    private BuildAbleMono _temp;
    private TurretAI _tempTurretAI;

    public static bool IsDrag = false;
    private RectTransform _itemImageRectTrm;
    private Image _itemImage;
    public Action<Vector2> DropEvent; // 드래그 앤 드롭 했을 때 이벤트
    private Transform _canvasTrm;

    private Text _nameText;
    private Text _hpText;
    private Text _damageText;
    private Text _rangeText;

    private bool _lastShopUIPointEnter = false;

    //월드포지션 받아와서 설치하는 것
    private Vector3 _tempWorldPos;
    Ray ray;

    private void Awake()
    {
        _canvasTrm = FindObjectOfType<Canvas>().transform;
        _price = _object.DefaultPrice;

        _priceTextUI = _slotInfoUI.transform.Find("PricePanel/PriceText").GetComponent<Text>();
        _priceTextUI.text = _price.ToString();

        _itemImageRectTrm = transform.Find("ItemImage").GetComponent<RectTransform>();
        _itemImage = _itemImageRectTrm.GetComponent<Image>();

        BlockInfo info = _object.BlockInfo;

        _nameText = _slotInfoUI.transform.Find("NamePanel/NameText").GetComponent<Text>();
        _nameText.text = info.name;
        _hpText = _slotInfoUI.transform.Find("InfoPanel/HPText").GetComponent<Text>();
        _damageText = _slotInfoUI.transform.Find("InfoPanel/DamageText").GetComponent<Text>();
        _rangeText = _slotInfoUI.transform.Find("InfoPanel/RangeText").GetComponent<Text>();

        _hpText.text = (info.health.ToString());
        _damageText.text = (info.attackDamage == 0 ? 
            "-" : info.attackDamage.ToString());
        _rangeText.text = (info.attackDist == 0 ?
            "-" : info.attackDist.ToString());


        //_object.BlockInfo
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowInfoUI(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowInfoUI(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ShowInfoUI(false);
    }

    //드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        ShowInfoUI(false);
        IsDrag = true;
        _itemImageRectTrm.SetParent(_canvasTrm);
        _itemImageRectTrm.SetAsLastSibling();

        _temp = PoolManager.Instance.Pop(_object.name) as BuildAbleMono;
        if(_temp.GetComponentInChildren<TurretAI>() != null)
            _tempTurretAI = _temp.GetComponentInChildren<TurretAI>();
        _itemImage.raycastTarget = false;
        _lastShopUIPointEnter = true;
    }
    //드래그
    public void OnDrag(PointerEventData eventData)
    {
        _itemImageRectTrm.position = eventData.position;
        ScreenToWorld();
        _tempWorldPos.x = (int)(Mathf.Round(_tempWorldPos.x / 2)) * 2;
        _tempWorldPos.z = (int)(Mathf.Round(_tempWorldPos.z / 2)) * 2;

        if (ShopUI.IsPointEnter != _lastShopUIPointEnter)
        {
            if (ShopUI.IsPointEnter == true)
                _itemImage.color = Color.white;
            else
                _itemImage.color = Color.clear;
            _lastShopUIPointEnter = ShopUI.IsPointEnter;
        }
            

        if (LoadWeight.Instance.isSetup[(int)(_tempWorldPos.x / 2) + 1, (int)(_tempWorldPos.z / 2) + 1] == null)
            _temp.transform.position = new Vector3(_tempWorldPos.x, 2, _tempWorldPos.z);

        _temp.SetXY((int)(_tempWorldPos.x / 2) + 1, (int)(_tempWorldPos.z / 2) + 1);
    }
    //드랍
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_itemImageRectTrm.parent == _canvasTrm)
        {
            _itemImageRectTrm.SetParent(transform);
            _itemImageRectTrm.localPosition = Vector3.zero;
            _itemImage.color = Color.white;
            _itemImage.raycastTarget = true;
        }
        IsDrag = false;

        if (GameManager.Instance.Money < _price
            || ShopUI.IsPointEnter == true)
        {
            PoolManager.Instance.Push(_temp);
            DropEvent?.Invoke(eventData.position);
            return;
        }
        else
        {
            if(_tempTurretAI != null)
                _tempTurretAI.canAttack = true;
            GameManager.Instance.SpentMoney(_price);
            DropEvent?.Invoke(eventData.position);
            LoadWeight.Instance.isSetup[(int)(_tempWorldPos.x / 2) + 1, (int)(_tempWorldPos.z / 2) + 1] = _temp;
        }
        _temp = null;
    }

    private void ShowInfoUI(bool show)
    {
        if (IsDrag == true)
            return;
        _slotInfoUI.SetActive(show);
        _slotInfoUI.transform.DOKill();
        _slotInfoUI.transform.DOScaleX(show ? 1 : 0, 0.2f);
    }

    private void ScreenToWorld()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            _tempWorldPos = hit.point;
        }
    }
}