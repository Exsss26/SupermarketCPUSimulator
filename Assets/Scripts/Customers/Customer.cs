
using System;

[Serializable]
public class Customer
{
    public string customerID;
    public int arrivalTime;
    public int burstTime;
    public int remainingTime;
    public int priority;

    public Customer(string id, int arrival, int burst, int priorityLevel)
    {
        customerID = id;
        arrivalTime = arrival;
        burstTime = burst;
        remainingTime = burst;
        priority = priorityLevel;
    }
}
