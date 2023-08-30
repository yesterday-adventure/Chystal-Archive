using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("½½·Ô Á¤º¸")]
    private int _price;

    [SerializeField]
    private GameObject _slotInfoUI;
    private Text _priceTextUI;

    [SerializeField]
    private BuildAbleMono _object;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowInfoUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShowInfoUI(false);
    }

    private void Awake()
    {
        _price = _object.DefaultPrice;

        _priceTextUI = _slotInfoUI.transform.Find("PriceText").GetComponent<Text>();
        _priceTextUI.text = _price.ToString();
    }

    private void ShowInfoUI(bool show)
    {
        _slotInfoUI.SetActive(show);
        _slotInfoUI.transform.DOKill();
        _slotInfoUI.transform.DOScaleX(show ? 1 : 0, 0.2f);
    }
}
