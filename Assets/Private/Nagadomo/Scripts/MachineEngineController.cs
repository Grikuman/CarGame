using UnityEngine;

public class MachineEngineController : MonoBehaviour
{
    [Header("エンジンの基本設定")]
    [SerializeField] private float _maxThrust = 3000.0f;    // 最大推進力
    [SerializeField] private float _maxSpeed = 120.0f;      // 最高速度
    [SerializeField] private AnimationCurve _thrustCurve; // 速度に応じた推力減衰カーブ

    [Header("抵抗の設定")]
    [SerializeField] private float _dragCoeff = 0.02f;    // 空気抵抗係数
    [SerializeField] private float _brakingDrag = 2.0f;   // ブレーキ時の減速力

    [Header("質量の設定")]
    [SerializeField] private float _mass = 200.0f;          // マシンの重量 [kg]

    public float CurrentSpeed { get; private set; }  // 現在速度 [m/s]
    public float InputThrottle { get; set; } = 0.0f; // アクセル入力(0〜1)
    public float InputBrake { get; set; } = 0.0f;    // ブレーキ入力(0〜1)

    private Rigidbody _rb;

    private void Start()
    {
        // コンポーネントを取得する
        _rb = GetComponent<Rigidbody>();

        _rb.mass = _mass;          // 質量を設定する
        _rb.linearDamping = 0.0f;    // 線形抵抗をゼロにして、自前で制御する
        _rb.angularDamping = 0.5f; // 回転に対しては少し抵抗を残しておく
    }

    private void FixedUpdate()
    {
        // エンジンの更新を行う
        UpdateEngine();
    }

    /// <summary>
    /// エンジンの更新を行う
    /// </summary>
    private void UpdateEngine()
    {
        // 現在速度を取得する
        CurrentSpeed = _rb.linearVelocity.magnitude;

        // 現在速度に応じてスロットル係数を計算する(0～1)
        float speedFactor = Mathf.Clamp01(CurrentSpeed / _maxSpeed);
        float thrustFactor = _thrustCurve.Evaluate(speedFactor);

        // 前方向の推進力
        float thrustForce = InputThrottle * _maxThrust * thrustFactor;

        // 空気抵抗 (速度の2乗に比例)
        float dragForce = _dragCoeff * CurrentSpeed * CurrentSpeed;

        // ブレーキ力（入力に応じて質量ベースで抵抗）
        float brakeForce = InputBrake * _brakingDrag * _mass;

        // 前方方向ベクトルに力を加える
        Vector3 forward = transform.forward;
        Vector3 force = (forward * thrustForce) - (forward * dragForce) - (forward * brakeForce);

        // Rigidbodyに力を加える
        _rb.AddForce(force, ForceMode.Force);
    }
}
