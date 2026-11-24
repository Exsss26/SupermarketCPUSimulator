
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SchedulingManager : MonoBehaviour
{
    public class ScheduleSegment
    {
        public string customerName;
        public int startTime;
        public int endTime;
    }

    public List<ScheduleSegment> GenerateSchedule(
        CustomerData[] customers,
        int selectedMethod,
        int quantumTime)
    {
        List<ScheduleSegment> result = new List<ScheduleSegment>();

        if (customers == null || customers.Length == 0)
            return result;

        switch (selectedMethod)
        {
            case 0:
                RoundRobin(customers, quantumTime, result);
                break;

            case 1:
                FCFS(customers, result);
                break;

            case 2:
                SJF(customers, result);
                break;

            case 3:
                SRTF(customers, result);
                break;

            case 4:
                PriorityNonPreemptive(customers, result);
                break;
        }

        return result;
    }

    void FCFS(CustomerData[] customers, List<ScheduleSegment> result)
    {
        int time = 0;

        foreach (var customer in customers
            .OrderBy(c => c.arrivalTime)
            .ThenBy(c => c.customerName))
        {
            if (time < customer.arrivalTime)
                time = customer.arrivalTime;

            AddSegment(result, customer.customerName, time, customer.burstTime);
            time += customer.burstTime;
        }
    }

    void SJF(CustomerData[] customers, List<ScheduleSegment> result)
    {
        List<CustomerData> waiting = new List<CustomerData>(customers);
        int time = 0;

        while (waiting.Count > 0)
        {
            var available = waiting
                .Where(c => c.arrivalTime <= time)
                .OrderBy(c => c.burstTime)
                .ThenBy(c => c.arrivalTime)
                .ThenBy(c => c.customerName)
                .FirstOrDefault();

            if (available == null)
            {
                time = waiting.Min(c => c.arrivalTime);
                continue;
            }

            AddSegment(result, available.customerName, time, available.burstTime);
            time += available.burstTime;
            waiting.Remove(available);
        }
    }

    void PriorityNonPreemptive(
        CustomerData[] customers,
        List<ScheduleSegment> result)
    {
        List<CustomerData> waiting = new List<CustomerData>(customers);
        int time = 0;

        while (waiting.Count > 0)
        {
            var available = waiting
                .Where(c => c.arrivalTime <= time)
                .OrderBy(c => c.priority)
                .ThenBy(c => c.arrivalTime)
                .ThenBy(c => c.customerName)
                .FirstOrDefault();

            if (available == null)
            {
                time = waiting.Min(c => c.arrivalTime);
                continue;
            }

            AddSegment(result, available.customerName, time, available.burstTime);
            time += available.burstTime;
            waiting.Remove(available);
        }
    }

    void SRTF(CustomerData[] customers, List<ScheduleSegment> result)
    {
        Dictionary<CustomerData, int> remaining = customers
            .ToDictionary(c => c, c => c.burstTime);

        int time = 0;

        while (remaining.Count > 0)
        {
            var available = remaining
                .Where(pair => pair.Key.arrivalTime <= time)
                .OrderBy(pair => pair.Value)
                .ThenBy(pair => pair.Key.arrivalTime)
                .ThenBy(pair => pair.Key.customerName)
                .FirstOrDefault();

            if (available.Key == null)
            {
                time = remaining.Keys.Min(c => c.arrivalTime);
                continue;
            }

            int start = time;
            int nextArrival = remaining.Keys
                .Where(c => c.arrivalTime > time)
                .Select(c => c.arrivalTime)
                .DefaultIfEmpty(int.MaxValue)
                .Min();

            int duration = Mathf.Min(
                available.Value,
                nextArrival == int.MaxValue
                    ? available.Value
                    : nextArrival - time
            );

            AddSegment(result, available.Key.customerName, start, duration);

            time += duration;
            remaining[available.Key] -= duration;

            if (remaining[available.Key] <= 0)
                remaining.Remove(available.Key);
        }
    }

    void RoundRobin(
        CustomerData[] customers,
        int quantumTime,
        List<ScheduleSegment> result)
    {
        if (quantumTime <= 0)
            return;

        Queue<CustomerData> readyQueue = new Queue<CustomerData>();

        Dictionary<CustomerData, int> remaining = customers
            .ToDictionary(c => c, c => c.burstTime);

        List<CustomerData> notArrived = customers
            .OrderBy(c => c.arrivalTime)
            .ThenBy(c => c.customerName)
            .ToList();

        int time = 0;

        while (remaining.Count > 0)
        {
            while (notArrived.Count > 0 &&
                   notArrived[0].arrivalTime <= time)
            {
                readyQueue.Enqueue(notArrived[0]);
                notArrived.RemoveAt(0);
            }

            if (readyQueue.Count == 0)
            {
                time = notArrived[0].arrivalTime;
                continue;
            }

            CustomerData current = readyQueue.Dequeue();

            int duration = Mathf.Min(
                quantumTime,
                remaining[current]
            );

            AddSegment(result, current.customerName, time, duration);

            time += duration;
            remaining[current] -= duration;

            while (notArrived.Count > 0 &&
                   notArrived[0].arrivalTime <= time)
            {
                readyQueue.Enqueue(notArrived[0]);
                notArrived.RemoveAt(0);
            }

            if (remaining[current] > 0)
                readyQueue.Enqueue(current);
            else
                remaining.Remove(current);
        }
    }

    void AddSegment(
        List<ScheduleSegment> result,
        string customerName,
        int startTime,
        int duration)
    {
        result.Add(new ScheduleSegment
        {
            customerName = customerName,
            startTime = startTime,
            endTime = startTime + duration
        });
    }
}