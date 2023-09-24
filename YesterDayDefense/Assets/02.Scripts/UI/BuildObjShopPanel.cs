using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildObjShopPanel : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler
{
    public bool MouseHover { get; private set; } = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseHover = false;
    }
}
