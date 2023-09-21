using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [Header("HP 애니메이션 값")]
    [SerializeField]
    private RectTransform _hpFrameRectTrm;
    private Image _hpFrameImage;
    [SerializeField]
    private Image _hpImage1;
    [SerializeField]
    private Image _hpImage2;
    private Color _hpColor1, _hpColor2;

    [SerializeField]
    private RectTransform _hpRectTrm;
    [SerializeField]
    private float _defaultY;
    private float _lastY = 0;
    [SerializeField]
    private float _deathY;
    [SerializeField]
    private float _damagedYOffset;
    [SerializeField]
    private Vector2 _defaultScale;
    [SerializeField]
    private Vector2 _damagedScale;

    private void Awake()
    {
        _hpFrameImage = _hpFrameRectTrm.GetComponent<Image>();
        _hpColor1 = _hpImage1.color;
        _hpColor2 = _hpImage2.color;
    }

    public void TweenHPAnim(float hpPer)
    {
        Sequence seq = DOTween.Sequence();
        Sequence seq2 = DOTween.Sequence();
        float moveYValue = Mathf.Lerp(_deathY, _defaultY, hpPer);

        float currentY = _lastY;
        _lastY = moveYValue;

        seq.Append(_hpFrameImage.DOColor(Color.red, 0.05f))
            .Join(_hpImage1.DOColor(Color.red, 0.05f))
            .Join(_hpImage2.DOColor(Color.red, 0.05f))
            .Append(_hpFrameImage.DOColor(Color.white, 0.1f))
            .Join(_hpImage1.DOColor(_hpColor1, 0.05f))
            .Join(_hpImage2.DOColor(_hpColor2, 0.05f));

        seq2.Append(_hpRectTrm.DOAnchorPosY(currentY + _damagedYOffset, 0.05f))
            .Join(_hpFrameRectTrm.DOScale(_damagedScale, 0.05f))
            .Append(_hpRectTrm.DOAnchorPosY(moveYValue, 0.1f))
            .Join(_hpFrameRectTrm.DOScale(_defaultScale, 0.1f));
    }
}
