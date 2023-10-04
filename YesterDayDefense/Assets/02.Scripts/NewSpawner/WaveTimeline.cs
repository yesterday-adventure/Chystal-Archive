using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WaveType
{
    public Spawner[]    spawners;
    public float        spawnTime; // ���� ��������
    private bool        _isStart;  // ������ ���̺����� ���� ó��
}


public class WaveTimeline : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Sprite _waveImage;
    [SerializeField] private Slider _waveSlider;

    [Header("�÷��� Ÿ��")]
    [SerializeField] private float _maxTimer;
    
    [Header("Wave����")]    
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