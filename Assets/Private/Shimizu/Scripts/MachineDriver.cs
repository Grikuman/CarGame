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
        // �}�V���h���C�o�[���͒l���X�V����
        _inputManager.UpdateDrivingInputAxis();

        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        // �n���h���̍X�V
       
        // �A�N�Z���̍X�V
        _machineEngine._acceleratorAxis = input.Accelerator;

    }
}
