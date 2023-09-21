using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 2f;
    public static bool IsMoveScreen = false;
    // 현재 화면을 움직이고 있는가
    Vector3 _dir;
    Vector3 _lastMousePos;
    Vector3 _mousePos;
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
    }
}