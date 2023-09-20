using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class ShopUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _rectTransform;
    private Scrollbar _scrollbar;
    private readonly int _onPanelY = 0, _offPanelY = -140;
    private bool _isShowed = false;
    public static bool IsPointEnter = false;

    [SerializeField]
    private Transform _content;
    private List<SlotUI> _slotUIList = new List<SlotUI>();


    private void Awake()
    {
        _slotUIList = _content.GetComponentsInChildren<SlotUI>().ToList<SlotUI>();

        _rectTransform = GetComponent<RectTransform>();
        _scrollbar = _rectTransform.Find("Scrollbar").GetComponent<Scrollbar>();
        ShowPanel(false);

        foreach (var slot in _slotUIList)
            slot.DropEvent += (pos) => ShowPanel(false);
    }

    private void ShowPanel(bool value)
    {
        IsPointEnter = value;
        if(_isShowed == value || SlotUI.IsDrag == true)
            return;
        _isShowed = value;
        _scrollbar.enabled = value;
        _rectTransform.DOKill();
        _rectTransform.DOAnchorPosY(value ? _onPanelY : _offPanelY, 0.2f);
        //_rectTransform.anchoredPosition = new Vector3(0, , 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("a");
        ShowPanel(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("b");
        ShowPanel(false);
        
    }


}
