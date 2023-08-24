using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private float _waveTime = 0f;
    [SerializeField]
    private float _spawnTime = 1f;
    [SerializeField]
    private float _spawnMosterCount = 0f;
    [SerializeField]
    private int _monsterSpawnLevel = 0;

    [SerializeField]
    private bool FirstSpawn = true;
    [SerializeField]
    private bool ScendSpawn = false;
    [SerializeField]
    private bool FinalSpawn = false;

    [SerializeField]
    private GameObject[] _monster;

    private void Awake()
    {
        _waveTime = 0f;
        _spawnMosterCount = 0f;
        if (FirstSpawn)
            StartCoroutine(FirstWaveMonster(_monster[_monsterSpawnLevel]));

    }

    private void Update()
    {
        _waveTime += Time.deltaTime;
        if(_waveTime >= 1f && _waveTime <=2f)
        {
            FirstSpawn = false;
            ScendSpawn = true;
            _spawnMosterCount = 1f;
        }
        else if(_waveTime >= 2f)
        {
            ScendSpawn = false;
            FinalSpawn = true;
            _spawnMosterCount = 2f;
        }
        if (_spawnMosterCount == 1f && ScendSpawn)
        {
            StopCoroutine(FirstWaveMonster(_monster[_monsterSpawnLevel]));
            _monsterSpawnLevel = Random.Range(0, 1);
            StartCoroutine(ScendWaveMonster(_monster[_monsterSpawnLevel]));
        }
        else if (_spawnMosterCount > 1f && FinalSpawn)
        {
            StopCoroutine(ScendWaveMonster(_monster[_monsterSpawnLevel]));
            _monsterSpawnLevel = Random.Range(0, 2);
            StartCoroutine(FinalWaveMonster(_monster[_monsterSpawnLevel]));
        }
    }

    IEnumerator FirstWaveMonster(GameObject monster)
    {
        Debug.Log("FirstWaveMonster");
        Instantiate(monster, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_spawnTime);
     
    }

    IEnumerator ScendWaveMonster(GameObject monster)
    {
        Debug.Log("ScendWaveMonster");
        Instantiate(monster, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_spawnTime);

    }

    IEnumerator FinalWaveMonster(GameObject monster)
    {
        Debug.Log("FinalWaveMonster");
        Instantiate(monster, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_spawnTime);
    }
}
