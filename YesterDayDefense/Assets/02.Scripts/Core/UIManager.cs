using DG.Tweening;
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


    public UIManager(Transform gameUIPanel)
    {
        _buildInfoPanel = gameUIPanel.Find("BuildObjInfoPanel").transform;
        _enhancementPriceText = _buildInfoPanel.Find("enhancementPriceText").GetComponent<Text>();
        _repairPriceText = _buildInfoPanel.Find("repairPriceText").GetComponent<Text>();
        _sellPriceText = _buildInfoPanel.Find("sellPriceText").GetComponent<Text>();
    }

    public void ShowBuildInfoPanel(Vector3 pos, int enhancementPrice, int repairPrice, int sellPrice)
    {
        _enhancementPriceText.text = enhancementPrice.ToString();
        _repairPriceText.text       = repairPrice.ToString();
        _sellPriceText.text         = sellPrice.ToString();

        _buildInfoPanel.position = Camera.main.WorldToScreenPoint(pos);
        _buildInfoPanel.gameObject.SetActive(true);
        _buildInfoPanel.DOKill();
        _buildInfoPanel.localScale = new Vector3(0, 1, 1);
        _buildInfoPanel.DOScaleX(1, 0.2f);
    }
    public void ShowOffBuildInfoPanel()
    {
        _buildInfoPanel.gameObject.SetActive(false);
        _buildInfoPanel.DOScaleX(0, 0.2f);
    }
}
