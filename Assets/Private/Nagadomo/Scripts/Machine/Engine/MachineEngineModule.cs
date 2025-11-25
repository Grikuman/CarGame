using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineEngineModule : IVehicleModule, IResettableVehicleModule<MachineEngineModuleData>
{

    public float MaxThrust { get; set; }
    public float MaxSpeed { get; set; }
    public AnimationCurve ThrustCurve { get; set; }

    public float DragCoeff { get; set; }
    public float BrakingDrag { get; set; }
    public float Mass { get; set; }

    public float CurrentSpeed { get; private set; }  // 現在の速度
    public float InputThrottle { get; set; } = 0.0f; // アクセル入力
    public float InputBrake { get; set; } = 0.0f;    // ブレーキ入力
    public float InputSteer { get; set; } = 0.0f;    // ステアリング入力
    public float BoostMultiplier { get; set; } = 1.0f; // ブースト倍率
    public float ExternalBoostMultiplier { get; set; } = 1.0f; // 外部のブースト倍率

    // リジッドボディー
    private Rigidbody _rb;

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

        // リジッドボディーの設定
        _rb = _vehicleController.gameObject.GetComponent<Rigidbody>();
        _rb.mass = Mass;
        _rb.linearDamping = 0.0f;
        _rb.angularDamping = 0.5f;
    }

    /// <summary> 開始処理 </summary>
    public void Start()
    {
        // モジュールデータリセット処理
        _vehicleController.ResetSettings<MachineEngineModuleData>();
    }

    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    {
        // 入力取得
        InputThrottle = _vehicleController.Accelerator;
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        // エンジンの推進力・抵抗・ブレーキを計算する
        UpdateEngine();
    }

    // リセット時の処理
    public void ResetModule(MachineEngineModuleData data)
    {
        MaxThrust = data.MaxThrust;
        MaxSpeed = data.MaxSpeed;
        ThrustCurve = data.ThrustCurve;
        DragCoeff = data.DragCoeff;
        BrakingDrag = data.BrakingDrag;
        Mass = data.Mass;
    }

    /// <summary>
    /// エンジンの推進力・抵抗・ブレーキを計算する
    /// </summary>
    private void UpdateEngine()
    {
        Debug.Log("エンジンのスロットル値は：" + InputThrottle + "です");
        // 現在の速度を取得する
        CurrentSpeed = _rb.linearVelocity.magnitude;
        // 速度比0～1に正規化する
        float speedFactor = Mathf.Clamp01(CurrentSpeed / MaxSpeed);
        // カーブで推力減衰を取得する
        float thrustFactor = ThrustCurve.Evaluate(speedFactor);

        float thrustForce = InputThrottle * MaxThrust * thrustFactor * BoostMultiplier * ExternalBoostMultiplier; // 推力
        float dragForce = DragCoeff * CurrentSpeed * CurrentSpeed; // 空気抵抗
        float brakeForce = InputBrake * BrakingDrag * Mass; // ブレーキ力

        // 最終の力を計算する
        Vector3 forward = _rb.transform.forward;
        Vector3 force = (forward * thrustForce) - (forward * dragForce) - (forward * brakeForce);
        // 前方方向に力を加える
        _rb.AddForce(force, ForceMode.Force);
    }
}
