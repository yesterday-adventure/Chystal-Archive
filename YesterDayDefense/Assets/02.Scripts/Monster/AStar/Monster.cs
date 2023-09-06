using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Monster : MonoBehaviour
{
    [Header("정보")]
    [SerializeField] private int _hp;
    [SerializeField] private int _attack;
    [SerializeField] private int _speed;

    [Header("위치")]
    [SerializeField] protected int _xIdx;
    [SerializeField] protected int _yIdx;

    [Header("디버프")]

    private XY _nexpos;
    private int _nextPosX;
    private int _nextPosY;

    XY xy;
    Vector3 nextMapPostion;
    Vector3 dir;

    private void OnEnable()
    {
        InitPosition();
        FindLoad();
    }

    private void InitPosition()
    {
        xy = MapManager.Instance.GetPostion(_xIdx, _yIdx);
        transform.position = new Vector3(xy.x,2,xy.y);
    }

    private void FindLoad()
    {
        _nexpos = LoadWeight.Instance.FindNextPos(_xIdx, _yIdx);
        _nextPosX = _nexpos.x;
        _nextPosY = _nexpos.y;
        Move();
    }
    protected virtual void Move()
    {
        StartCoroutine(IMove());
    }

    protected abstract void Attack();

    public void SetPosXY(int x, int y)
    {
        _xIdx = x;
        _yIdx = y;
    }

    
    IEnumerator IMove()
    {
        xy = MapManager.Instance.GetPostion(_nextPosX, _nextPosY);
        nextMapPostion = new Vector3(xy.x, transform.position.y, xy.y);
        dir = nextMapPostion - transform.position;

        while (Mathf.Abs(transform.position.x - nextMapPostion.x) > 0.1f ||
            Mathf.Abs(transform.position.z - nextMapPostion.z) > 0.1f)
        {

            transform.position += dir * _speed * Time.deltaTime;
            yield return null;
        }

        yield return null;
        transform.position = nextMapPostion;

        SetPosXY(_nextPosX, _nextPosY);
        FindLoad();
    }
}