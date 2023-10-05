using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    public AudioSource _audioSource;

    public void Intro(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Restart(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void NextStage(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ClickButtonSound()
    {
        StartCoroutine(PlayerButton());
    }

    private IEnumerator PlayerButton()
    {
        _audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        _audioSource.Stop();
    }

}
