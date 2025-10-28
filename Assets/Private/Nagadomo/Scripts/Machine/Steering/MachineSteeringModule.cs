using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineSteeringModule : IVehicleModule, IResettableVehicleModule<MachineSteeringModuleData>
{
    public Transform VisualModel { get; set; }
    public float VisualYawAngle { get; set; }
    public float VisualRollAngle { get; set; }
    public float VisualRotateSpeed { get; set; }

    private Quaternion _defaultRotation; // 見た目用の初期姿勢

    // ステアリング入力
    public float InputSteer { get; set; } = 0.0f;

    // 物理挙動制御モジュール
    private VehiclePhysicsModule _vehiclePhysicsModule;

    // エンジンモジュール
    private MachineEngineModule _machineEngineModule;

    // 地面法線
    private Vector3 _groundUp = Vector3.zero;

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

        // 見た目用モデルの初期化処理
        InitVisualModel();
    }

    /// <summary> 開始処理 </summary>
    public void Start()
    {
        // モジュールデータリセット処理
        _vehicleController.ResetSettings<MachineSteeringModuleData>();
        // 物理挙動制御モジュールを取得する
        _vehiclePhysicsModule = _vehicleController.Find<VehiclePhysicsModule>();
        // エンジンモジュールを取得する
        _machineEngineModule = _vehicleController.Find<MachineEngineModule>();
    }

    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    {
        // 入力取得
        InputSteer = _vehicleController.Steering;
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        // 法線の向きを取得する
        _groundUp = _vehiclePhysicsModule.GroundNormal;
        // 地面法線を軸に回転
        Quaternion turnRot = Quaternion.AngleAxis(InputSteer * 50.0f * Time.fixedDeltaTime,_groundUp);
        // 現在の回転に加算する
        _vehicleController.transform.rotation = turnRot * _vehicleController.transform.rotation;

        // 見た目用モデルを傾ける
        UpdateVisualRotation();
    }

    // リセット時の処理
    public void ResetModule(MachineSteeringModuleData data)
    {
        VisualYawAngle = data.VisualYawAngle;
        VisualYawAngle = data.VisualRollAngle;
        VisualRotateSpeed = data.VisualRotateSpeed;

        // 見た目用モデルの初期化処理
        InitVisualModel();
    }

    /// <summary>
    /// 見た目用モデルの初期化処理
    /// </summary>
    private void InitVisualModel()
    {
        if (VisualModel == null)
        {
            VisualModel = _vehicleController.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "VisualModel");

            if (VisualModel == null)
            {
                Debug.LogWarning("マシンのVisualModelが見つかりません");
                return;
            }
        }

        // 初期角度を保存する
        _defaultRotation = VisualModel.localRotation;
    }

    /// <summary>
    /// 入力値と速度に応じてマシンの見た目用モデルを傾ける
    /// </summary>
    private void UpdateVisualRotation()
    {
        if (VisualModel == null) return;

        float currentSpeed = _machineEngineModule.CurrentSpeed;
        float maxSpeed = _machineEngineModule.MaxSpeed;

        // 現在速度を0〜1の範囲に正規化する
        float speedFactor = Mathf.Clamp01(currentSpeed / maxSpeed);
        // 入力と速度に応じて傾きを決定(速いほど強く傾く)
        float targetYaw = -InputSteer * VisualYawAngle * speedFactor;
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
