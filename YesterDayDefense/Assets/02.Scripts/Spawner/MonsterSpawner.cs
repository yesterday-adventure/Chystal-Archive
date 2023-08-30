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
        StartCoroutine(SpawnMonster1(_waveData[_waveCount].SpawnDatas[0].MobPrefab));
        StartCoroutine(SpawnMonster2(_waveData[_waveCount].SpawnDatas[1].MobPrefab));
        StartCoroutine(SpawnMonster3(_waveData[_waveCount].SpawnDatas[2].MobPrefab));
    }

    private void Update()
    {
        _waveTime += Time.deltaTime;

        if (_waveTime >= _changeSpawnTime[0].changeSpawnTime && _spawningFirstWave)
        {
            _waveCount++;
            _spawningFirstWave = false;
            _spawningSecondWave = true;
            StartCoroutine(SpawnMonster1(_waveData[_waveCount].SpawnDatas[0].MobPrefab));
            StartCoroutine(SpawnMonster2(_waveData[_waveCount].SpawnDatas[1].MobPrefab));
            StartCoroutine(SpawnMonster3(_waveData[_waveCount].SpawnDatas[2].MobPrefab));
        }
        else if (_waveTime >= _changeSpawnTime[1].changeSpawnTime && _spawningSecondWave)
        {
            _waveCount++;
            _spawningSecondWave = false;
            _spawningFinalWave = true;
            StartCoroutine(SpawnMonster1(_waveData[_waveCount].SpawnDatas[0].MobPrefab));
            StartCoroutine(SpawnMonster2(_waveData[_waveCount].SpawnDatas[1].MobPrefab));
            StartCoroutine(SpawnMonster3(_waveData[_waveCount].SpawnDatas[2].MobPrefab));
        }
        _waveSlider.value = _waveTime;
    }

    IEnumerator SpawnMonster1(GameObject monster)
    {
        yield return new WaitForSeconds(_waveData[_waveCount].SpawnDatas[0].SpawnDelay);
        while (_spawningFirstWave || _spawningSecondWave || _spawningFinalWave)
        {
            Debug.Log("SpawnWave: " + monster.name);
            Vector3 spawnPosition = _waveData[_waveCount].SpawnDatas[0].SpawnPos;
            Instantiate(monster, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(_waveData[_waveCount].SpawnDatas[0].SpawnTerm);
        }
    }
    IEnumerator SpawnMonster2(GameObject monster)
    {
        yield return new WaitForSeconds(_waveData[_waveCount].SpawnDatas[1].SpawnDelay);
        while (_spawningFirstWave || _spawningSecondWave || _spawningFinalWave)
        {
            Debug.Log("SpawnWave: " + monster.name);
            Vector3 spawnPosition = _waveData[_waveCount].SpawnDatas[1].SpawnPos;
            Instantiate(monster, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(_waveData[_waveCount].SpawnDatas[1].SpawnTerm);
        }
    }
    IEnumerator SpawnMonster3(GameObject monster)
    {
        yield return new WaitForSeconds(_waveData[_waveCount].SpawnDatas[2].SpawnDelay);
        while (_spawningFirstWave || _spawningSecondWave || _spawningFinalWave)
        {
            Debug.Log("SpawnWave: " + monster.name);
            Vector3 spawnPosition = _waveData[_waveCount].SpawnDatas[2].SpawnPos;
            Instantiate(monster, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(_waveData[_waveCount].SpawnDatas[2].SpawnTerm);
        }
    }
}


