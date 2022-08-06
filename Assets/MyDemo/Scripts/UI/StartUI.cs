using UnityEngine;
using UnityEditor;
using FairyGUI;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    GComponent startUI_root;
    UIPanel startUIPanel;
    GButton tutorialButton;
    GButton quitButton;
    GButton settingButton;
    GButton infoButton;
    GButton selectButton;

    private void Awake()
    {
        startUIPanel = GetComponent<UIPanel>();
    }
    private void Start()
    {
        startUI_root = startUIPanel.ui;

        quitButton = startUI_root.GetChild("quitButton").asButton;
        tutorialButton = startUI_root.GetChild("tutorialButton").asButton;
        settingButton = startUI_root.GetChild("settingButton").asButton;
        infoButton = startUI_root.GetChild("infoButton").asButton;
        selectButton = startUI_root.GetChild("startButton").asButton;

        quitButton.onClick.Add(QuitGame);
        infoButton.onClick.Add(delegate () { SceneManager.LoadScene("Info"); });
        tutorialButton.onClick.Add(delegate () { SceneManager.LoadScene("Tutotral"); });
        selectButton.onClick.Add(delegate () { SceneManager.LoadScene("SelectMusic"); });
        settingButton.onClick.Add(delegate () { SceneManager.LoadScene("Setting"); });
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();  
#endif
    }
}
