using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private PoolableMono _temp;

    public static bool IsDrag = false;
    private RectTransform _itemImage;
    public Action<Vector2> DropEvent; // 드래그 앤 드롭 했을 때 이벤트
    private Transform _canvasTrm;

    //월드포지션 받아와서 설치하는 것
    private Vector3 _tempWorldPos;
    Ray ray;

    private void Awake()
    {
        _canvasTrm = FindObjectOfType<Canvas>().transform;
        _price = _object.DefaultPrice;

        _priceTextUI = _slotInfoUI.transform.Find("PriceText").GetComponent<Text>();
        _priceTextUI.text = _price.ToString();

        _itemImage = transform.Find("ItemImage").GetComponent<RectTransform>();
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
        _itemImage.SetParent(_canvasTrm);
        _itemImage.SetAsLastSibling();

        _temp = PoolManager.Instance.Pop(_object.name);
    }
    //드래그
    public void OnDrag(PointerEventData eventData)
    {
        _itemImage.position = eventData.position;
        ScreenToWorld();
        _tempWorldPos.x = (int)(Mathf.Round(_tempWorldPos.x / 2)) * 2;
        _tempWorldPos.z = (int)(Mathf.Round(_tempWorldPos.z / 2)) * 2;

        if (LoadWeight.Instance.isSetup[(int)(_tempWorldPos.x / 2) + 1, (int)(_tempWorldPos.z / 2) + 1] == null)
            _temp.transform.position = new Vector3(_tempWorldPos.x, 2, _tempWorldPos.z);
    }


    //드랍
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_itemImage.parent == _canvasTrm)
        {
            _itemImage.SetParent(transform);
            _itemImage.localPosition = Vector3.zero;
        }
        IsDrag = false;

        if (GameManager.Instance.Money < _price)
        {
            PoolManager.Instance.Push(_temp);
            return;
        }
        else
        {
            GameManager.Instance.SpentMoney(_price);
            DropEvent?.Invoke(eventData.position);
            LoadWeight.Instance.isSetup[(int)(_tempWorldPos.x / 2) + 1, (int)(_tempWorldPos.z / 2) + 1] = (BuildAbleMono)_temp;
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