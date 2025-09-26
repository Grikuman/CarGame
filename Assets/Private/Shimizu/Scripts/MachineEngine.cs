using UnityEngine;

public class MachineEngine : MonoBehaviour
{
    [SerializeField] private float maxTorque = 1200f;   // �ő�G���W���g���N [Nm]
    [SerializeField] private float wheelRadius = 0.3f;  // �ԗ֔��a [m]
    [SerializeField] private AnimationCurve torqueCurve; // ���x�ɉ������g���N����

    [SerializeField] private float dragCoeff = 0.5f;     // ��C��R�W��
    [SerializeField] private float rollingResistance = 30f;

    [SerializeField] private float mass = 150f;          // �J�[�g�d��

    [SerializeField] private float speed;  // ���ݑ��x m/sf

    Rigidbody rb;

    public float inputKey { get; set; } = 0.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateEngine(inputKey);

        Vector3 forward = transform.forward;
        rb.AddForce(transform.forward * speed, ForceMode.Acceleration);

        Debug.Log(speed);
    }

    public float ComputeDriveForce(float throttleInput)
    {
        // ���x�ɉ������g���N�W���i0�`1�j
        float speedFactor = torqueCurve.Evaluate(speed);

        // �G���W���g���N
        float engineTorque = throttleInput * maxTorque * speedFactor;

        // �쓮�́i�^�C�����a�Ŋ���j
        float driveForce = engineTorque / wheelRadius;

        return driveForce;
    }

    public float ComputeResistance()
    {
        float drag = dragCoeff * speed * speed;
        float roll = rollingResistance;
        return drag + roll;
    }

    public void UpdateEngine(float throttleInput)
    {
        float driveForce = ComputeDriveForce(throttleInput);
        float resist = ComputeResistance();

        // �����x
        float ax = (driveForce - resist) / mass;

        // ���x�X�V
        speed += ax * Time.fixedDeltaTime;
        speed = Mathf.Max(0f, speed); // ��ނȂ�
    }
}
