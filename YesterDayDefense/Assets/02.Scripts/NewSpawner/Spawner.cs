using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawner
{
    public Monster[]    monsters;   // 소환할 몬스터들
    public int[]        cnts;       // 소환할 몬스터 수
    [Range(0,31)] // x좌표 범위 <- MapManger참고
    public int          xidx;      // x좌표
    [Range(0,31)] // y좌표 범위 <- MapManger참고
    public int          yidx;      // y좌표
    [Range(0f, 2f)]
    public float        spawnDelay; // 소환 딜레이
}
