using UnityEngine;
using UnityEngine.InputSystem;

public class MachineDriver : MonoBehaviour
{
    private MachineEngine _machineEngine;

    private InputManager _inputManager;

    private void Start()
    {
        _machineEngine = GetComponent<MachineEngine>();
        _inputManager = InputManager.Instance;
        _inputManager.Initialize();
    }

    private void Update()
    {
        // マシンドライバー入力値を更新する
        _inputManager.UpdateDrivingInputAxis();

        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        // ハンドルの更新
       
        // アクセルの更新
        _machineEngine._acceleratorAxis = input.Accelerator;

    }
}
