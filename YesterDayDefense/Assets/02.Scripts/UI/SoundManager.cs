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
        Debug.Log(PlayerPrefs.GetFloat("master"));
        _masterSlider.value = PlayerPrefs.GetFloat("master");
        _effectSlider.value = PlayerPrefs.GetFloat("effect");
        _bgmSlider.value = PlayerPrefs.GetFloat("bgm");
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

    private void Update()
    {
        PlayerPrefs.SetFloat("master", _masterSlider.value);
        PlayerPrefs.SetFloat("effect", _effectSlider.value);
        PlayerPrefs.SetFloat("bgm", _bgmSlider.value);
    }

    private IEnumerator PlayerButton()
    {
        _audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        _audioSource.Stop();
    }
}
