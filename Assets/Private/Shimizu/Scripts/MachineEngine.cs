using UnityEngine;

public class MachineEngine : MonoBehaviour
{
    public float _acceleratorAxis = 0;
    private float _brakeAxis = 0;

    // ドリフト
    public float _driftFactor = 0.5f;
    public float _brakeDuringTurn = 0.3f;

    public float _sideFriction = 0.9f;
    public float _accelerationRate = 500f; // 力の増加速度


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
       this.ApplyAcceleration();
       //this.ReduceSidewaysVelocity();

        Debug.Log(rb.linearVelocity.magnitude * 3.6f);
    }

    /// <summary>
    /// 加速度を追加
    /// </summary>
    private void ApplyAcceleration()
    {
        // throttleInput: 0～1 のアクセル入力
        float force = _acceleratorAxis * _accelerationRate;
        rb.AddForce(transform.forward * force, ForceMode.Force);
        Debug.Log("force");
    }

    /// <summary>
    /// 横力の除去、削除
    /// </summary>
    private void ReduceSidewaysVelocity()
    {
        Vector3 velocity = rb.linearVelocity;

        // マシンの右方向
        Vector3 right = transform.right;

        // 横成分を抽出
        float sideSpeed = Vector3.Dot(velocity, right);

        // 横成分を除去（または減らす）
        Vector3 sideVelocity = right * sideSpeed;

        // 方法B: 少しずつ減らす（自然に慣性制御）
        rb.linearVelocity = velocity - sideVelocity * _sideFriction;
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
