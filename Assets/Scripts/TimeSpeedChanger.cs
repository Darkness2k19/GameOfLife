using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeSpeedChanger : MonoBehaviour
{
    [SerializeField]
    private FieldManager m_fieldManager;
    [SerializeField]
    private Scrollbar m_scrollbar;
    [SerializeField]
    private Text m_timeSpeedText;
    [SerializeField]
    private Text m_pauseButtonText;

    public void UpdateTimeSpeed()
    {
        if (m_scrollbar.value > 0.5f)
        {
            m_fieldManager.timeSpeed = Mathf.Lerp(1, 10, (m_scrollbar.value - 0.5f) / 0.5f);
        }
        else
        {
            m_fieldManager.timeSpeed = Mathf.Lerp(0.25f, 1, m_scrollbar.value / 0.5f);
        }
        m_fieldManager.timeSpeed = (float)Math.Round(m_fieldManager.timeSpeed, 2);
        m_timeSpeedText.text = "x" + m_fieldManager.timeSpeed.ToString();
    }

    public void SwapStoppedState()
    {
        if (m_fieldManager.IsFinished())
        {
            return;
        }

        var manager = FieldManager.GetInstance();
        manager.isStopped = !manager.isStopped;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !UIManager.GetInstance().isInputCaptured)
        {
            SwapStoppedState();
        }


        if (FieldManager.GetInstance().isStopped)
        {
            m_pauseButtonText.text = "Continue";
        }
        else
        {
            m_pauseButtonText.text = "Pause";
        }
    }
}