using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawner
{
    public Monster[]    monsters;   // ��ȯ�� ���͵�
    public int[]        cnts;       // ��ȯ�� ���� ��
    [Range(0,31)] // x��ǥ ���� <- MapManger����
    public int          _xidx;      // x��ǥ
    [Range(0,31)] // y��ǥ ���� <- MapManger����
    public int          _yidx;      // y��ǥ
    [Range(0f, 2f)]
    public float        _spawnDelay; // ��ȯ ������
}
