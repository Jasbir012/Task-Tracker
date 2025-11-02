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
        checkbox.isOn = task.IsComplete;

        // Prevent duplicate event calls
        checkbox.onValueChanged.RemoveAllListeners();
        checkbox.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        currentTask.IsComplete = isOn;
        UIManager.Instance.UpdateProgressBar();
    }
}
