using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HoverScaleUP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Vector3 _defaultScale = Vector3.one;
    [SerializeField]
    private Vector3 _upScale = new Vector3(1.2f, 1.2f, 1);

    [SerializeField]
    private float _tweenTime = 0.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        DoScaleChange(_upScale, _tweenTime);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DoScaleChange(_defaultScale, _tweenTime);
    }

    private void DoScaleChange(Vector3 scale, float time)
    {
        transform.DOKill();
        transform.DOScale(scale, time);
    }
}
