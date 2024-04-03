using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject SettingPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenSetting()
    {
        SettingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        SettingPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
