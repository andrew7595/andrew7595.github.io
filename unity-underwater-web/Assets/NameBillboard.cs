using UnityEngine;
using TMPro;

public class NameBillboard : MonoBehaviour
{
    public TextMeshPro text;

    void LateUpdate()
    {
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }

    public void SetName(string username)
    {
        text.text = username;
    }
}