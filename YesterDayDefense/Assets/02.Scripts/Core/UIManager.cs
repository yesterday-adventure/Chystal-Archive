using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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

    private bool _isShowBuildInfoPanel = false;
    private bool _isShowBuildShopPanel = false;


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
    }

    public void ShowBuildInfoPanel(Vector3 pos, string enhancementPrice, string repairPrice, string sellPrice)
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
    public void ShowOffBuildInfoPanel()
    {
        if (_isShowBuildInfoPanel == false || _isShowBuildShopPanel == true)
            return;

        _isShowBuildInfoPanel = false;

        _buildInfoPanel.gameObject.SetActive(false);
        _buildInfoPanel.DOScaleX(0, 0.2f);
    }
    public void OpenBuildObjShopPanel(Vector3 pos, Action enhancement, Action repair, Action sell)
    {
        if(_isShowBuildShopPanel == true)
        {
            _enhancementBtn.onClick.RemoveAllListeners();
            _repairBtn.onClick.RemoveAllListeners();
            _sellBtn.onClick.RemoveAllListeners();
        }

        _isShowBuildShopPanel = true;
        _buildObjShopPanel.gameObject.SetActive(true);
        _buildObjShopPanel.localScale = Vector3.zero;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        _buildObjShopPanel.position = screenPos;
        Sequence seq = DOTween.Sequence();
        seq.Append(_buildObjShopPanel.DOMoveY(screenPos.y + 80, 0.3f))
            .Join(_buildObjShopPanel.DOScale(Vector3.one, 0.3f));
    }
}
