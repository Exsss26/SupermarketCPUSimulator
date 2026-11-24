
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ResultsUI : MonoBehaviour
{
    [System.Serializable]
    public class CustomerRowUI
    {
        public TMP_Text arrivalTime;
        public TMP_Text burstTime;
        public TMP_Text finishTime;
        public TMP_Text turnaroundTime;
        public TMP_Text waitingTime;
    }

    [Header("References")]
    public ResultCalculator resultCalculator;
    public SchedulingManager schedulingManager;

    [Header("Customer Rows")]
    public CustomerRowUI[] customerRows = new CustomerRowUI[4];

    [Header("Average Text")]
    public TMP_Text averageTAT;
    public TMP_Text averageWT;

    void Start()
    {
        DisplayResults();
    }

    public void DisplayResults()
    {
        if (SimulationData.Instance == null)
        {
            Debug.LogError("SimulationData tidak ditemukan!");
            return;
        }

        if (resultCalculator == null)
            resultCalculator = GetComponent<ResultCalculator>();

        if (schedulingManager == null)
            schedulingManager = FindFirstObjectByType<SchedulingManager>();

        if (resultCalculator == null || schedulingManager == null)
        {
            Debug.LogError("ResultCalculator atau SchedulingManager belum diatur!");
            return;
        }

        CustomerData[] customers = SimulationData.Instance.customers;

        List<SchedulingManager.ScheduleSegment> schedule =
            schedulingManager.GenerateSchedule(
                customers,
                SimulationData.Instance.selectedMethod,
                SimulationData.Instance.quantumTime
            );

        List<ResultCalculator.CustomerResult> results =
            resultCalculator.CalculateResults(customers, schedule);

        foreach (CustomerRowUI row in customerRows)
        {
            if (row == null)
                continue;

            SetRowText(row, "-", "-", "-", "-", "-");
        }

        foreach (var result in results)
        {
            int index = int.Parse(
                result.customerName.Substring(1)
            ) - 1;

            if (index < 0 || index >= customerRows.Length)
                continue;

            CustomerRowUI row = customerRows[index];

            if (row == null)
                continue;

            SetRowText(
                row,
                result.arrivalTime.ToString(),
                result.burstTime.ToString(),
                result.finishTime.ToString(),
                result.turnaroundTime.ToString(),
                result.waitingTime.ToString()
            );
        }

        if (averageTAT != null)
        {
            averageTAT.text =
                resultCalculator.CalculateAverageTAT(results)
                .ToString("0.00");
        }

        if (averageWT != null)
        {
            averageWT.text =
                resultCalculator.CalculateAverageWT(results)
                .ToString("0.00");
        }

        Debug.Log("Tabel hasil CPU berhasil diperbarui!");
    }

    void SetRowText(
        CustomerRowUI row,
        string arrival,
        string burst,
        string finish,
        string turnaround,
        string waiting)
    {
        if (row.arrivalTime != null)
            row.arrivalTime.text = arrival;

        if (row.burstTime != null)
            row.burstTime.text = burst;

        if (row.finishTime != null)
            row.finishTime.text = finish;

        if (row.turnaroundTime != null)
            row.turnaroundTime.text = turnaround;

        if (row.waitingTime != null)
            row.waitingTime.text = waiting;
    }
}
