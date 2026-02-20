using UnityEngine;
using System.Collections;

public class CameraFocusController : MonoBehaviour
{
    [Header("Focus Settings")]
    public float focusDistance = 4f;
    public float heightOffset = 1.5f;
    public float focusTime = 2f;
    public float moveSpeed = 8f;
    public float returnSpeed = 12f;

    private Vector3 originalPos;
    private Quaternion originalRot;
    private Coroutine focusRoutine;

    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    public void FocusOn(Transform target)
    {
        if (focusRoutine != null)
            StopCoroutine(focusRoutine);

        focusRoutine = StartCoroutine(FocusSequence(target));
    }

    IEnumerator FocusSequence(Transform target)
    {
        Vector3 focusPos =
            target.position +
            (-target.forward * focusDistance) +
            Vector3.up * heightOffset;

        Quaternion focusRot = Quaternion.LookRotation(
            target.position - focusPos
        );

        // 移動到聚焦位置
        while (Vector3.Distance(transform.position, focusPos) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, focusPos, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, focusRot, Time.deltaTime * moveSpeed);
            yield return null;
        }

        // 停留
        yield return new WaitForSeconds(focusTime);

        // 快速回到原位
        while (Vector3.Distance(transform.position, originalPos) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPos, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRot, Time.deltaTime * returnSpeed);
            yield return null;
        }

        transform.position = originalPos;
        transform.rotation = originalRot;
    }
}