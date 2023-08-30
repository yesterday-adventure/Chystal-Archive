using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSpawnData
{
    public GameObject MobPrefab;
    public Vector3 SpawnPos;
    public int SpawnCount;
    public float SpawnDelay;
    public float SpawnTerm;
}

[System.Serializable]
public class Wave
{
    public List<MonsterSpawnData> SpawnDatas;
}
