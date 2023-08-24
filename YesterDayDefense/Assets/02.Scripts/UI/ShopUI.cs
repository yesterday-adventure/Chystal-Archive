using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _rectTransform;
    private Scrollbar _scrollbar;
    private readonly int _onPanelY = 0, _offPanelY = -140;
    private bool _isShowed = false;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _scrollbar = _rectTransform.Find("Scrollbar").GetComponent<Scrollbar>();
        ShowPanel(false);
    }

    private void ShowPanel(bool value)
    {
        if(_isShowed == value)
            return;
        _isShowed = value;
        _scrollbar.enabled = value;
        _rectTransform.DOKill();
        _rectTransform.DOAnchorPosY(value ? _onPanelY : _offPanelY, 0.2f);
        //_rectTransform.anchoredPosition = new Vector3(0, , 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowPanel(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ShowPanel(false);
        
    }


}
