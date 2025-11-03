using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskItemUI : MonoBehaviour
{
    public TMP_Text taskNameText;
    public TMP_Text dueDateText;
    public Toggle checkbox;

    private TaskItem currentTask;

    public void Setup(TaskItem task)
    {
        currentTask = task;

        taskNameText.text = task.Title;
        dueDateText.text = task.DueDate.ToShortDateString();
        checkbox.isOn = task.IsCompleted;

        // Prevent duplicate event calls
        checkbox.onValueChanged.RemoveAllListeners();
        checkbox.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        currentTask.IsCompleted = isOn;
        UIManager.Instance.UpdateProgressBar();

        if (isOn)
        {
            // 🟢 Add XP when task completed
            PlayerProgress.Instance.AddXP(currentTask.xp);
        }
        else
        {
            // 🔴 Remove XP if unchecked (optional)
            PlayerProgress.Instance.AddXP(-currentTask.xp);
        }
    }

}
