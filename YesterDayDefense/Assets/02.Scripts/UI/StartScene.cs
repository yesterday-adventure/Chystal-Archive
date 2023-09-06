using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingPanel;
    private void Awake()
    {
        _settingPanel.SetActive(false);
    }
    public void GameStart(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void SettingPanel()
    {

        _settingPanel.SetActive(true);
    }
    public void SettingPanelExit()
    {
        _settingPanel.SetActive(false);
    }
}
