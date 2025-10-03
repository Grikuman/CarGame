using UnityEngine;
using UnityEngine.InputSystem;

public class MachineDriver : MonoBehaviour
{
    private MachineEngine _machineEngine;
    private VehiclePhysicsModule _vehiclePhysicsModule;
    private InputManager _inputManager;

    private void Start()
    {
        _machineEngine = GetComponent<MachineEngine>();
        _vehiclePhysicsModule = GetComponent<VehiclePhysicsModule>();
        _inputManager = InputManager.Instance;
        _inputManager.Init();
    }

    private void Update()
    {
        // マシンドライバー入力値を更新する
        _inputManager.UpdateDrivingInputAxis();

        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        // ハンドルの更新
        _vehiclePhysicsModule._input = input.Handle;
        // アクセルの更新
        _machineEngine.inputKey      = input.Accelerator;

    }
}
