using UnityEngine;
using UnityEngine.InputSystem;

public class MachinePlayerController : MonoBehaviour
{
    private MachineEngineController _machineEngineController;
    private VehiclePhysicsModule _vehiclePhysicsModule;
    private InputManager _inputManager;

    private void Start()
    {
        // �R���|�[�l���g���擾����
        _machineEngineController = GetComponent<MachineEngineController>();
        _vehiclePhysicsModule = GetComponent<VehiclePhysicsModule>();

        // �C���v�b�g�}�l�[�W���[�̃C���X�^���X���擾�E������
        _inputManager = InputManager.Instance;
        _inputManager.Initialize();
    }

    private void Update()
    {
        // ���͒l���X�V����
        _inputManager.UpdateDrivingInputAxis();
        // ���͒l���擾����
        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        // �n���h���̍X�V
        _vehiclePhysicsModule._input = -input.Handle;
        // �A�N�Z���̍X�V
        _machineEngineController.InputThrottle = input.Accelerator;
        Debug.Log(input.Accelerator);
    }
}
