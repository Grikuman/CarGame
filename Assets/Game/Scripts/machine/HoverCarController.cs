using UnityEngine;

public class HoverCarController : MonoBehaviour
{
    [Header("�z���ݒ�")]
    public float targetDistance = 2.0f;
    public float attractionForce = 50f;
    public float damping = 5.0f;

    [Header("�ړ��ݒ�")]
    public float moveSpeed = 15f;
    public float turnSpeed = 120f;
    public float stopFriction = 8f;

    private Rigidbody rb;
    private Vector3 surfaceNormal = Vector3.up;

    // ���͒l�i���̃X�N���v�g������������j
    [HideInInspector] public float forwardInput;
    [HideInInspector] public float turnInput;
    [HideInInspector] public float boostMultiplier = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 2f;
        rb.angularDamping = 4f;
    }

    void FixedUpdate()
    {
        HandleSurfaceAttraction();
        HandleMovement();
    }

    void HandleSurfaceAttraction()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, targetDistance * 2f))
        {
            surfaceNormal = hit.normal;
            float distanceError = hit.distance - targetDistance;
            float normalSpeed = Vector3.Dot(rb.linearVelocity, surfaceNormal);
            float force = (-distanceError * attractionForce) - (normalSpeed * damping);

            rb.AddForce(surfaceNormal * force, ForceMode.Acceleration);

            // �ڒn�ʂɉ�]�␳
            Vector3 forwardProjected = Vector3.ProjectOnPlane(transform.forward, surfaceNormal).normalized;
            if (forwardProjected.sqrMagnitude < 0.001f)
                forwardProjected = Vector3.Cross(transform.right, surfaceNormal).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(forwardProjected, surfaceNormal);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }
    }

    void HandleMovement()
    {
        // ��]
        Quaternion turnRot = Quaternion.AngleAxis(turnInput * turnSpeed * Time.fixedDeltaTime, surfaceNormal);
        rb.MoveRotation(turnRot * rb.rotation);

        // �O�i�����i�ڒn�ʂɉ����j
        Vector3 forwardDir = Vector3.Cross(transform.right, surfaceNormal).normalized;

        // ���͒l & �u�[�X�g�𔽉f
        Vector3 targetVelocity = forwardDir * (forwardInput * moveSpeed * boostMultiplier);
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * stopFriction);
    }
}
