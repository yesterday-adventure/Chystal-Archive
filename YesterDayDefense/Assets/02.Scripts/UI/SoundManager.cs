using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public AudioMixer _audioMixer;
    public Slider _slider;

    public void AudioControl()
    {
        float sound = _slider.value;

        //if (sound == 0) _audioMixer.SetFloat("BGM", -80);

        _audioMixer.SetFloat("BGM", sound);
    }
    // Start is called before the first frame update
    void Start()
    {

        _audioMixer.SetFloat("BGM", _slider.value);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
