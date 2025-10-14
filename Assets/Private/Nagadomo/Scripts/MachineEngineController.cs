using UnityEngine;

public class MachineEngineController : MonoBehaviour
{
    [Header("エンジンの基本設定")]
    [SerializeField] private float _maxThrust = 3000.0f;    // 最大推進力
    [SerializeField] private float _maxSpeed = 120.0f;      // 最高速度
    [SerializeField] private AnimationCurve _thrustCurve;   // 速度に応じた推力減衰カーブ

    [Header("抵抗の設定")]
    [SerializeField] private float _dragCoeff = 0.02f;      // 空気抵抗係数
    [SerializeField] private float _brakingDrag = 2.0f;     // ブレーキ時の減速力

    [Header("質量の設定")]
    [SerializeField] private float _mass = 200.0f;          // マシンの重量 [kg]

    [Header("横方向の設定")]
    [SerializeField] private float _lateralThrust = 500.0f; // 左右方向の力の強さ

    public float CurrentSpeed { get; private set; }  // 現在速度 [m/s]
    public float InputThrottle { get; set; } = 0.0f; // アクセル入力(0〜1)
    public float InputBrake { get; set; } = 0.0f;    // ブレーキ入力(0〜1)
    public float InputSteer { get; set; } = 0.0f;    // 左右入力(-1〜1)

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.mass = _mass;
        _rb.linearDamping = 0.0f;
        _rb.angularDamping = 0.5f;
    }

    private void FixedUpdate()
    {
        UpdateEngine();
    }

    private void UpdateEngine()
    {
        // 現在速度
        CurrentSpeed = _rb.linearVelocity.magnitude;
        Debug.Log("現在の速度：" + CurrentSpeed);

        // 推力係数（速度に応じた減衰）
        float speedFactor = Mathf.Clamp01(CurrentSpeed / _maxSpeed);
        float thrustFactor = _thrustCurve.Evaluate(speedFactor);

        // 前方推進力
        float thrustForce = InputThrottle * _maxThrust * thrustFactor;

        // 空気抵抗
        float dragForce = _dragCoeff * CurrentSpeed * CurrentSpeed;

        // ブレーキ力
        float brakeForce = InputBrake * _brakingDrag * _mass;

        // 横方向の力
        Vector3 lateral = transform.right * InputSteer * _lateralThrust;

        // 最終的な力の合成
        Vector3 forward = transform.forward;
        Vector3 force = (forward * thrustForce) - (forward * dragForce) - (forward * brakeForce) + lateral;

        _rb.AddForce(force, ForceMode.Force);
    }
}
