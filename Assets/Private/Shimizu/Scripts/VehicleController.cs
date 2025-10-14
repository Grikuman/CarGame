using UnityEngine;

public class VehicleController : MonoBehaviour
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        this.DrawRay();
    }

    private void FixedUpdate()
    {
        Vector3 forward = transform.forward;

        _rb.linearVelocity = forward;
    }

    private void DrawRay()
    {
        Vector3 velocity = _rb.linearVelocity;

        // �e�����̃x�N�g��
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 steeringDirection = Quaternion.Euler(0, 10, 0) * forward;

        // �e�����ւ̑��x�����iDot�ρj
        float forwardMag  = Vector3.Dot(velocity, forward);
        float rightMag    = Vector3.Dot(velocity, right);
        float steeringMag = Vector3.Dot(velocity, steeringDirection);

        // �X�P�[�����O�i���o��̒����j
        float scale = 0.5f;

        // Debug�\���i�����͊e�����̐����ɔ��j
        Debug.DrawRay(transform.position, forward * forwardMag * scale, Color.green);    // �O����
        Debug.DrawRay(transform.position, right * rightMag * scale, Color.red);          // ������
        Debug.DrawRay(transform.position, steeringDirection * steeringMag * scale, Color.magenta); // �X�e�A����
        Debug.DrawRay(transform.position, velocity * scale, Color.blue);                 // ���ۂ̑��x�x�N�g��
    }


}
