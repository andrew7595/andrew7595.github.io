using UnityEngine;

public class CreatureMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float rotateSpeed = 3f;
    public float changeTargetInterval = 4f;

    [Header("Swim Mode")]
    public bool isSwimming = true;
    public float swimHeightMin = -1f;
    public float swimHeightMax = 3f;

    private Vector3 targetPosition;
    private float timer;

    void Start()
    {
        SetNewTarget();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeTargetInterval)
        {
            SetNewTarget();
            timer = 0f;
        }

        MoveToTarget();
    }

    void SetNewTarget()
    {
        Vector3 randomDir = Random.insideUnitSphere * 5f;

        if (!isSwimming)
            randomDir.y = 0f;

        targetPosition = transform.position + randomDir;

        if (isSwimming)
        {
            targetPosition.y = Random.Range(swimHeightMin, swimHeightMax);
        }
    }

    void MoveToTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotateSpeed * Time.deltaTime);
        }

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}