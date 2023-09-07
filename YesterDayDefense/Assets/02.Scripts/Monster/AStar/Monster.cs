using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : PoolableMono
{
    [Header("정보")]
    [SerializeField] private int _hp;
    [SerializeField] private int _attack;
    [SerializeField] private int _speed;
    private int _curhp;

    [Header("위치")]
    [SerializeField] protected int _xIdx;
    [SerializeField] protected int _yIdx;

    [Header("디버프")]
    private bool isSlow;
    public bool IsSlow => isSlow;

    [Header("애니메이터")]
    private Animator _animator;
    protected readonly int _moveHash = Animator.StringToHash("CanMove");
    protected readonly int _dieHash = Animator.StringToHash("IsDie");
    protected readonly int _overHash = Animator.StringToHash("IsOver");
    protected readonly int _attackHash = Animator.StringToHash("IsAttack");

    private XY _nexpos;
    private int _nextPosX;
    private int _nextPosY;

    XY xy;
    Vector3 nextMapPostion;
    Vector3 dir;

    private void Awake()
    {
        transform.SetParent(GameManager.Instance.monsterParentTrm);
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Reset();
    }

    public void SetPosXY(int x, int y)
    {
        _xIdx = x;
        _yIdx = y;
    }

    protected virtual void Move()
    {
        StartCoroutine(IMove());
    }

    public override void Reset()
    {
        InitPosition();
        FindLoad();
        InitVariable();
    }

    public void OnDamage(int damage)
    {
        _curhp -= damage;
        if(_curhp <= 0 )
        {
            _animator.SetBool(_dieHash, true);
            StartCoroutine(IDie());
        }
    }

    public void GameOver()
    {
        _animator.SetBool(_overHash, true);
    }

    protected abstract void Attack();

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
    
    private void InitVariable()
    {
        _curhp = _hp;
        _animator.SetBool(_moveHash, true);
        _animator.SetBool(_dieHash, false);
        _animator.SetBool(_overHash, false);
        _animator.SetBool(_attackHash, false);
    }

    private bool CheckTower(int x, int y)
    {
        return LoadWeight.Instance.isSetup[x, y];
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

    IEnumerator IDie()
    {
        yield return new WaitForSeconds(2f);

        PoolManager.Instance.Push(this);
    }
}