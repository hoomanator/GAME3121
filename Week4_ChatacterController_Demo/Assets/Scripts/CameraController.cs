using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform followTarget, lookTarget;

    public float FollowSpeed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, followTarget.position, FollowSpeed * Time.deltaTime);
        }

        if (lookTarget != null)
        {
            transform.LookAt(lookTarget);
        }
    }
}
