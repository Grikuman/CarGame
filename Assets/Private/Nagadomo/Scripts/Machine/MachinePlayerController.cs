using UnityEngine;
using UnityEngine.InputSystem;

public class MachinePlayerController : MonoBehaviour
{
    private MachineEngineController _machineEngineController;
    private MachineBoostController _machineBoostController;
    private MachineUltimateController _machineUltimateController;
    private VehiclePhysicsModule _vehiclePhysicsModule;
    private InputManager _inputManager;

    private void Start()
    {
        // コンポーネントを取得する
        _machineEngineController = GetComponent<MachineEngineController>();
        _machineBoostController = GetComponent<MachineBoostController>();
        _machineUltimateController = GetComponent<MachineUltimateController>();
        _vehiclePhysicsModule = GetComponent<VehiclePhysicsModule>();

        // インプットマネージャーのインスタンスを取得・初期化
        _inputManager = InputManager.Instance;
        _inputManager.Initialize();
    }

    private void Update()
    {
        // 入力値を更新する
        _inputManager.UpdateDrivingInputAxis();
        // 入力値を取得する
        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        // ハンドルの入力
        _vehiclePhysicsModule._input = input.Handle * 0.45f;
        // アクセルの入力
        _machineEngineController.InputThrottle = input.Accelerator;
        // ブレーキの入力
        _machineEngineController.InputBrake = input.Brake;
        // 見た目用モデルの傾き値の入力
        _machineEngineController.InputSteer = (-input.Handle);
        // ブーストの入力
        if(input.Boost)
        {
            _machineBoostController.TryStartBoost();
        }
        // アルティメットの入力
        if(Input.GetKeyDown(KeyCode.R))
        {
            _machineUltimateController.TryActivateUltimate();
        }
    }
}
