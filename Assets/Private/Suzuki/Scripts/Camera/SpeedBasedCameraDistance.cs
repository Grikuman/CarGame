using UnityEngine;
using Cinemachine;

public class SpeedBasedCameraDistance : MonoBehaviour
{
    public Rigidbody targetRigidbody;
    public CinemachineVirtualCamera vcam;
    public float baseDistance = -5f;
    public float maxDistance = -8f;
    public float speedToMax = 200f; // 200�ōő勗���ɓ��B
    public float smooth = 3f;

    private CinemachineTransposer transposer;
    private Vector3 currentOffset;

    void Start()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        currentOffset = transposer.m_FollowOffset;
    }

    void LateUpdate()
    {
        if (targetRigidbody == null || transposer == null) return;

        float speed = targetRigidbody.linearVelocity.magnitude;
        float t = Mathf.Clamp01(speed / speedToMax);
        float targetZ = Mathf.Lerp(baseDistance, maxDistance, t);
        currentOffset.z = Mathf.Lerp(currentOffset.z, targetZ, Time.deltaTime * smooth);
        transposer.m_FollowOffset = currentOffset;
    }
}
