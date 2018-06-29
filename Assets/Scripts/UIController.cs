using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

    private int score;

    [SerializeField]
    private Text scoreKill;

    [SerializeField]
    private SettingsMenu settingMenu;

    [SerializeField]
    private UIUpravlenieMenu uprMenu;

	void Start () {
        score = 0;
        settingMenu.Close();
        uprMenu.Close();
    }

    public void OnOpenSetting()
    {
        settingMenu.Open();
    }

    public void OnCloseSetting()
    {
        settingMenu.Close();
    }

    private void OnEnemyHit()
    {
        score += 1;
        scoreKill.text = score.ToString();
    }

    public void OnUpravlenOpen()
    {
        settingMenu.Close();
        uprMenu.Open();
    }

    public void OnUpravlenClose()
    {
        settingMenu.Open();
        uprMenu.Close();
    }

    public void OnExitApplication()
    {
        Application.Quit();
    }
}
