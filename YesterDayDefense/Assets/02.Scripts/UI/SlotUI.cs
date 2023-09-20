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
    [Header("���� ����")]
    private int _price;

    [SerializeField]
    private GameObject _slotInfoUI;
    private Text _priceTextUI;

    [SerializeField]
    private BuildAbleMono _object;

    public static bool IsDrag = false;
    private RectTransform _itemImageRectTrm;
    private Image _itemImage;
    public Action<Vector2> DropEvent; // �巡�� �� ��� ���� �� �̺�Ʈ
    private Transform _canvasTrm;

    private void Awake()
    {
        _canvasTrm = FindObjectOfType<Canvas>().transform;
        _price = _object.DefaultPrice;

        _priceTextUI = _slotInfoUI.transform.Find("PriceText").GetComponent<Text>();
        _priceTextUI.text = _price.ToString();

        _itemImageRectTrm = transform.Find("ItemImage").GetComponent<RectTransform>();
        _itemImage = _itemImageRectTrm.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowInfoUI(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ShowInfoUI(false);
    }

    //�巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        ShowInfoUI(false);
        IsDrag = true;
        _itemImageRectTrm.SetParent(_canvasTrm);
        _itemImageRectTrm.SetAsLastSibling();

        _itemImage.color = Color.clear;
    }
    //�巡��
    public void OnDrag(PointerEventData eventData)
    {
        _itemImageRectTrm.position = eventData.position;
    }

    //���
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_itemImageRectTrm.parent == _canvasTrm)
        {
            _itemImageRectTrm.SetParent(transform);
            _itemImageRectTrm.localPosition = Vector3.zero;
        }
        IsDrag = false;
        _itemImage.color = Color.white;

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
