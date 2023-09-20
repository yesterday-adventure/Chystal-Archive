using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSpawnData
{
    public Monster MobPrefab;
    public int x;
    public int y;
    public int SpawnCount;
    public float SpawnDelay;
    public float SpawnTerm;
}

[System.Serializable]
public class Wave
{
    public List<MonsterSpawnData> SpawnDatas;
}
