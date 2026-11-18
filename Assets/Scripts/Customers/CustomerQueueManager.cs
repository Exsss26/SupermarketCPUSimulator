
using UnityEngine;
using System.Collections;

public class CustomerQueueManager : MonoBehaviour
{
    public Transform[] queuePoints;
    public Transform[] pathPoints;
    public CustomerMovement[] customers;

    public float spawnDelay = 1f;

    void Start()
    {
        StartCoroutine(StartQueue());
    }

    IEnumerator StartQueue()
    {
        for (int i = 0; i < customers.Length; i++)
        {
            if (customers[i] != null && i < queuePoints.Length)
            {
                customers[i].SetPath(pathPoints, queuePoints[i]);
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}
