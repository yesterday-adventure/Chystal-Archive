using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpawnTime
{
    public float changeSpawnTime;
}

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private Image[] _waveImage; 
    [SerializeField]
    private Slider _waveSlider;
    [SerializeField]
    private float _waveTime = 0f;
    [SerializeField]
    private List<SpawnTime> _changeSpawnTime;

    [Header("몬스터 소환 웨이브 데이터")]
    [SerializeField]
    private List<Wave> _waveData;

    [SerializeField]
    private int _waveCount = 0;

    private bool _spawningFirstWave = true;
    private bool _spawningSecondWave = false;
    private bool _spawningFinalWave = false;

    private void Awake()
    {
        _waveTime = 0f;
        StartSpawn();
    }

    private void Update()
    {
        _waveTime += Time.deltaTime;

        if (_waveTime >= _changeSpawnTime[0].changeSpawnTime && _spawningFirstWave)
        {
            _waveCount++;
            _spawningFirstWave = false;
            _spawningSecondWave = true;
            StartSpawn();
        }
        else if (_waveTime >= _changeSpawnTime[1].changeSpawnTime && _spawningSecondWave)
        {
            _waveCount++;
            _spawningSecondWave = false;
            _spawningFinalWave = true;
            StartSpawn();
        }
        _waveSlider.value = _waveTime;
    }

    void StartSpawn()
    {
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(SpawnMonster(_waveData[_waveCount].SpawnDatas[i].MobPrefab, i));
        }
    }

    IEnumerator SpawnMonster(Monster monster, int idx)
    {
        yield return new WaitForSeconds(_waveData[_waveCount].SpawnDatas[idx].SpawnDelay);
        while (_spawningFirstWave || _spawningSecondWave || _spawningFinalWave)
        {
            Monster mob = PoolManager.Instance.Pop(monster.name) as Monster;
            mob.xIdx = _waveData[_waveCount].SpawnDatas[idx].x;
            mob.yIdx = _waveData[_waveCount].SpawnDatas[idx].y;
            mob.Reset();
            
            yield return new WaitForSeconds(_waveData[_waveCount].SpawnDatas[idx].SpawnTerm);
        }
    }
}


