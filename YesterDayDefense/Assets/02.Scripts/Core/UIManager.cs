using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    public static UIManager Instance;
    private Transform _buildInfoPanel;
    private Text _enhancementPriceText;
    private Text _repairPriceText;
    private Text _sellPriceText;

    private Transform _buildObjShopPanel;
    private Button _enhancementBtn;
    private Button _repairBtn;
    private Button _sellBtn;

    private TextMeshProUGUI _moneyText;

    private HP _hpUI;
    public HP HPUI => _hpUI;

    private bool _isShowBuildInfoPanel = false;
    private bool _isShowBuildShopPanel = false;
    public bool GetBuildInfoPanelShowed => _isShowBuildInfoPanel;
    public bool GetBuildShopPanelShowed => _isShowBuildShopPanel;


    public UIManager(Transform gameUIPanel)
    {
        _buildInfoPanel = gameUIPanel.Find("BuildObjInfoPanel").transform;
        _enhancementPriceText = _buildInfoPanel.Find("enhancementPriceText").GetComponent<Text>();
        _repairPriceText = _buildInfoPanel.Find("repairPriceText").GetComponent<Text>();
        _sellPriceText = _buildInfoPanel.Find("sellPriceText").GetComponent<Text>();

        _buildObjShopPanel = gameUIPanel.Find("BuildObjShopPanel").transform;
        _enhancementBtn = _buildObjShopPanel.Find("EnhancementBtn").GetComponent<Button>();
        _repairBtn = _buildObjShopPanel.Find("RepairBtn").GetComponent<Button>();
        _sellBtn = _buildObjShopPanel.Find("SellBtn").GetComponent<Button>();

        _moneyText = gameUIPanel.Find("IngamePanel/MoneyPos/Money").GetComponent<TextMeshProUGUI>();
        _hpUI = gameUIPanel.Find("IngamePanel/HPPos/HP").GetComponent<HP>();
    }

    public void OpenBuildInfoPanel(Vector3 pos, string enhancementPrice, string repairPrice, string sellPrice)
    {
        if (_isShowBuildInfoPanel == true)
            return;

        _isShowBuildInfoPanel = true;

        _enhancementPriceText.text = enhancementPrice;
        _repairPriceText.text       = repairPrice;
        _sellPriceText.text         = sellPrice;

        _buildInfoPanel.position = Camera.main.WorldToScreenPoint(pos);
        _buildInfoPanel.gameObject.SetActive(true);
        _buildInfoPanel.DOKill();
        _buildInfoPanel.localScale = new Vector3(0, 1, 1);
        _buildInfoPanel.DOScaleX(1, 0.2f);
    }
    public void CloseBuildInfoPanel()
    {
        if (_isShowBuildInfoPanel == false || _isShowBuildShopPanel == true)
            return;

        _isShowBuildInfoPanel = false;

        Sequence seq = DOTween.Sequence();
        seq.Append(_buildInfoPanel.DOScaleX(0, 0.2f))
            .OnComplete(() =>
            {
                _buildInfoPanel.gameObject.SetActive(false);
            });
        
    }
    public void OpenBuildObjShopPanel(Vector3 pos, Action enhancement, Action repair, Action sell)
    {
        if(_isShowBuildShopPanel == true)
        {
            _enhancementBtn.onClick.RemoveAllListeners();
            _repairBtn.onClick.RemoveAllListeners();
            _sellBtn.onClick.RemoveAllListeners();
        }

        _enhancementBtn.gameObject.SetActive(enhancement!=null);

        _enhancementBtn.onClick.AddListener(() => { enhancement?.Invoke(); });
        _repairBtn.onClick.AddListener(() => { repair?.Invoke(); });
        _sellBtn.onClick.AddListener(() => { sell?.Invoke(); });

        _isShowBuildShopPanel = true;
        _buildObjShopPanel.gameObject.SetActive(true);
        _buildObjShopPanel.localScale = Vector3.zero;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        _buildObjShopPanel.position = screenPos;
        _buildObjShopPanel.DOKill();
        Sequence seq = DOTween.Sequence();
        seq.Append(_buildObjShopPanel.DOMoveY(screenPos.y + 80, 0.3f))
            .Join(_buildObjShopPanel.DOScale(Vector3.one, 0.3f));
    }
    public void CloseBuildObjShopPanel()
    {
        if (_isShowBuildShopPanel == false)
            return;

        _enhancementBtn.onClick.RemoveAllListeners();
        _repairBtn.onClick.RemoveAllListeners();
        _sellBtn.onClick.RemoveAllListeners();

        _isShowBuildShopPanel = false;
        _buildObjShopPanel.DOKill();
        CloseBuildInfoPanel();
        Sequence seq = DOTween.Sequence();
        seq.Append(_buildObjShopPanel.DOScale(Vector3.zero, 0.3f))
            .OnComplete(() =>
            {
                _buildObjShopPanel.gameObject.SetActive(false);
            });
    }
    public void SetMoneyText(int money, bool shake = false)
    {
        _moneyText.text = money.ToString();
        if(shake == true)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_moneyText.rectTransform
                .DOShakeAnchorPos(1, 20, 100, 90));
        }
        
    }
}
