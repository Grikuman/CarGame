using UnityEngine;
using UnityEngine.InputSystem;

public class MachinePlayerInput : MonoBehaviour , IMachineInput
{
    private MachineEngineController _machineEngineController;
    private MachineBoostController _machineBoostController;
    private MachineUltimateController _machineUltimateController;
    private InputManager _inputManager;

    private void Start()
    {
        // �R���|�[�l���g���擾����
        _machineEngineController = GetComponent<MachineEngineController>();
        _machineBoostController = GetComponent<MachineBoostController>();
        _machineUltimateController = GetComponent<MachineUltimateController>();
       

        // �C���v�b�g�}�l�[�W���[�̃C���X�^���X���擾�E������
        _inputManager = InputManager.Instance;
        _inputManager.Initialize();
    }

    public void InputUpdate()
    {
        // ���͒l���X�V����
        _inputManager.UpdateDrivingInputAxis();
        // ���͒l���擾����
        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        // �n���h���̓���
        //_vehiclePhysicsModule._input = input.Handle * 0.45f;
        // �A�N�Z���̓���
        _machineEngineController.InputThrottle = input.Accelerator;
        // �u���[�L�̓���
        _machineEngineController.InputBrake = input.Brake;
        // �����ڗp���f���̌X���l�̓���
        _machineEngineController.InputSteer = (-input.Handle);
        // �u�[�X�g�̓���
        if(input.Boost)
        {
            _machineBoostController.TryActivateBoost();
        }
        // �A���e�B���b�g�̓���
        if(Input.GetKeyDown(KeyCode.R))
        {
            _machineUltimateController.TryActivateUltimate();
        }
    }
}
