using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public static bool IsDrag = false;
    private RectTransform _itemImage;
    public Action<Vector2> DropEvent; // 드래그 앤 드롭 했을 때 이벤트
    private Transform _canvasTrm;

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

        
    }
    //드래그
    public void OnDrag(PointerEventData eventData)
    {
        _itemImage.position = eventData.position;
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
            return;
        GameManager.Instance.SpentMoney(_price);
        DropEvent?.Invoke(eventData.position);
    }

    private void ShowInfoUI(bool show)
    {
        if (IsDrag == true)
            return;
        _slotInfoUI.SetActive(show);
        _slotInfoUI.transform.DOKill();
        _slotInfoUI.transform.DOScaleX(show ? 1 : 0, 0.2f);
    }

}
