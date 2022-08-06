using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using System.Collections;

public class MyGameManager : MonoBehaviour
{
    //音乐资源单例对象
    public MusicResource musicResource = MusicResource.GetMusicResourceInstance();
    //游戏设置单例对象
    public MyGameSettings gameSettings = MyGameSettings.GetMyGameSettingsInstance();

    //游戏管理器单例对象
    private MyGameManager() { }
    private static MyGameManager MyGameManagerInstance;
    public static MyGameManager GetGameManagerInstance()
    {
        return MyGameManagerInstance;
    }

    //游戏管理器成员
    private int selectedMusicIndex;
    public int SelectedMusicIndex
    {
        get
        {
            return selectedMusicIndex;
        }
        set
        {
            if (MusicResource.GetMusicResourceInstance().isAllLoad)
            {
                if (value < MusicResource.GetMusicResourceInstance().musics.Length && value >= 0)
                {
                    selectedMusicIndex = value;
                }
                else
                {
                    Debug.Log("selected music index out of range");
                }
            }
            else
            {
                Debug.Log("musics are not loaded");
            }
        }
    }

    public int playerLife;
    public int playerShield;

    public bool isWin;
    public bool isPause;
    public readonly int totalcount = 3;

    private void Awake()
    {
        MyGameManagerInstance = this;
    }

    private void Start()
    {
        if (!MusicResource.GetMusicResourceInstance().isAllLoad)
        {
            MusicResource.GetMusicResourceInstance().musics = Resources.LoadAll<AudioClip>("音乐");
            MusicResource.GetMusicResourceInstance().koreographies = Resources.LoadAll<Koreography>("Koreographer");
            MusicResource.GetMusicResourceInstance().isAllLoad = true;
        }
        playerLife = 10;
        playerShield = 0;
        isWin = false;
        isPause = false;
    }

    public void InitialGame()
    {
        playerLife = 10;
        playerShield = 0;
        GameObject.Find("UIPanel").GetComponent<GameUI>().LifeTextUpdate();
        GameObject.Find("UIPanel").GetComponent<GameUI>().ShieldUpdate();
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0, -1, 5);
        isWin = false;
        isPause = false;
        GameObject.Find("NoteCreater").SendMessage("ClearAllNote", SendMessageOptions.RequireReceiver);
        GameObject.Find("MusicPlayer").SendMessage("InitialMusic", SendMessageOptions.RequireReceiver);
    }
    public void PauseGame()
    {
        isPause = true;
        GameObject.Find("MusicPlayer").SendMessage("PauseMusic", SendMessageOptions.RequireReceiver);
    }
    public void ContinueGame()
    {
        isPause = false;
        GameObject.Find("MusicPlayer").GetComponent<SimpleMusicPlayer>().Pause();
        GameObject.Find("MusicPlayer").SendMessage("PlayMusicWithInvoke", SendMessageOptions.RequireReceiver);
    }
    public void QuitGame()
    {
        isPause = false;
        StopShield();
        SceneManager.LoadScene("SelectMusic");
    }

    public void EndGame()
    {
        isPause = true;
        StopShield();
        GameObject.Find("UIPanel").GetComponent<GameUI>().GameEnd(isWin);
    }

    public IEnumerator ShieldCountDown()
    {
        while (true)
        {
            if (!MyGameManager.GetGameManagerInstance().isPause)
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }

            if (!MyGameManager.GetGameManagerInstance().isPause)
            {
                playerShield--;
                if (GameObject.Find("UIPanel") != null)
                {
                    GameObject.Find("UIPanel").GetComponent<GameUI>().ShieldUpdate();
                }
            }
        }
    }

    public void StartShield()
    {
        Debug.Log("StartShield");
        StartCoroutine(ShieldCountDown());
    }

    public void StopShield()
    {
        Debug.Log("StopShield");
        StopAllCoroutines();
        //StopCoroutine(ShieldCountDown());
    }
}

public class MyGameSettings
{
    private static MyGameSettings MyGameSettingsInstance;
    private MyGameSettings() { }
    public static MyGameSettings GetMyGameSettingsInstance()
    {
        if (MyGameSettingsInstance == null)
        {
            MyGameSettingsInstance = new MyGameSettings();
            MyGameSettingsInstance.Initial();
        }
        return MyGameSettingsInstance;
    }

    //游戏设置成员
    private int volume;
    public int Volume
    {
        get
        {
            return volume;
        }
        set
        {
            if (value >= 0 && value <= 100)
            {
                volume = value;
            }
        }
    }
    private int timeShifting;
    public int TimeShifting
    {
        get
        {
            return timeShifting;
        }
        set
        {
            if (value >= -3000 && value <= 3000)
            {
                timeShifting = value;
            }
            else if (value < -3000)
            {
                timeShifting = -3000;
            }
            else if (value > 3000)
            {
                timeShifting = 3000;
            }
        }
    }

    public void Initial()
    {
        Volume = 50;
        TimeShifting = 0;
    }
}

public class MusicResource
{
    private static MusicResource MusicResourceInstance;
    private MusicResource() { }
    public static MusicResource GetMusicResourceInstance()
    {
        if (MusicResourceInstance == null)
        {
            MusicResourceInstance = new MusicResource();
            MusicResourceInstance.Initial();
        }
        return MusicResourceInstance;
    }

    //成员
    public AudioClip[] musics;
    public bool isAllLoad;
    public Koreography[] koreographies;

    private void Initial()
    {
        isAllLoad = false;
    }
}