using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class BuildObjInfo
{
    public int EnhancementPrice; //��ȭ ����
    public int Health; //ü��
    //public 
}

public abstract class buildAbleMono : MonoBehaviour
{
    [Header("�ǹ� �Ӽ���")]
    [SerializeField]
    protected int _price;
    [SerializeField]
    protected int _enhancementValue; // ���� ��ȭ��

    private int _spentToBuildPrice = 0; // ���µ� ����� ��
    // �Ǹ��� ���� �������� �ǹ��� �� ���� 50%

    [Header("�ǹ� ��ȭ ������Ʈ ����")]
    [SerializeField]
    protected List<BuildObjInfo> _objects;

    private readonly Vector3 _infoUIOffset = new Vector3(0, 1, 0);

    private void OnMouseEnter()
    {
        UIManager.Instance.ShowBuildInfoPanel(transform.position + _infoUIOffset, 100, 100, 100);
    }
    private void OnMouseExit()
    {
        UIManager.Instance.ShowOffBuildInfoPanel();
    }
}
