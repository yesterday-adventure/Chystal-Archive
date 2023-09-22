using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] LayerMask _buildObjLayer;
    public static bool IsMoveScreen = false;
    // 현재 화면을 움직이고 있는가
    Vector3 _dir;
    Vector3 _lastMousePos;
    Vector3 _mousePos;
    BuildAbleMono _currentBuildObj = null;

    private void LateUpdate()
    {
        if(Input.GetMouseButtonDown(1))
        {
            UIManager.Instance.CloseBuildObjShopPanel();
            IsMoveScreen = true;
            _lastMousePos = Input.mousePosition;
            _mousePos = Input.mousePosition;
        }
        if(Input.GetMouseButton(1))
        {
            _lastMousePos = Input.mousePosition;
            _dir = _mousePos - _lastMousePos;
            _dir.z = _dir.y;
            _dir.y = 0;
            transform.position += _dir * _moveSpeed * Time.deltaTime;
            _mousePos = _lastMousePos;
            transform.localPosition = new Vector3(
                Mathf.Clamp(transform.localPosition.x, -14, 13),
                transform.position.y, 
                Mathf.Clamp(transform.localPosition.z, -17, 21));
        }
        if(Input.GetMouseButtonUp(1))
        {
            IsMoveScreen = false;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (UIManager.Instance.GetBuildShopPanelShowed == true)
                CloseBuildObjShop();

            if (UIManager.Instance.GetBuildInfoPanelShowed == true)
                ShowBuildObjShop();
        }
        CheckBuildObj();
    }

    private void CheckBuildObj()
    {
        _mousePos = Input.mousePosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(_mousePos);
        Physics.Raycast(ray, out hit, 100000, _buildObjLayer);
        if ((hit.collider != null) == UIManager.Instance.GetBuildInfoPanelShowed)
            return;

        if(hit.collider)
        {
            if (hit.collider.TryGetComponent<BuildAbleMono>(out _currentBuildObj))
            {
                ShowBuildObjInfo();
            }
        }
        else
        {
            CloseBuildObjInfo();
        }
    }
    private void ShowBuildObjInfo()
    {
        _currentBuildObj.ShowInfo(true);
    }
    private void ShowBuildObjShop()
    {
        _currentBuildObj.ShowShop(true);
    }
    private void CloseBuildObjInfo()
    {
        _currentBuildObj.ShowInfo(false);
    }
    private void CloseBuildObjShop()
    {
        _currentBuildObj.ShowShop(false);
    }
}