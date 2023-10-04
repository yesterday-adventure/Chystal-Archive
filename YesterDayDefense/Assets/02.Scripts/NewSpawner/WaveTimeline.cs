using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WaveType
{
    public Spawner[] spawners;
}


public class WaveTimeline : MonoBehaviour
{
    [SerializeField] private Image _waveImage;
    [SerializeField] private Slider _waveSlider;
    [SerializeField] private float _maxTimer;
    [SerializeField] WaveType[] _waveTypes;
    

    private float _waveTime = 0f;


    private void Update()
    {
        _waveTime += Time.deltaTime;


    }
}