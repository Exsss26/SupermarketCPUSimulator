
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GanttChartUI : MonoBehaviour
{
    [Header("Gantt Chart")]
    public RectTransform ganttContainer;
    public GameObject ganttBlockPrefab;
    public SchedulingManager schedulingManager;

    [Header("Time Labels")]
    public RectTransform timeContainer;
    public GameObject timeLabelPrefab;

    [Header("Block Settings")]
    public float blockHeight = 40f;

    void Start()
    {
        GenerateGanttChart();
    }

    public void GenerateGanttChart()
    {
        if (SimulationData.Instance == null)
        {
            Debug.LogError("SimulationData tidak ditemukan!");
            return;
        }

        if (ganttContainer == null || ganttBlockPrefab == null ||
            timeContainer == null || timeLabelPrefab == null)
        {
            Debug.LogError("Referensi Gantt Chart belum lengkap!");
            return;
        }

        if (schedulingManager == null)
            schedulingManager = FindFirstObjectByType<SchedulingManager>();

        if (schedulingManager == null)
        {
            Debug.LogError("SchedulingManager tidak ditemukan!");
            return;
        }

        CustomerData[] customers = SimulationData.Instance.customers;

        List<SchedulingManager.ScheduleSegment> schedule =
            schedulingManager.GenerateSchedule(
                customers,
                SimulationData.Instance.selectedMethod,
                SimulationData.Instance.quantumTime
            );

        if (schedule == null || schedule.Count == 0)
        {
            Debug.LogError("Jadwal Gantt Chart kosong!");
            return;
        }

        ClearContainer(ganttContainer);
        ClearContainer(timeContainer);

        int totalTime = schedule.Max(s => s.endTime);

        if (totalTime <= 0)
            return;

        float chartWidth = ganttContainer.rect.width;
        float unitWidth = chartWidth / totalTime;

        // Membuat blok pelanggan
        foreach (var segment in schedule)
        {
            float duration = segment.endTime - segment.startTime;
            float blockWidth = duration * unitWidth;

            GameObject block = Instantiate(
                ganttBlockPrefab,
                ganttContainer
            );

            block.SetActive(true);
            block.name = "Block_" + segment.customerName;

            RectTransform rect = block.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(blockWidth, blockHeight);

            float positionX =
                -chartWidth / 2f +
                segment.startTime * unitWidth +
                blockWidth / 2f;

            rect.anchoredPosition = new Vector2(positionX, 0);

            TMP_Text label =
                block.GetComponentInChildren<TMP_Text>(true);

            if (label != null)
                label.text = segment.customerName;

            UnityEngine.UI.Image image =
                block.GetComponent<UnityEngine.UI.Image>();

            if (image != null)
                image.color = GetCustomerColor(segment.customerName);
        }

        // Membuat angka waktu
        List<int> timePoints = schedule
            .SelectMany(s => new int[] { s.startTime, s.endTime })
            .Distinct()
            .OrderBy(t => t)
            .ToList();

        float timeWidth = timeContainer.rect.width;

        for (int i = 0; i < timePoints.Count; i++)
        {
            int time = timePoints[i];

            GameObject timeLabel = Instantiate(
                timeLabelPrefab,
                timeContainer
            );

            timeLabel.SetActive(true);
            timeLabel.name = "Time_" + time;

            RectTransform rect =
                timeLabel.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);

            float positionX =
                -timeWidth / 2f + time * (timeWidth / totalTime);

            if (i == 0)
                rect.pivot = new Vector2(0f, 0.5f);
            else if (i == timePoints.Count - 1)
                rect.pivot = new Vector2(1f, 0.5f);
            else
                rect.pivot = new Vector2(0.5f, 0.5f);

            rect.anchoredPosition = new Vector2(positionX, 0);

            TMP_Text text =
                timeLabel.GetComponent<TMP_Text>();

            if (text != null)
                text.text = time.ToString();
        }

        Debug.Log("Gantt Chart berhasil dibuat!");
    }

    void ClearContainer(RectTransform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }

    Color GetCustomerColor(string customerName)
    {
        switch (customerName)
        {
            case "P1":
                return new Color32(255, 105, 180, 255);

            case "P2":
                return new Color32(190, 100, 220, 255);

            case "P3":
                return new Color32(100, 145, 255, 255);

            case "P4":
                return new Color32(255, 165, 75, 255);

            default:
                return Color.white;
        }
    }
}
