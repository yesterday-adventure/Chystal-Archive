using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : PoolableMono
{
    [Header("사운드")]
    [SerializeField] private AudioSource _attackSound;
    [SerializeField] private AudioSource _damage;
    [SerializeField] private AudioSource _dieSound;

    [Header("정보")]
    [SerializeField] private int _hp;
    [SerializeField] private int _attack;
    [SerializeField] private float _speed;
    private int _curhp;

    [Header("위치")]
    public int xIdx;
    public int yIdx;

    [Header("디버프")]
    private bool isSlow;
    public bool IsSlow => isSlow;

    [Header("애니메이터")]
    private Animator _animator;
    public Animator Animator => _animator;
    protected readonly int _moveHash = Animator.StringToHash("CanMove");
    protected readonly int _dieHash = Animator.StringToHash("IsDie");
    protected readonly int _overHash = Animator.StringToHash("IsOver");
    public int OverHash => _overHash;
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

    public void SetPosXY(int x, int y)
    {
        xIdx = x;
        yIdx = y;
    }

    public override void Reset()
    {
        StopAllCoroutines();
        InitPosition();
        InitVariable();
        FindLoad();
    }

    public void OnDamage(int damage, bool ice)
    {
        // 데미지 사운드 재생

        _curhp -= damage;
        _damage.Play(); 
        if (_curhp <= 0)
        {
            _animator.SetBool(_dieHash, true);
            StopAllCoroutines() ;
            StartCoroutine(IDie());
            _dieSound.Play();
        }

        if (ice == true)
            isSlow = true;
    }

    public void GameOver()
    {
        _animator.SetBool(_overHash, true);
    }

    protected void Attack()
    {
        if (CheckTower(_nextPosX, _nextPosY))
        {
            //공격 사운드
            _attackSound.Play();
            LoadWeight.Instance.isSetup[_nextPosX, _nextPosY].Damage(_attack);
        }
        else
        {
            _animator.SetBool(_attackHash, false);
            StartCoroutine(IMove());
        }
    }

    private void InitPosition()
    {
        xy = MapManager.Instance.GetPostion(xIdx, yIdx);
        transform.position = new Vector3(xy.x, 2, xy.y);
    }

    private void FindLoad()
    {
        _nexpos = LoadWeight.Instance.FindNextPos(xIdx, yIdx);
        _nextPosX = _nexpos.x;
        _nextPosY = _nexpos.y;
        StartCoroutine(IMove());
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
        return LoadWeight.Instance.isSetup[x, y] != null;
    }

    IEnumerator IMove()
    {
        for (int i = 0; i < 1; i++)
        {
            xy = MapManager.Instance.GetPostion(_nextPosX, _nextPosY);
            nextMapPostion = new Vector3(xy.x, transform.position.y, xy.y);
            dir = nextMapPostion - transform.position;
            if (transform.position.x == nextMapPostion.x)
            {
                if (transform.position.z > nextMapPostion.z)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                else
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                if(transform.position.x > nextMapPostion.x)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                else
                    transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            if (CheckTower(_nextPosX, _nextPosY))
            {
                _animator.SetBool(_attackHash, true);
                break;
            }
            while (Mathf.Abs(transform.position.x - nextMapPostion.x) > 0.1f ||
                Mathf.Abs(transform.position.z - nextMapPostion.z) > 0.1f)
            {
                if(isSlow)
                    transform.position += dir * (_speed / 2) * Time.deltaTime;
                else
                    transform.position += dir * _speed * Time.deltaTime;

                yield return null;
            }

            yield return null;
            transform.position = nextMapPostion;

            SetPosXY(_nextPosX, _nextPosY);
            FindLoad();
        }
        yield return null;
    }

    IEnumerator IDie()
    {
        yield return new WaitForSeconds(0.3f);

        PoolManager.Instance.Push(this);
    }
}