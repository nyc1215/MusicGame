using FairyGUI;
using UnityEngine.SceneManagement;
using UnityEngine;
public class SettingUI : MonoBehaviour
{
    GComponent settingUI_root;
    UIPanel settingUIPanel;
    GButton backButton;
    GButton resetButton;
    GTextField resetButtonText;

    GTextField volText;
    GTextField timeText;

    GButton volLeftButton;
    GButton volRightButton;
    GButton timeLeftButton;
    GButton timeRightButton;

    private void Awake()
    {
        settingUIPanel = GetComponent<UIPanel>();
    }

    private void Start()
    {
        settingUI_root = settingUIPanel.ui;

        volText = settingUI_root.GetChild("vol").asTextField;
        volText.SetVar("vol", MyGameSettings.GetMyGameSettingsInstance().Volume.ToString()).FlushVars();

        timeText = settingUI_root.GetChild("time").asTextField;
        timeText.SetVar("time", MyGameSettings.GetMyGameSettingsInstance().TimeShifting.ToString()).FlushVars();

        backButton = settingUI_root.GetChild("backButton").asButton;
        backButton.onClick.Add(delegate () { SceneManager.LoadScene("StartScene"); });

        resetButtonText = settingUI_root.GetChild("resetText").asTextField;
        resetButton = settingUI_root.GetChild("resetButton").asButton;
        resetButton.onRollOver.Add(delegate () { resetButtonText.visible = true; });
        resetButton.onRollOut.Add(delegate () { resetButtonText.visible = false; });
        resetButton.onClick.Add(delegate ()
        {
            MyGameSettings.GetMyGameSettingsInstance().Initial();
            volText.SetVar("vol", MyGameSettings.GetMyGameSettingsInstance().Volume.ToString()).FlushVars();
            timeText.SetVar("time", MyGameSettings.GetMyGameSettingsInstance().TimeShifting.ToString()).FlushVars();
        });

        volLeftButton = settingUI_root.GetChild("volLeft").asButton;
        volLeftButton.onClick.Add(VolDown);
        //长按
        volLeftButton.onTouchBegin.Add(delegate () { InvokeRepeating(nameof(VolDown), 0.3f, 0.1f); });
        volLeftButton.onTouchEnd.Add(delegate ()
        {
            if (IsInvoking(nameof(VolDown)))
            {
                CancelInvoke(nameof(VolDown));
            }
        });

        volRightButton = settingUI_root.GetChild("volRight").asButton;
        volRightButton.onClick.Add(VolUp);
        //长按
        volRightButton.onTouchBegin.Add(delegate () { InvokeRepeating(nameof(VolUp), 0.3f, 0.1f); });
        volRightButton.onTouchEnd.Add(delegate ()
        {
            if (IsInvoking(nameof(VolUp)))
            {
                CancelInvoke(nameof(VolUp));
            }
        });

        timeLeftButton = settingUI_root.GetChild("TimeLeft").asButton;
        timeLeftButton.onClick.Add(TimeShiftDown);
        timeLeftButton.onTouchBegin.Add(delegate () { InvokeRepeating(nameof(TimeShiftDownLongTouch), 0.3f, 0.1f); });
        timeLeftButton.onTouchEnd.Add(delegate ()
        {
            if (IsInvoking(nameof(TimeShiftDownLongTouch)))
            {
                CancelInvoke(nameof(TimeShiftDownLongTouch));
            }
        });

        timeRightButton = settingUI_root.GetChild("TimeRight").asButton;
        timeRightButton.onClick.Add(TimeShiftUp);
        timeRightButton.onTouchBegin.Add(delegate () { InvokeRepeating(nameof(TimeShiftUpLongTouch), 0.3f, 0.1f); });
        timeRightButton.onTouchEnd.Add(delegate ()
        {
            if (IsInvoking(nameof(TimeShiftUpLongTouch)))
            {
                CancelInvoke(nameof(TimeShiftUpLongTouch));
            }
        });
    }

    void VolDown()
    {
        MyGameSettings.GetMyGameSettingsInstance().Volume--;
        volText.SetVar("vol", MyGameSettings.GetMyGameSettingsInstance().Volume.ToString()).FlushVars();
    }

    void VolUp()
    {
        MyGameSettings.GetMyGameSettingsInstance().Volume++;
        volText.SetVar("vol", MyGameSettings.GetMyGameSettingsInstance().Volume.ToString()).FlushVars();
    }

    void TimeShiftUp()
    {
        MyGameSettings.GetMyGameSettingsInstance().TimeShifting += 10;
        timeText.SetVar("time", MyGameSettings.GetMyGameSettingsInstance().TimeShifting.ToString()).FlushVars();
    }

    void TimeShiftUpLongTouch()
    {
        MyGameSettings.GetMyGameSettingsInstance().TimeShifting += 50;
        timeText.SetVar("time", MyGameSettings.GetMyGameSettingsInstance().TimeShifting.ToString()).FlushVars();
    }

    void TimeShiftDown()
    {
        MyGameSettings.GetMyGameSettingsInstance().TimeShifting -= 10;
        timeText.SetVar("time", MyGameSettings.GetMyGameSettingsInstance().TimeShifting.ToString()).FlushVars();
    }

    void TimeShiftDownLongTouch()
    {
        MyGameSettings.GetMyGameSettingsInstance().TimeShifting -= 50;
        timeText.SetVar("time", MyGameSettings.GetMyGameSettingsInstance().TimeShifting.ToString()).FlushVars();
    }

}
