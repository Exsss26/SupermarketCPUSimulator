using UnityEngine;
using TMPro;

public class SummaryUI : MonoBehaviour
{
    [Header("Input Data")]
    public TMP_InputField[] arrivalInputs;
    public TMP_InputField[] burstInputs;

    [Header("Dropdown")]
    public TMP_Dropdown methodDropdown;

    [Header("Summary Text")]
    public TMP_Text selectedAlgorithmText;
    public TMP_Text totalCustomersText;
    public TMP_Text totalBurstTimeText;

    void Start()
    {
        methodDropdown.onValueChanged.AddListener(delegate { UpdateSummary(); });

        foreach (TMP_InputField input in arrivalInputs)
        {
            input.onValueChanged.AddListener(delegate { UpdateSummary(); });
        }

        foreach (TMP_InputField input in burstInputs)
        {
            input.onValueChanged.AddListener(delegate { UpdateSummary(); });
        }

        UpdateSummary();
    }

    void UpdateSummary()
    {
        int totalCustomers = 0;
        int totalBurstTime = 0;

        for (int i = 0; i < burstInputs.Length; i++)
        {
            int burst = 0;

            int.TryParse(burstInputs[i].text, out burst);

            if (burst > 0)
            {
                totalCustomers++;
                totalBurstTime += burst;
            }
        }

        selectedAlgorithmText.text =
            "Selected Algorithm: " +
            methodDropdown.options[methodDropdown.value].text;

        totalCustomersText.text =
            "Total Customers: " + totalCustomers;

        totalBurstTimeText.text =
            "Total Burst Time: " + totalBurstTime;
    }
}