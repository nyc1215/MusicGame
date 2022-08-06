using FairyGUI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutotralUI : MonoBehaviour
{
    GComponent tutotralUI_root;
    UIPanel tutotralUIPanel;
    GButton backButton;

    private void Awake()
    {
        tutotralUIPanel = GetComponent<UIPanel>();
    }

    void Start()
    {
        tutotralUI_root = tutotralUIPanel.ui;
        backButton = tutotralUI_root.GetChild("backButton").asButton;
        backButton.onClick.Add(delegate () { SceneManager.LoadScene("StartScene"); });
    }
}
