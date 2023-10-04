using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WaveType
{
    public Spawner[]    spawners;
    public float        spawnTime; // 언제 시작할지
    private bool        _isStart;  // 진행한 웨이브인지 예외 처리
}


public class WaveTimeline : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Sprite _waveImage;
    [SerializeField] private Slider _waveSlider;

    [Header("플레이 타임")]
    [SerializeField] private float _maxTimer;
    
    [Header("Wave정보")]    
    [SerializeField] WaveType[] _waveTypes;

    private float _curWaveTime = 0f;

    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        _curWaveTime += Time.deltaTime;       
    }
}