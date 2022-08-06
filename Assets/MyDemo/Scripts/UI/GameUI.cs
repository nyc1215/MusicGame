using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    UIPanel gameUIPanel;
    GComponent gameUI_root;
    GComponent pauseUI_root;
    GComponent endUI_root;

    GButton pauseButton;
    GButton continueButton;
    GButton restartButton;
    GButton quitButton;

    GTextField startCountdownText;
    GTextField lifeText;
    GTextField shieldText;
    GTextField musicProcess;
    GTextField endText;

    int totalcount;

    private void Awake()
    {
        gameUIPanel = GetComponent<UIPanel>();
        gameUI_root = gameUIPanel.ui;

        pauseUI_root = UIPackage.CreateObject("GamePackage", "PauseComponent").asCom;

        startCountdownText = UIPackage.CreateObject("GamePackage", "CountComponent").asCom
            .GetChild("startCountdownText").asTextField;

        endUI_root = UIPackage.CreateObject("GamePackage", "EndComponent").asCom;
    }

    void Start()
    {
        totalcount = MyGameManager.GetGameManagerInstance().totalcount;


        endText = endUI_root.GetChild("endtext").asTextField;
        endUI_root.GetChild("restartButton").asButton.onClick.Add(Restart);
        endUI_root.GetChild("quitButton").asButton.onClick.Add(Quit);

        pauseButton = gameUI_root.GetChild("pauseButton").asButton;
        pauseButton.onClick.Add(GamePause);

        lifeText = gameUI_root.GetChild("lifeText").asTextField;
        LifeTextUpdate();

        shieldText = gameUI_root.GetChild("shieldText").asTextField;
        ShieldUpdate();

        musicProcess = gameUI_root.GetChild("musicProcess").asTextField;

        continueButton = pauseUI_root.GetChild("ContinueButton").asButton;
        continueButton.onClick.Add(GameContinue);

        restartButton = pauseUI_root.GetChild("restartButton").asButton;
        restartButton.onClick.Add(Restart);

        quitButton = pauseUI_root.GetChild("quitButton").asButton;
        quitButton.onClick.Add(Quit);

        float[] timeinfo = new float[2]
        {
            0.0f,
            MyGameManager.GetGameManagerInstance().musicResource.musics[MyGameManager.GetGameManagerInstance().SelectedMusicIndex].length
        };
        ProgressUpdate(timeinfo);
    }

    public void GamePause()
    {
        MyGameManager.GetGameManagerInstance().PauseGame();
        pauseUI_root.SetPosition(gameUI_root.width / 2.0f, gameUI_root.height / 2.0f, 0);
        gameUI_root.AddChild(pauseUI_root);
        pauseUI_root.visible = true;
    }

    private void GameContinue()
    {
        MyGameManager.GetGameManagerInstance().ContinueGame();
        pauseUI_root.visible = false;
    }

    public void GameEnd(bool isWin)
    {
        MyGameManager.GetGameManagerInstance().PauseGame();
        endUI_root.SetPosition(gameUI_root.width / 2.0f, gameUI_root.height / 2.0f, 0);
        if (isWin)
        {
            endText.SetVar("endtext", "你赢了").FlushVars();
        }
        else
        {
            endText.SetVar("endtext", "你输了").FlushVars();
        }
        gameUI_root.AddChild(endUI_root);
        pauseUI_root.visible = false;
        endUI_root.visible = true;
    }

    private void Quit()
    {
        MyGameManager.GetGameManagerInstance().QuitGame();
        pauseUI_root.visible = false;
        endUI_root.visible = false;
    }

    private void Restart()
    {
        MyGameManager.GetGameManagerInstance().InitialGame();
        pauseUI_root.visible = false;
        endUI_root.visible = false;
    }

    public void LifeTextUpdate()
    {
        lifeText.SetVar("life", MyGameManager.GetGameManagerInstance().playerLife.ToString()).FlushVars();
    }

    public void ShieldUpdate()
    {
        if (MyGameManager.GetGameManagerInstance().playerShield > 0)
        {
            shieldText.SetVar(
                "shield", "护盾持续时间:" + MyGameManager.GetGameManagerInstance().playerShield.ToString() + "秒"
                ).FlushVars();
        }
        else
        {
            shieldText.SetVar("shield", "无").FlushVars();
        }
    }

    public void ProgressUpdate(float[] time)
    {
        float nowtime = time[0];
        float totaltime = time[1];
        musicProcess.SetVar(
            "now", ((int)(nowtime / 60.0f)).ToString().PadLeft(2, '0') + ":" + ((int)(nowtime % 60.0f)).ToString().PadLeft(2, '0')
            ).SetVar(
            "total", ((int)(totaltime / 60.0f)).ToString().PadLeft(2, '0') + ":" + ((int)(totaltime % 60.0f)).ToString().PadLeft(2, '0')
            ).SetVar(
            "process", ((int)(nowtime / totaltime * 100.0f)).ToString()
            ).FlushVars();
    }

    public IEnumerator IE_StartCountDown()
    {
        Debug.Log("IE_StartCountDown");
        MyGameManager.GetGameManagerInstance().isPause = true;
        while (totalcount > 0)
        {
            yield return new WaitForSeconds(1);
            totalcount--;
            startCountdownText.SetVar("Countdown", totalcount.ToString()).FlushVars();
        }
        startCountdownText.visible = false;
        MyGameManager.GetGameManagerInstance().isPause = false;
    }

    public void StartCountDown()
    {
        totalcount = MyGameManager.GetGameManagerInstance().totalcount;
        gameUI_root.AddChild(startCountdownText);
        startCountdownText.SetVar("Countdown", totalcount.ToString()).FlushVars();
        startCountdownText.SetPosition(gameUI_root.width / 2.0f, gameUI_root.height / 2.0f, 0);
        startCountdownText.visible = true;
        StartCoroutine(IE_StartCountDown());
    }

    private void Update()
    {
        ShieldUpdate();
    }
}
