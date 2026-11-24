using UnityEngine;
using TMPro;

public class MethodSelectionUI : MonoBehaviour
{
    [Header("Dropdown Metode")]
    public TMP_Dropdown methodDropdown;

    [Header("Quantum Time")]
    public GameObject quantumSection;

    [Header("Priority")]
    public GameObject[] priorityInputs;
    public GameObject priorityHeader;

    void Start()
    {
        methodDropdown.onValueChanged.AddListener(UpdateMethodUI);

        UpdateMethodUI(methodDropdown.value);
    }

    void UpdateMethodUI(int selectedMethod)
    {
        bool isRoundRobin = selectedMethod == 0;
        bool isPriority = selectedMethod == 4;

        // Tampilkan Quantum Time hanya untuk Round Robin
        quantumSection.SetActive(isRoundRobin);

        // Tampilkan seluruh input Priority
        foreach (GameObject input in priorityInputs)
        {
            if (input != null)
            {
                input.SetActive(isPriority);
            }
        }

        // Tampilkan judul kolom Priority
        if (priorityHeader != null)
        {
            priorityHeader.SetActive(isPriority);
        }
    }
}