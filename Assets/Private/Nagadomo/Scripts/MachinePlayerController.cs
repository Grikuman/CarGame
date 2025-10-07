using UnityEngine;
using UnityEngine.InputSystem;

public class MachinePlayerController : MonoBehaviour
{
    private MachineEngineController _machineEngineController;
    private VehiclePhysicsModule _vehiclePhysicsModule;
    private InputManager _inputManager;

    private void Start()
    {
        // コンポーネントを取得する
        _machineEngineController = GetComponent<MachineEngineController>();
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

        // ハンドルの更新
        _vehiclePhysicsModule._input = -input.Handle;
        // アクセルの更新
        _machineEngineController.InputThrottle = input.Accelerator;
        Debug.Log(input.Accelerator);
    }
}
