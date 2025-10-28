using UnityEngine;

public class MachineEngineController : MonoBehaviour
{
    [Header("エンジンの基本設定")]
    [SerializeField] private float _maxThrust = 8000.0f;  // 最大推進力
    [SerializeField] private float _maxSpeed = 120.0f;    // 最高速度
    [SerializeField] private AnimationCurve _thrustCurve; // 速度に応じた推進力

    [Header("抵抗の設定")]
    [SerializeField] private float _dragCoeff = 0.02f;  // 空気抵抗係数
    [SerializeField] private float _brakingDrag = 2.0f; // ブレーキの強さ

    [Header("質量の設定")]
    [SerializeField] private float _mass = 200.0f; // マシンの質量

    [Header("横方向の設定")]
    [SerializeField] private float _lateralGrip = 10.0f; // 横滑りの抑制する強さ

    [Header("見た目用設定")]
    [SerializeField] private Transform _visualModel;          // マシンのモデル参照(子オブジェクト)
    [SerializeField] private float _visualYawAngle = 10.0f;   // 回転時のモデルの最大傾き角度(Yaw)
    [SerializeField] private float _visualRollAngle = 25.0f;  // 回転時のモデルの最大傾き角度(Roll)
    [SerializeField] private float _visualRotateSpeed = 5.0f; // 回転補間速度

    public float CurrentSpeed { get; private set; }   // 現在の速度
    public float InputThrottle { get; set; } = 0.0f;  // アクセル入力
    public float InputBrake { get; set; } = 0.0f;     // ブレーキ入力
    public float InputSteer { get; set; } = 0.0f;     // ステアリング入力
    public float InputBoost { get; set;} = 1.0f;      // ブーストの入力

    private Rigidbody _rb; // RigitBody
    private Quaternion _defaultRotation; // 見た目用の初期姿勢

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = _mass;
        _rb.linearDamping = 0.0f;
        _rb.angularDamping = 0.5f;

        // 見た目用モデルの初期化処理
        InitVisualModel();
    }

    private void FixedUpdate()
    {
        // エンジンの推進力・抵抗・ブレーキを計算する
        UpdateEngine();
        // 横滑りを抑える処理
        Grip();
        // 入力値と速度に応じてマシンの見た目用モデルを傾ける
        UpdateVisualRotation();
    }

    // 見た目用モデルの初期化処理
    private void InitVisualModel()
    {
        if (_visualModel == null)
        {
            Debug.LogWarning("VisualModelが設定されていません");
        }
        else
        {
            // 初期角度を保存する
            _defaultRotation = _visualModel.localRotation;
        }
    }

    /// <summary>
    /// エンジンの推進力・抵抗・ブレーキを計算する
    /// </summary>
    private void UpdateEngine()
    {
        // 現在の速度を取得する
        CurrentSpeed = _rb.linearVelocity.magnitude;
        // 速度比0～1に正規化する
        float speedFactor = Mathf.Clamp01(CurrentSpeed / _maxSpeed);
        // カーブで推力減衰を取得する
        float thrustFactor = _thrustCurve.Evaluate(speedFactor);

        float thrustForce = InputThrottle * _maxThrust * thrustFactor * InputBoost; // 推力
        float dragForce = _dragCoeff * CurrentSpeed * CurrentSpeed;    // 空気抵抗
        float brakeForce = InputBrake * _brakingDrag * _mass; // ブレーキ力

        // 最終の力を計算する
        Vector3 forward = transform.forward;
        Vector3 force = (forward * thrustForce) - (forward * dragForce) - (forward * brakeForce);
        // 前方方向に力を加える
        _rb.AddForce(force, ForceMode.Force);
    }

    /// <summary>
    /// 横滑りを抑える処理
    /// </summary>
    private void Grip()
    {
        // 現在の速度
        Vector3 velocity = _rb.linearVelocity;
        // 前方方向の速度成分
        Vector3 forward = transform.forward;
        Vector3 forwardVel = Vector3.Project(velocity, forward);
        // 横方向の速度成分
        Vector3 lateralVel = velocity - forwardVel;
        // 横滑りを抑える力を加える
        _rb.AddForce(-lateralVel * _lateralGrip, ForceMode.Acceleration);
    }

    /// <summary>
    /// 入力値と速度に応じてマシンの見た目用モデルを傾ける
    /// </summary>
    private void UpdateVisualRotation()
    {
        if (_visualModel == null) return;

        // 現在速度を0〜1の範囲に正規化する
        float speedFactor = Mathf.Clamp01(CurrentSpeed / _maxSpeed);
        // 入力と速度に応じて傾きを決定(速いほど強く傾く)
        float targetYaw = InputSteer * _visualYawAngle * speedFactor;
        float targetRoll = InputSteer * _visualRollAngle * speedFactor;
        // 入力がない時はゆっくりと元の角度に戻す
        Quaternion targetRot = _defaultRotation * Quaternion.Euler(0, targetYaw, -targetRoll);

        // スムーズに補間させる
        _visualModel.localRotation = Quaternion.Slerp(
            _visualModel.localRotation,
            targetRot,
            Time.fixedDeltaTime * _visualRotateSpeed
        );
    }
}
