
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResultCalculator : MonoBehaviour
{
    public class CustomerResult
    {
        public string customerName;
        public int arrivalTime;
        public int burstTime;
        public int finishTime;
        public int turnaroundTime;
        public int waitingTime;
    }

    public List<CustomerResult> CalculateResults(
        CustomerData[] customers,
        List<SchedulingManager.ScheduleSegment> schedule)
    {
        List<CustomerResult> results = new List<CustomerResult>();

        if (customers == null || schedule == null)
            return results;

        foreach (CustomerData customer in customers)
        {
            if (customer.burstTime <= 0)
                continue;

            int finishTime = schedule
                .Where(s => s.customerName == customer.customerName)
                .Select(s => s.endTime)
                .DefaultIfEmpty(0)
                .Max();

            int turnaroundTime =
                finishTime - customer.arrivalTime;

            int waitingTime =
                turnaroundTime - customer.burstTime;

            results.Add(new CustomerResult
            {
                customerName = customer.customerName,
                arrivalTime = customer.arrivalTime,
                burstTime = customer.burstTime,
                finishTime = finishTime,
                turnaroundTime = turnaroundTime,
                waitingTime = waitingTime
            });
        }

        return results;
    }

    public float CalculateAverageTAT(List<CustomerResult> results)
    {
        if (results == null || results.Count == 0)
            return 0f;

        return (float)results.Average(r => r.turnaroundTime);
    }

    public float CalculateAverageWT(List<CustomerResult> results)
    {
        if (results == null || results.Count == 0)
            return 0f;

        return (float)results.Average(r => r.waitingTime);
    }
}
