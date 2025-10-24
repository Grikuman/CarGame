using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineBoostModule : IVehicleModule, IResettableVehicleModule<MachineBoostModuleData>
{
    public float BoostMultiplier { get; set; }
    public float MaxBoostGauge { get; set; }
    public float GaugeConsumptionRate { get; set; }
    public float GaugeRecoveryRate { get; set; }
    public float BoostCoolDown { get; set; }
    public float CurrentGauge { get; set; }
    public float CoolDownTimer { get; private set; }
    public bool IsBoosting { get; set; }

    // �G���W�����W���[��
    private MachineEngineModule _machineEngineModule;
    // �A���e�B���b�g���W���[��
    //private MachineUltimateModule _machineUltimateModule;

    private bool _isActive = true;
    private VehicleController _vehicleController = null;

    /// <summary> �A�N�e�B�u��Ԃ�ݒ� </summary>
    public void SetActive(bool value) => _isActive = value;
    /// <summary> �A�N�e�B�u��Ԃ��擾 </summary>
    public bool GetIsActive() => _isActive;

    /// <summary> ���������� </summary>
    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;

        // �����̃Q�[�W��ݒ肷��
    }

    /// <summary> �J�n���� </summary>
    public void Start()
    {
        Debug.Log("Start MachineBoostModule");
        // ���W���[���f�[�^���Z�b�g����
        _vehicleController.ResetSettings<MachineBoostModuleData>();
    }

    /// <summary> �X�V���� </summary>
    public void UpdateModule()
    {
        Debug.Log("Update MachineBoostModule");
    }
    /// <summary> �����v�Z�X�V���� </summary>
    public void FixedUpdateModule()
    {
        Debug.Log("FixedUpdate MachineBoostModule");
    }

    // ���Z�b�g���̏���
    public void ResetModule(MachineBoostModuleData data)
    {
        Debug.Log("Reset MachineBoostData");
    }
}
