using UnityEngine;
using UnityEngine.UIElements;

public class MachineEngineModule : IVehicleModule, IResettableVehicleModule<MachineEngineSettings>
{

    public float MaxThrust { get; set; }
    public float MaxSpeed { get; set; }
    public AnimationCurve ThrustCurve { get; set; }

    public float DragCoeff { get; set; }
    public float BrakingDrag { get; set; }
    public float Mass { get; set; }
    public float LateralGrip { get; set; }

    public Transform VisualModel { get; set; }
    public float VisualYawAngle { get; set; }
    public float VisualRollAngle { get; set; }
    public float VisualRotateSpeed { get; set; }

    public float CurrentSpeed { get; private set; }   // 現在の速度
    public float InputThrottle { get; set; } = 0.0f;  // アクセル入力
    public float InputBrake { get; set; } = 0.0f;     // ブレーキ入力
    public float InputSteer { get; set; } = 0.0f;     // ステアリング入力
    public float InputBoost { get; set; } = 1.0f;      // ブーストの入力

    private Rigidbody _rb; // RigitBody
    private Quaternion _defaultRotation; // 見た目用の初期姿勢

    private bool _isActive = true;
    private VehicleController _vehicleController = null;

    /// <summary> アクティブ状態を設定 </summary>
    public void SetActive(bool value) => _isActive = value;
    /// <summary> アクティブ状態を取得 </summary>
    public bool GetIsActive() => _isActive;

    /// <summary> 初期化処理 </summary>
    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;

        _rb = _vehicleController.gameObject.GetComponent<Rigidbody>();
        _rb.mass = Mass;
        _rb.linearDamping = 0.0f;
        _rb.angularDamping = 0.5f;

        // 見た目用モデルの初期化処理
        InitVisualModel();
    }
    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    {
        Debug.Log("Update Machine Engine Module");
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        // エンジンの推進力・抵抗・ブレーキを計算する
        UpdateEngine();
        // 横滑りを抑える処理
        Grip();
        // 入力値と速度に応じてマシンの見た目用モデルを傾ける
        UpdateVisualRotation();
    }

    // リセット時の処理
    public void ResetModule(MachineEngineSettings settings)
    {
        Debug.Log("Reset Machine Engine Settings");
    }

    // 見た目用モデルの初期化処理
    private void InitVisualModel()
    {
        if (VisualModel == null)
        {
            Debug.LogWarning("マシンのVisualModelが設定されていません");
        }
        else
        {
            // 初期角度を保存する
            _defaultRotation = VisualModel.localRotation;
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
        float speedFactor = Mathf.Clamp01(CurrentSpeed / MaxSpeed);
        // カーブで推力減衰を取得する
        float thrustFactor = ThrustCurve.Evaluate(speedFactor);

        float thrustForce = InputThrottle * MaxThrust * thrustFactor * InputBoost; // 推力
        float dragForce = DragCoeff * CurrentSpeed * CurrentSpeed;    // 空気抵抗
        float brakeForce = InputBrake * BrakingDrag * Mass;         // ブレーキ力

        // 最終の力を計算する
        Vector3 forward = _rb.transform.forward;
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
        Vector3 forward = _rb.transform.forward;
        Vector3 forwardVel = Vector3.Project(velocity, forward);
        // 横方向の速度成分
        Vector3 lateralVel = velocity - forwardVel;
        // 横滑りを抑える力を加える
        _rb.AddForce(-lateralVel * LateralGrip, ForceMode.Acceleration);
    }

    /// <summary>
    /// 入力値と速度に応じてマシンの見た目用モデルを傾ける
    /// </summary>
    private void UpdateVisualRotation()
    {
        if (VisualModel == null) return;

        // 現在速度を0〜1の範囲に正規化する
        float speedFactor = Mathf.Clamp01(CurrentSpeed / MaxSpeed);
        // 入力と速度に応じて傾きを決定(速いほど強く傾く)
        float targetYaw = InputSteer * VisualYawAngle * speedFactor;
        float targetRoll = InputSteer * VisualRollAngle * speedFactor;
        // 入力がない時はゆっくりと元の角度に戻す
        Quaternion targetRot = _defaultRotation * Quaternion.Euler(0, targetYaw, -targetRoll);

        // スムーズに補間させる
        VisualModel.localRotation = Quaternion.Slerp(
            VisualModel.localRotation,
            targetRot,
            Time.fixedDeltaTime * VisualRotateSpeed
        );
    }
}
