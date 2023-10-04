using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WaveType
{
    public Spawner[] spawners;
    public float spawnTime; // 언제 시작할지
    public bool _isStart = false;  // 진행한 웨이브인지 예외 처리
}


public class WaveTimeline : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _waveImage;
    [SerializeField] private Slider _waveSlider;

    [Header("플레이 타임")]
    [SerializeField] private float _maxTimer = 120;

    [Header("Wave정보")]
    [SerializeField] WaveType[] _waveTypes;

    public float _curWaveTime = 0f;

    private void Start()
    {
        for(int i = 0; i <  _waveTypes.Length; i++)
        {
            if (_waveTypes[i].spawnTime != _maxTimer)
            {
                RectTransform rt = PoolManager.Instance.Pop(_waveImage.name).GetComponent<RectTransform>();
                rt.parent = _waveSlider.transform;
                //-240 ~ 240
                rt.anchoredPosition = new Vector2(-240 + (480 /(_waveTypes[i].spawnTime / _maxTimer)),17.1f);
            }
        }
    }

    private void Update()
    {
        _curWaveTime += Time.deltaTime;
        _waveSlider.value = _curWaveTime / _maxTimer;
    }

    private void FixedUpdate()
    {
        foreach (WaveType waveType in _waveTypes)
        {
            if (!waveType._isStart && waveType.spawnTime <= _curWaveTime)
            {
                StartSpawn(waveType.spawners);
                waveType._isStart = true;
            }
        }
    }

    private void StartSpawn(Spawner[] spawners)
    {
        foreach (Spawner spawner in spawners)
        {
            StartWave(spawner);
        }
    }

    IEnumerator StartWave(Spawner spawner)
    {
        WaitForSeconds wfs = new WaitForSeconds(spawner.spawnDelay);
        for (int i = 0; i < spawner.monsters.Length; i++)
        {
            for(int j = 0; j < spawner.cnts[i]; j++)
            {
                Monster mob = PoolManager.Instance.Pop(spawner.monsters[i].name) as Monster;
                mob.xIdx = spawner.xidx;
                mob.yIdx = spawner.yidx;
                mob.Reset();

            }
            yield return wfs;
        }
    }
}