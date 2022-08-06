using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;

public class InfoUI : MonoBehaviour
{
    GComponent infoUI_root;
    UIPanel infoUIPanel;
    GButton backButton;

    private void Awake()
    {
        infoUIPanel = GetComponent<UIPanel>();
    }

    void Start()
    {
        infoUI_root = infoUIPanel.ui;

        backButton = infoUI_root.GetChild("backButton").asButton;
        backButton.onClick.Add(delegate () { SceneManager.LoadScene("StartScene"); });
    }
}
