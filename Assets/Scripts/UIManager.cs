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
    public TMP_InputField inputXP;
    public TMP_InputField inputDueDate;
    public Button confirmAddButton;
    public Button cancelButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dateText.text = DateTime.Now.ToString("dddd, dd MMM yyyy");
        addTaskPanel.SetActive(false);

        confirmAddButton.onClick.AddListener(OnAddTaskClicked);
        cancelButton.onClick.AddListener(CloseAddTaskPanel);

        RefreshTasks();
    }

    public void RefreshTasks()
    {
        foreach (Transform child in taskListContent)
            Destroy(child.gameObject);

        List<TaskItem> allTasks = TaskManager.Instance.GetAllTasks();

        foreach (var task in allTasks)
        {
            GameObject t = Instantiate(taskItemPrefab, taskListContent);
            t.GetComponent<TaskItemUI>().Setup(task);
        }

        UpdateProgressBar();
    }

    public void OpenAddTaskPanel()
    {
        addTaskPanel.SetActive(true);
        inputTaskName.text = "";
        inputTaskDescription.text = "";
        inputXP.text = "";
        inputDueDate.text = "";
    }

    public void CloseAddTaskPanel()
    {
        addTaskPanel.SetActive(false);
    }

    public void OnAddTaskClicked()
    {
        string name = inputTaskName.text.Trim();
        string desc = inputTaskDescription.text.Trim();
        string xpText = inputXP.text.Trim();
        int xp = 10;

        if (!string.IsNullOrEmpty(xpText))
            int.TryParse(xpText, out xp);

        DateTime dueDate = DateTime.Now.Date;
        if (DateTime.TryParse(inputDueDate.text, out DateTime parsed))
            dueDate = parsed;

        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning("⚠️ Task name cannot be empty!");
            return;
        }

        TaskManager.Instance.AddTask(name, desc, dueDate, xp);
        CloseAddTaskPanel();
        RefreshTasks();
    }

    public void UpdateProgressBar()
    {
        List<TaskItem> all = TaskManager.Instance.GetAllTasks();

        if (all.Count == 0)
        {
            progressBar.value = 0;
            return;
        }

        int completed = all.FindAll(t => t.IsCompleted).Count;
        progressBar.value = (float)completed / all.Count;
    }
}
