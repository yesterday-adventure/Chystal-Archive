using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [Header("HP 속성값")]
    [SerializeField]
    private int _maxHp;
    [SerializeField]
    private int _currentHp;

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
        _currentHp = _maxHp;
        _hpFrameImage = _hpFrameRectTrm.GetComponent<Image>();
        _hpColor1 = _hpImage1.color;
        _hpColor2 = _hpImage2.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ReceiveDamage(20);
    }

    public void ReceiveDamage(int damage)
    {
        _currentHp = Mathf.Clamp(_currentHp - damage, 0, _maxHp);
        TweenHPAnim();
    }

    private void TweenHPAnim()
    {
        Sequence seq = DOTween.Sequence();
        Sequence seq2 = DOTween.Sequence();
        float moveYValue = Mathf.Lerp(_deathY, _defaultY, (float)_currentHp / _maxHp);
        float currentY = _hpRectTrm.anchoredPosition.y;

        seq.Append(_hpFrameImage.DOColor(Color.red, 0.05f))
            .Join(_hpImage1.DOColor(Color.red, 0.05f))
            .Join(_hpImage2.DOColor(Color.red, 0.05f))
            .Append(_hpFrameImage.DOColor(Color.white, 0.1f))
            .Join(_hpImage1.DOColor(_hpColor1, 0.05f))
            .Join(_hpImage2.DOColor(_hpColor2, 0.05f));

        seq2.Append(_hpRectTrm.DOAnchorPosY(currentY + _damagedYOffset, 0.05f))
            .Join(_hpFrameRectTrm.DOScale(_damagedScale, 0.05f))
            .Append(_hpRectTrm.DOAnchorPosY(moveYValue, 0.1f))
            .Join(_hpFrameRectTrm.DOScale(_defaultScale, 0.1f))
            .Append(_hpRectTrm.DOShakeAnchorPos(1f, 20f, 10, 90f));
            
    }
}
