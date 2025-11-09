using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskItemUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text taskNameText;
    public TMP_Text descriptionText;
    public TMP_Text dueDateText;
    public Toggle checkbox;
    public Button mainButton; 

    private TaskItem currentTask;
    private bool isExpanded = false;

    public void Setup(TaskItem task)
    {
        currentTask = task;

        taskNameText.text = task.Title;
        descriptionText.text = string.IsNullOrEmpty(task.Description)
            ? ""
            : task.Description;

        dueDateText.text = "Due: " + task.DueDate.ToString("dd MMM yyyy");
        checkbox.isOn = task.IsCompleted;

        // Start hidden
        descriptionText.gameObject.SetActive(false);

        // Clean listeners
        checkbox.onValueChanged.RemoveAllListeners();
        mainButton.onClick.RemoveAllListeners();

        checkbox.onValueChanged.AddListener(OnToggleChanged);
        mainButton.onClick.AddListener(ToggleDescription);
    }

    void ToggleDescription()
    {
        isExpanded = !isExpanded;
        descriptionText.gameObject.SetActive(isExpanded);
    }

    void OnToggleChanged(bool isOn)
    {
        currentTask.IsCompleted = isOn;
        UIManager.Instance.UpdateProgressBar();

        if (isOn)
            PlayerProgress.Instance.AddXP(currentTask.xp);
        else
            PlayerProgress.Instance.AddXP(-currentTask.xp);
    }
}
