using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private Slider _waveSlider;
    [SerializeField]
    private float _waveTime = 0f;
    [SerializeField]
    private float _changeWaveTime = 20f;
    [SerializeField]
    private float _spawnTime = 1f;
    [SerializeField]
    private int _monsterSpawnLevel = 1;

    [SerializeField]
    private GameObject[] _monster;

    [SerializeField]
    private float spawnPositionX; 
    [SerializeField]
    private float spawnPositionY; 

    private bool _spawningFirstWave = true;
    private bool _spawningSecondWave = false;
    private bool _spawningFinalWave = false;

    private void Awake()
    {
        _waveTime = 0f;
        StartCoroutine(SpawnWave(_monster[_monsterSpawnLevel]));
    }

    private void Update()
    {
        _waveTime += Time.deltaTime;

        if (_waveTime >= _changeWaveTime && _spawningFirstWave)
        {
            _spawningFirstWave = false;
            _spawningSecondWave = true;
            _monsterSpawnLevel = Random.Range(0, _monster.Length);
            StartCoroutine(SpawnWave(_monster[_monsterSpawnLevel]));
        }
        else if (_waveTime >= (_changeWaveTime * 2) && _spawningSecondWave)
        {
            _spawningSecondWave = false;
            _spawningFinalWave = true;
            _monsterSpawnLevel = Random.Range(0, _monster.Length);
            StartCoroutine(SpawnWave(_monster[_monsterSpawnLevel]));
        }
        _waveSlider.value = _waveTime;
    }

    IEnumerator SpawnWave(GameObject monster)
    {
        while (_spawningFirstWave || _spawningSecondWave || _spawningFinalWave)
        {
            Debug.Log("SpawnWave: " + monster.name);
            Vector3 spawnPosition = new Vector3(spawnPositionX, spawnPositionY, 0f); 
            Instantiate(monster, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(_spawnTime);
        }
    }
}
