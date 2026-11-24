using UnityEngine;
using TMPro;

public class ResetInputUI : MonoBehaviour
{
    [Header("Arrival Time")]
    public TMP_InputField[] arrivalInputs;

    [Header("Burst Time")]
    public TMP_InputField[] burstInputs;

    [Header("Priority")]
    public TMP_InputField[] priorityInputs;

    [Header("Quantum Time")]
    public TMP_InputField quantumInput;

    public void ResetAllInputs()
    {
        foreach (TMP_InputField input in arrivalInputs)
            input.text = "0";

        foreach (TMP_InputField input in burstInputs)
            input.text = "0";

        foreach (TMP_InputField input in priorityInputs)
            input.text = "0";

        if (quantumInput != null)
            quantumInput.text = "0";
    }
}