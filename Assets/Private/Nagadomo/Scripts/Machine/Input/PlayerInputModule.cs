using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputModule : IVehicleModule, IResettableVehicleModule<PlayerInputModuleData>
{
    private MachineEngineModule _machineEngineModule;
    private MachineSteeringModule _machineSteeringModule;
    private MachineBoostModule _machineBoostModule;
    private MachineUltimateModule _machineUltimateModule;
    private InputManager _inputManager;

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
    }

    /// <summary> 開始処理 </summary>
    public void Start()
    {
        Debug.Log("Start Player Input Module");
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
        Debug.Log("Update Player Input Module");

        // 入力値を更新する
        _inputManager.UpdateDrivingInputAxis();
        // 入力値を取得する
        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        // ハンドルの入力
        _machineSteeringModule.InputSteer = input.Handle;
        // アクセルの入力
        _machineEngineModule.InputThrottle = input.Accelerator;
        // ブレーキの入力
        _machineEngineModule.InputBrake = input.Brake;
        // 見た目用モデルの傾き値の入力
        _machineEngineModule.InputSteer = (-input.Handle);

        // ブースト入力
        if(input.Boost)
        {
            _machineBoostModule.TryActivateBoost();
        }
        // アルティメット入力
        if(Input.GetKeyDown(KeyCode.R))
        {
            _machineUltimateModule.TryActivateUltimate();
        }
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        
    }

    // リセット時の処理
    public void ResetModule(PlayerInputModuleData data)
    {
        Debug.Log("Reset Player Input Data");
    }
}
