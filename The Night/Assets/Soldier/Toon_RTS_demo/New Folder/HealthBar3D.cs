using UnityEngine;

public class HealthBar3D : MonoBehaviour
{
    public Transform fill;               // Obiectul verde care se scalează
    public Vector3 offset = new Vector3(0, 2f, 0); // Poziție deasupra caracterului
    private Transform target;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetHealth(float percent)
    {
        percent = Mathf.Clamp01(percent);
        if (fill != null)
        {
            fill.localScale = new Vector3(percent, 1, 1);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;
        transform.forward = Camera.main.transform.forward;
    }
}
