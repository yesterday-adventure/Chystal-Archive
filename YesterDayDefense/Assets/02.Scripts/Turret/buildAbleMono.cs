using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class BuildObjInfo
{
    public int EnhancementPrice; //강화 가격
    public int Health; //체력
    //public 
}

public abstract class buildAbleMono : MonoBehaviour
{
    [Header("건물 속성값")]
    [SerializeField]
    protected int _price;
    [SerializeField]
    protected int _enhancementValue; // 현재 강화값

    private int _spentToBuildPrice = 0; // 짓는데 사용한 돈
    // 판매할 때는 여때까지 건물에 쓴 돈의 50%

    [Header("건물 강화 오브젝트 정보")]
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
