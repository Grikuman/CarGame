using UnityEngine;

public class MachineEngine : MonoBehaviour
{
    [SerializeField] private float maxTorque = 1200f;   // 最大エンジントルク [Nm]
    [SerializeField] private float wheelRadius = 0.3f;  // 車輪半径 [m]
    [SerializeField] private AnimationCurve torqueCurve; // 速度に応じたトルク特性

    [SerializeField] private float dragCoeff = 0.5f;     // 空気抵抗係数
    [SerializeField] private float rollingResistance = 30f;

    [SerializeField] private float mass = 150f;          // カート重量

    [SerializeField] private float speed;  // 現在速度 m/sf

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
        // 速度に応じたトルク係数（0～1）
        float speedFactor = torqueCurve.Evaluate(speed);

        // エンジントルク
        float engineTorque = throttleInput * maxTorque * speedFactor;

        // 駆動力（タイヤ半径で割る）
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

        // 加速度
        float ax = (driveForce - resist) / mass;

        // 速度更新
        speed += ax * Time.fixedDeltaTime;
        speed = Mathf.Max(0f, speed); // 後退なし
    }
}
