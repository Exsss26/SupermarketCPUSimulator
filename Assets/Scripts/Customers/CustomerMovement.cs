
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Transform[] pathPoints;
    private Transform finalTarget;
    private int currentPoint = 0;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPath(Transform[] newPath, Transform destination)
    {
        pathPoints = newPath;
        finalTarget = destination;
        currentPoint = 0;

        if (animator != null)
            animator.SetBool("IsWalking", true);
    }

    void Update()
    {
        if (pathPoints == null || finalTarget == null)
            return;

        Transform target;

        if (currentPoint < pathPoints.Length)
            target = pathPoints[currentPoint];
        else
            target = finalTarget;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            if (currentPoint < pathPoints.Length)
            {
                currentPoint++;
            }
            else
            {
                pathPoints = null;
                finalTarget = null;

                if (animator != null)
                    animator.SetBool("IsWalking", false);
            }
        }
    }
}
