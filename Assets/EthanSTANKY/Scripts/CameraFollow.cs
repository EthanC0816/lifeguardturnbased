using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = Vector3.zero;
    public float followSpeed = 5f;
    public float offsetLerpSpeed = 3f;
    public float rotationLerpSpeed = 3f;

    public MainMenu menuScript;

    private Vector3 currentOffset;
    
    void Start()
    {
        currentOffset = offset;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        if (menuScript.gameStarted == true)
        {
            currentOffset = Vector3.Lerp(currentOffset, offset, offsetLerpSpeed * Time.deltaTime);

            Vector3 desiredPos = target.position + currentOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

            Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationLerpSpeed * Time.deltaTime);
        }
        else return;

    }
    public void SetTarget(Transform newTarget, Vector3 newOffset)
    {
        target = newTarget;
        offset = newOffset; 
    }
}
