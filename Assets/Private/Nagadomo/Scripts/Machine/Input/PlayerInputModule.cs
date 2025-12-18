using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputModule : IVehicleModule, IResettableVehicleModule<PlayerInputModuleData>
{
    private MachineEngineModule _machineEngineModule;
    private MachineSteeringModule _machineSteeringModule;
    private MachineBoostModule _machineBoostModule;
    private MachineUltimateModule _machineUltimateModule;
    private InputManager _inputManager;
    private RaceManager _raceManager;

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

        // インプットマネージャーのインスタンスを取得・初期化
        _inputManager = InputManager.Instance;
        _inputManager.Initialize();

        // RaceManager を取得
        _raceManager = Object.FindFirstObjectByType<RaceManager>();
    }

    /// <summary> 開始処理 </summary>
    public void Start()
    {
        
        // エンジンモジュールを取得する
        _machineEngineModule = _vehicleController.Find<MachineEngineModule>();
        // ステアリングモジュールを取得する
        _machineSteeringModule = _vehicleController.Find<MachineSteeringModule>();
        // ブーストモジュールを取得する
        _machineBoostModule = _vehicleController.Find<MachineBoostModule>();
        // アルティメットモジュールを取得する
        _machineUltimateModule = _vehicleController.Find<MachineUltimateModule>();

        // モジュールデータリセット処理
        _vehicleController.ResetSettings<PlayerInputModuleData>();
    }

    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    {
        // レース中でなければ入力を遮断
        if (_raceManager == null ||
            _raceManager.CurrentState != RaceManager.RaceState.Racing ||
            !_isActive)
        {
            _vehicleController.Steering = 0f;
            _vehicleController.Accelerator = 0f;
            _vehicleController.brake = 0f;
            _vehicleController.boost = false;
            _vehicleController.Ultimate = false;
            return;
        }

        // 入力値を更新する
        _inputManager.UpdateDrivingInputAxis();
        // 入力値を取得する
        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        // ハンドルの入力
        _vehicleController.Steering = input.Handle;
        // アクセルの入力
        _vehicleController.Accelerator = input.Accelerator;
        // ブレーキの入力
        _vehicleController.brake = input.Brake;
        // ブースト入力
        _vehicleController.boost = input.Boost;
        // アルティメット入力
        _vehicleController.Ultimate = input.Ultimate;
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        
    }

    // リセット時の処理
    public void ResetModule(PlayerInputModuleData data)
    {
        
    }
}
