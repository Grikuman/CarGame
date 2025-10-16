using UnityEngine;
using UnityEngine.InputSystem;

public class TEST_Controller : MonoBehaviour
{
    private TEST_1 _machineEngineController;
    private VehiclePhysicsModule _vehiclePhysicsModule;
    private InputManager _inputManager;

    private void Start()
    {
        // �R���|�[�l���g���擾����
        _machineEngineController = GetComponent<TEST_1>();
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
        _vehiclePhysicsModule._input = input.Handle * 0.3f;
        // �A�N�Z���̍X�V
        _machineEngineController.InputThrottle = input.Accelerator;
        // �u���[�L�̍X�V
        _machineEngineController.InputBrake = input.Brake;

        // �e�X�g�p
        float inputHorizontal = Input.GetAxis("Horizontal");
        // ������̒ǉ�
        _machineEngineController.InputSteer = (inputHorizontal);
    }
}
