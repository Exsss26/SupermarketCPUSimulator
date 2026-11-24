
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Transform[] pathPoints;
    private Transform finalTarget;
    private int currentPoint = 0;

    private Animator animator;

    public bool IsMoving { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPath(Transform[] newPath, Transform destination)
    {
        pathPoints = newPath;
        finalTarget = destination;
        currentPoint = 0;

        IsMoving = true;

        if (animator != null)
            animator.SetBool("IsWalking", true);
    }

    public void MoveTo(Transform destination)
    {
        pathPoints = null;
        finalTarget = destination;
        currentPoint = 0;

        IsMoving = destination != null;

        if (animator != null)
            animator.SetBool("IsWalking", IsMoving);
    }

    void Update()
    {
        if (!IsMoving || finalTarget == null)
            return;

        Transform target;

        if (pathPoints != null && currentPoint < pathPoints.Length)
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
            if (pathPoints != null && currentPoint < pathPoints.Length)
            {
                currentPoint++;
            }
            else
            {
                IsMoving = false;
                pathPoints = null;
                finalTarget = null;

                if (animator != null)
                    animator.SetBool("IsWalking", false);
            }
        }
    }
}
