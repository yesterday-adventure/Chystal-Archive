using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingPanel;
    [SerializeField]
    private GameObject _stageSelectPanel;
    public bool Setting = false;
    public bool Stage = false;
    private void Awake()
    {
        _stageSelectPanel.SetActive(false);
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
        Setting = true;
    }
    public void SettingPanelExit()
    {
        _settingPanel.SetActive(false);
        Setting = false;
    }

    public void StageSelectPanel()
    {

        _stageSelectPanel.SetActive(true);
        Stage = true;
    }
    public void StageSelectExit()
    {
        _stageSelectPanel.SetActive(false);
        Stage = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Setting)
        {
            _settingPanel.SetActive(true);
            Setting = true;
        }else if(Setting && Input.GetKeyDown(KeyCode.Escape))
        {
            _settingPanel.SetActive(false);
            Setting = false;
        }
    }
}
