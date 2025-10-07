using UnityEngine;

public class MachineEngine : MonoBehaviour
{
    public float _acceleratorAxis = 0;
    private float _brakeAxis = 0;

    // �h���t�g
    public float _driftFactor = 0.5f;
    public float _brakeDuringTurn = 0.3f;

    public float _sideFriction = 0.9f;
    public float _accelerationRate = 500f; // �͂̑������x


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
       this.ApplyAcceleration();
       //this.ReduceSidewaysVelocity();

        Debug.Log(rb.linearVelocity.magnitude * 3.6f);
    }

    /// <summary>
    /// �����x��ǉ�
    /// </summary>
    private void ApplyAcceleration()
    {
        // throttleInput: 0�`1 �̃A�N�Z������
        float force = _acceleratorAxis * _accelerationRate;
        rb.AddForce(transform.forward * force, ForceMode.Force);
        Debug.Log("force");
    }

    /// <summary>
    /// ���͂̏����A�폜
    /// </summary>
    private void ReduceSidewaysVelocity()
    {
        Vector3 velocity = rb.linearVelocity;

        // �}�V���̉E����
        Vector3 right = transform.right;

        // �������𒊏o
        float sideSpeed = Vector3.Dot(velocity, right);

        // �������������i�܂��͌��炷�j
        Vector3 sideVelocity = right * sideSpeed;

        // ���@B: ���������炷�i���R�Ɋ�������j
        rb.linearVelocity = velocity - sideVelocity * _sideFriction;
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
