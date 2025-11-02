using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Header UI")]
    public TMP_Text dateText;
    public Slider progressBar;

    [Header("Task List")]
    public Transform taskListContent;
    public GameObject taskItemPrefab;

    [Header("Add Task Panel")]
    public GameObject addTaskPanel;
    public TMP_InputField inputTaskName;
    public TMP_InputField inputTaskDescription;
    public TMP_InputField inputDueDate; // Optional (for future)
    public Button confirmAddButton;
    public Button cancelButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 🗓 Display today's date
        dateText.text = DateTime.Now.ToString("dddd, dd MMM yyyy");

        // 🫗 Hide Add Task Panel initially
        addTaskPanel.SetActive(false);

        // 🧩 Assign button listeners
        confirmAddButton.onClick.AddListener(OnAddTaskClicked);
        cancelButton.onClick.AddListener(CloseAddTaskPanel);

        // 🌀 Refresh task list on start
        RefreshTasks();
    }

    /// <summary>
    /// Refresh the list of visible tasks in the ScrollView.
    /// </summary>
    public void RefreshTasks()
    {
        // Clear current list
        foreach (Transform child in taskListContent)
            Destroy(child.gameObject);

        // ✅ Get all saved tasks
        List<TaskItem> allTasks = TaskManager.Instance.GetAllTasks();

        // Instantiate each task item
        foreach (var task in allTasks)
        {
            GameObject t = Instantiate(taskItemPrefab, taskListContent);
            t.GetComponent<TaskItemUI>().Setup(task);
        }

        // Update progress bar
        UpdateProgressBar();
    }

    /// <summary>
    /// Show the Add Task popup panel.
    /// </summary>
    public void OpenAddTaskPanel()
    {
        addTaskPanel.SetActive(true);
        inputTaskName.text = "";
        inputTaskDescription.text = "";
        inputDueDate.text = "";
    }

    /// <summary>
    /// Hide the Add Task popup panel.
    /// </summary>
    public void CloseAddTaskPanel()
    {
        addTaskPanel.SetActive(false);
    }

    /// <summary>
    /// Called when the "Add Task" confirm button is pressed.
    /// </summary>
    public void OnAddTaskClicked()
    {
        string name = inputTaskName.text.Trim();
        string desc = inputTaskDescription.text.Trim();

        // ⏰ Ensure time is stripped (midnight)
        DateTime dueDate = DateTime.Now.Date;

        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning("⚠️ Task name cannot be empty!");
            return;
        }

        // ➕ Add task and refresh list
        TaskManager.Instance.AddTask(name, desc, dueDate);
        CloseAddTaskPanel();
        RefreshTasks();
    }

    /// <summary>
    /// Update the progress bar based on completed tasks.
    /// </summary>
    public void UpdateProgressBar()
    {
        List<TaskItem> all = TaskManager.Instance.GetAllTasks();

        if (all.Count == 0)
        {
            progressBar.value = 0;
            return;
        }

        int completed = all.FindAll(t => t.IsComplete).Count;
        progressBar.value = (float)completed / all.Count;
    }
}
