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
        // �}�V���h���C�o�[���͒l���X�V����
        _inputManager.UpdateDrivingInputAxis();

        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        // �n���h���̍X�V
        _vehiclePhysicsModule._input = input.Handle;
        // �A�N�Z���̍X�V
        _machineEngine.inputKey      = input.Accelerator;

    }
}
