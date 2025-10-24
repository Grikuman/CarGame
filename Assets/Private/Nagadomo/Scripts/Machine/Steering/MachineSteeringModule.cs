using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineSteeringModule : IVehicleModule, IResettableVehicleModule<MachineSteeringModuleData>
{
    // ステアリング入力
    public float InputSteer { get; set; } = 0.0f;

    // 物理挙動制御モジュール
    private VehiclePhysicsModule _vehiclePhysicsModule;

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
    }

    /// <summary> 開始処理 </summary>
    public void Start()
    {
        Debug.Log("Start Machine Engine Module");
        // モジュールデータリセット処理
        _vehicleController.ResetSettings<MachineSteeringModuleData>();

        // 物理挙動制御モジュールを取得する
        _vehiclePhysicsModule = _vehicleController.Find<VehiclePhysicsModule>();
    }

    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    {
        Debug.Log("Update MachineSteeringModule");
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        Debug.Log("FixedUpdate MachineSteeringModule");

        // 法線の向きを取得する
        _groundUp = _vehiclePhysicsModule.GroundNormal;

        // 地面法線を軸に回転
        Quaternion turnRot = Quaternion.AngleAxis(InputSteer * 50.0f * Time.fixedDeltaTime,_groundUp);

        // 現在の回転に加算する
        _vehicleController.transform.rotation = turnRot * _vehicleController.transform.rotation;
    }

    // リセット時の処理
    public void ResetModule(MachineSteeringModuleData data)
    {
        Debug.Log("Reset MachineSteeringData");
    }
}
