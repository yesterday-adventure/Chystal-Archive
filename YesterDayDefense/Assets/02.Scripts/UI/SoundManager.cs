using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer _audioMixer;
    public AudioSource _audioSource;
    public Slider _bgmSlider;
    public Slider _effectSlider;
    public Slider _masterSlider;

    public void BGMAudioControl()
    {
        float sound = _bgmSlider.value;


        _audioMixer.SetFloat("BGM", sound);
    }

    public void EffectAudioControl()
    {
        float sound = _effectSlider.value;


        _audioMixer.SetFloat("Effect", sound);
    }
    // Start is called before the first frame update
    void Start()
    {

        _audioMixer.SetFloat("BGM", _bgmSlider.value);
        _audioMixer.SetFloat("Effect", _effectSlider.value);
    }

    public void ClickButtonSound()
    {
        StartCoroutine(PlayerButton());
    }

    public void MasterAudioControl()
    {
        float sound = _masterSlider.value;
        
        _audioMixer.SetFloat("Master", sound);
    }

    private IEnumerator PlayerButton()
    {
        _audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        _audioSource.Stop();
    }
}
