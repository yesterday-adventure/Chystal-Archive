using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("½½·Ô Á¤º¸")]
    [SerializeField] private int _price;

    [SerializeField]
    private GameObject _slotInfoUI;
    private Text _priceTextUI;

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
        _priceTextUI = _slotInfoUI.transform.Find("PriceText").GetComponent<Text>();
        _priceTextUI.text = _price.ToString();
    }

    private void ShowInfoUI(bool show)
    {
        _slotInfoUI.SetActive(show);
    }
}
