
using UnityEngine;

[System.Serializable]
public class CustomerData
{
    public string customerName;
    public int arrivalTime;
    public int burstTime;
    public int priority;
}

public class SimulationData : MonoBehaviour
{
    public static SimulationData Instance;

    public CustomerData[] customers;
    public int selectedMethod;
    public int quantumTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
