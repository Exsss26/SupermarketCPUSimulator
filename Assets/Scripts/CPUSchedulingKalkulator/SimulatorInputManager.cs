
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class SimulatorInputManager : MonoBehaviour
{
    [Header("Input Data")]
    public TMP_InputField[] arrivalInputs;
    public TMP_InputField[] burstInputs;
    public TMP_InputField[] priorityInputs;

    [Header("Dropdown")]
    public TMP_Dropdown methodDropdown;

    [Header("Quantum")]
    public TMP_InputField quantumInput;

    [Header("Error Message")]
    public TMP_Text errorMessage;

    void Start()
    {
        errorMessage.text = "";
    }

    public bool ValidateInputs()
    {
        errorMessage.text = "";

        int totalCustomers = 0;

        for (int i = 0; i < burstInputs.Length; i++)
        {
            int arrival;
            int burst;

            if (!int.TryParse(arrivalInputs[i].text, out arrival) || arrival < 0)
            {
                errorMessage.text = "Arrival Time harus 0 atau lebih!";
                return false;
            }

            if (!int.TryParse(burstInputs[i].text, out burst) || burst < 0)
            {
                errorMessage.text = "Burst Time tidak boleh negatif!";
                return false;
            }

            if (burst > 0)
                totalCustomers++;

            if (methodDropdown.value == 4 && burst > 0)
            {
                int priority;

                if (!int.TryParse(priorityInputs[i].text, out priority) || priority < 0)
                {
                    errorMessage.text = "Priority harus 0 atau lebih!";
                    return false;
                }
            }
        }

        if (totalCustomers == 0)
        {
            errorMessage.text = "Masukkan minimal satu customer!";
            return false;
        }

        if (methodDropdown.value == 0)
        {
            int quantum;

            if (!int.TryParse(quantumInput.text, out quantum) || quantum <= 0)
            {
                errorMessage.text = "Quantum Time harus lebih dari 0!";
                return false;
            }
        }

        return true;
    }

    public void RunSimulator()
    {
        if (!ValidateInputs())
            return;

        if (SimulationData.Instance == null)
        {
            errorMessage.text = "SimulationData belum tersedia!";
            return;
        }

        List<CustomerData> customerList = new List<CustomerData>();

        for (int i = 0; i < burstInputs.Length; i++)
        {
            int burst = int.Parse(burstInputs[i].text);

            if (burst <= 0)
                continue;

            int arrival = int.Parse(arrivalInputs[i].text);
            int priority = 0;

            if (methodDropdown.value == 4)
                priority = int.Parse(priorityInputs[i].text);

            customerList.Add(new CustomerData
            {
                customerName = "P" + (i + 1),
                arrivalTime = arrival,
                burstTime = burst,
                priority = priority
            });
        }

        SimulationData.Instance.customers = customerList.ToArray();
        SimulationData.Instance.selectedMethod = methodDropdown.value;

        SimulationData.Instance.quantumTime = 0;

        if (methodDropdown.value == 0)
        {
            SimulationData.Instance.quantumTime =
                int.Parse(quantumInput.text);
        }

        SceneManager.LoadScene("SupermarketScene");
    }
}
