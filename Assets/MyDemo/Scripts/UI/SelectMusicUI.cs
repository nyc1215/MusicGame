using System;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;

public class SelectMusicUI : MonoBehaviour
{
    GComponent selectUI_root;
    UIPanel selectUIPanel;

    GButton backButton;
    GButton upButton;
    GButton downButton;
    GTextField musicInfo;
    int musicIndexOnShow;

    GButton startButton;
    GTextField startText;

    AudioSource musicClipNow;

    private void Awake()
    {
        musicClipNow = GetComponent<AudioSource>();
        selectUIPanel = GetComponent<UIPanel>();
    }

    private void Start()
    {
        musicClipNow.volume = MyGameSettings.GetMyGameSettingsInstance().Volume / 100.0f;

        musicIndexOnShow = 0;

        selectUI_root = selectUIPanel.ui;

        backButton = selectUI_root.GetChild("backButton").asButton;
        backButton.onClick.Add(delegate () { SceneManager.LoadScene("StartScene"); });

        upButton = selectUI_root.GetChild("up").asButton;
        downButton = selectUI_root.GetChild("down").asButton;

        musicInfo = selectUI_root.GetChild("musicInfo").asTextField;
        UpdateMusicInfo();

        upButton.onClick.Add(MusicUp);
        downButton.onClick.Add(MusicDown);

        startText = selectUI_root.GetChild("startText").asTextField;

        startButton = selectUI_root.GetChild("startButton").asButton;
        startButton.onRollOver.Add(delegate () { startText.visible = true; });
        startButton.onRollOut.Add(delegate () { startText.visible = false; });
        startButton.onClick.Add(delegate ()
            {
                if (musicClipNow.isPlaying)
                {
                    musicClipNow.Stop();
                }
                MyGameManager.GetGameManagerInstance().playerLife = 10;
                MyGameManager.GetGameManagerInstance().playerShield = 0;
                MyGameManager.GetGameManagerInstance().isWin = false;
                SceneManager.LoadScene("Game");
            });
    }

    private void MusicUp()
    {
        musicIndexOnShow++;
        if (musicIndexOnShow >= MusicResource.GetMusicResourceInstance().musics.Length)
        {
            musicIndexOnShow = 0;
        }
        MyGameManager.GetGameManagerInstance().SelectedMusicIndex = musicIndexOnShow;
        UpdateMusicInfo();
    }

    private void MusicDown()
    {
        musicIndexOnShow--;
        if (musicIndexOnShow < 0)
        {
            musicIndexOnShow = MusicResource.GetMusicResourceInstance().musics.Length - 1;
        }
        MyGameManager.GetGameManagerInstance().SelectedMusicIndex = musicIndexOnShow;
        UpdateMusicInfo();
    }

    private void UpdateMusicInfo()
    {
        if (MusicResource.GetMusicResourceInstance().isAllLoad)
        {
            if (musicClipNow.isPlaying)
            {
                musicClipNow.Stop();
            }
            musicClipNow.clip = MusicResource.GetMusicResourceInstance().musics[musicIndexOnShow];
            musicClipNow.Play();
        }
        musicInfo.SetVar(
           "music", MusicResource.GetMusicResourceInstance().musics[musicIndexOnShow].name
           )
           .SetVar(
           "min", ((int)(MusicResource.GetMusicResourceInstance().musics[musicIndexOnShow].length / 60.0f)).ToString().PadLeft(2, '0')
           )
           .SetVar(
           "sec", ((int)(MusicResource.GetMusicResourceInstance().musics[musicIndexOnShow].length % 60.0f)).ToString().PadLeft(2, '0')
           )
           .FlushVars();
    }
}
