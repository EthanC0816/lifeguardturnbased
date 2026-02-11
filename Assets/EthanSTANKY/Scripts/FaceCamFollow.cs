using UnityEngine;

public class FaceCamFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 10f;
    public float rotateSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

     
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);

        
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotateSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
