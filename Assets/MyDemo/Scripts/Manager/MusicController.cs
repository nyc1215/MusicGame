using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class MusicController : MonoBehaviour
{
    Koreographer Mykoreographer;
    AudioSource audioSource;
    SimpleMusicPlayer simplemusicPlayer;

    int musicIndex;
    float[] timeInfo = new float[2];

    private void Awake()
    {
        musicIndex = MyGameManager.GetGameManagerInstance().SelectedMusicIndex;
        audioSource = GetComponent<AudioSource>();
        Mykoreographer = GetComponent<Koreographer>();
        simplemusicPlayer = GetComponent<SimpleMusicPlayer>();
    }

    private void Start()
    {
        Mykoreographer.EventDelayInSeconds = MyGameSettings.GetMyGameSettingsInstance().TimeShifting / 1000.0f;
        audioSource.volume = MyGameSettings.GetMyGameSettingsInstance().Volume / 100.0f;

        simplemusicPlayer.LoadSong(
            MusicResource.GetMusicResourceInstance().koreographies[musicIndex], 0, false
        );

        timeInfo[0] = 0.0f;
        timeInfo[1] = MusicResource.GetMusicResourceInstance().musics[musicIndex].length;

        PlayMusicWithInvoke();
        MyGameManager.GetGameManagerInstance().StartShield();
    }

    private void PlayMusicWithInvoke()
    {
        if (IsInvoking(nameof(PlayMusic)))
        {
            CancelInvoke(nameof(PlayMusic));
        }
        GameObject.Find("UIPanel").GetComponent<GameUI>().StartCountDown();
        Invoke(nameof(PlayMusic), 3.0f);
    }

    private void PlayMusic()
    {
        if (!simplemusicPlayer.IsPlaying)
        {
            MyGameManager.GetGameManagerInstance().isPause = false;
            StartCoroutine(ProgressUIUpdateCall());
            simplemusicPlayer.Play();
        }
    }

    private void PauseMusic()
    {
        if (IsInvoking(nameof(PlayMusic)))
        {
            CancelInvoke(nameof(PlayMusic));
        }
        if (simplemusicPlayer.IsPlaying)
        {
            StopCoroutine(ProgressUIUpdateCall());
            simplemusicPlayer.Pause();
        }
    }

    private void InitialMusic()
    {
        if (simplemusicPlayer.IsPlaying)
        {
            StopCoroutine(ProgressUIUpdateCall());
            simplemusicPlayer.Stop();
        }
        audioSource.time = 0.0f;
        PlayMusicWithInvoke();
    }

    IEnumerator ProgressUIUpdateCall()
    {
        while (audioSource.time <= MusicResource.GetMusicResourceInstance().musics[musicIndex].length)
        {
            timeInfo[0] = audioSource.time;
            GameObject.Find("UIPanel").GetComponent<GameUI>().ProgressUpdate(timeInfo);
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void Update()
    {
        if (MyGameManager.GetGameManagerInstance().playerLife > 0 && audioSource.time >= audioSource.clip.length - 0.3f)
        {
            MyGameManager.GetGameManagerInstance().isWin = true;
            MyGameManager.GetGameManagerInstance().EndGame();
        }
    }
}
