
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CustomerQueueManager : MonoBehaviour
{
    [Header("Queue Settings")]
    public Transform[] queuePoints;
    public Transform[] pathPoints;
    public CustomerMovement[] customers;

    [Header("Service Points")]
    public Transform cashierPoint;
    public Transform exitPoint;

    [Header("Scheduling")]
    public SchedulingManager schedulingManager;
    public float spawnDelay = 1f;
    public float timeUnitDuration = 1f;

    private bool[] activeCustomers;

    void Start()
    {
        StartCoroutine(StartSimulation());
    }

    IEnumerator StartSimulation()
    {
        if (SimulationData.Instance == null)
        {
            Debug.LogError("SimulationData tidak ditemukan!");
            yield break;
        }

        if (schedulingManager == null)
            schedulingManager = FindFirstObjectByType<SchedulingManager>();

        if (schedulingManager == null)
        {
            Debug.LogError("SchedulingManager tidak ditemukan!");
            yield break;
        }

        if (cashierPoint == null || exitPoint == null)
        {
            Debug.LogError("CashierPoint atau ExitPoint belum diatur!");
            yield break;
        }

        CustomerData[] data = SimulationData.Instance.customers;
        activeCustomers = new bool[customers.Length];

        for (int i = 0; i < customers.Length; i++)
        {
            if (customers[i] == null)
                continue;

            string customerName = "P" + (i + 1);

            foreach (CustomerData customerData in data)
            {
                if (customerData.customerName == customerName)
                {
                    activeCustomers[i] = true;
                    customers[i].gameObject.SetActive(true);
                    break;
                }
            }

            if (!activeCustomers[i])
                customers[i].gameObject.SetActive(false);
        }

        yield return StartCoroutine(MoveCustomersToQueue());

        List<SchedulingManager.ScheduleSegment> schedule =
            schedulingManager.GenerateSchedule(
                data,
                SimulationData.Instance.selectedMethod,
                SimulationData.Instance.quantumTime
            );

        if (schedule.Count == 0)
        {
            Debug.LogError("Jadwal simulasi kosong!");
            yield break;
        }

        yield return StartCoroutine(ExecuteSchedule(schedule));
    }

    IEnumerator MoveCustomersToQueue()
    {
        for (int i = 0; i < customers.Length; i++)
        {
            if (!activeCustomers[i])
                continue;

            if (i >= queuePoints.Length || queuePoints[i] == null)
            {
                Debug.LogError(
                    "Queue Point untuk P" + (i + 1) + " belum diatur!"
                );
                yield break;
            }

            customers[i].SetPath(pathPoints, queuePoints[i]);

            yield return new WaitForSeconds(spawnDelay);
        }

        bool moving;

        do
        {
            moving = false;

            for (int i = 0; i < customers.Length; i++)
            {
                if (activeCustomers[i] && customers[i].IsMoving)
                {
                    moving = true;
                    break;
                }
            }

            yield return null;

        } while (moving);
    }

    IEnumerator ExecuteSchedule(
        List<SchedulingManager.ScheduleSegment> schedule)
    {
        Dictionary<string, int> lastSegment =
            new Dictionary<string, int>();

        for (int i = 0; i < schedule.Count; i++)
        {
            lastSegment[schedule[i].customerName] = i;
        }

        int previousEndTime = 0;

        for (int i = 0; i < schedule.Count; i++)
        {
            SchedulingManager.ScheduleSegment segment = schedule[i];

            int idleTime = segment.startTime - previousEndTime;

            if (idleTime > 0)
            {
                yield return new WaitForSeconds(
                    idleTime * timeUnitDuration
                );
            }

            int customerIndex =
                int.Parse(segment.customerName.Substring(1)) - 1;

            if (customerIndex < 0 || customerIndex >= customers.Length)
                continue;

            CustomerMovement customer = customers[customerIndex];

            if (customer == null)
                continue;

            Debug.Log(segment.customerName + " menuju kasir.");

            customer.MoveTo(cashierPoint);

            yield return new WaitUntil(() => !customer.IsMoving);

            Debug.Log(segment.customerName + " sedang dilayani.");

            float serviceDuration =
                (segment.endTime - segment.startTime) * timeUnitDuration;

            yield return new WaitForSeconds(serviceDuration);

            previousEndTime = segment.endTime;

            if (lastSegment[segment.customerName] == i)
            {
                Debug.Log(segment.customerName + " selesai dilayani.");

                customer.MoveTo(exitPoint);

                yield return new WaitUntil(() => !customer.IsMoving);

                customer.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log(segment.customerName + " kembali mengantre.");

                customer.MoveTo(queuePoints[customerIndex]);

                yield return new WaitUntil(() => !customer.IsMoving);
            }
        }

        Debug.Log("Semua pelanggan selesai dilayani!");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("HasilCpu");
    }
}
