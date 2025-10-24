using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Boost Module Data")]
public class MachineBoostModuleData : VehicleModuleFactoryBase
{
    [Header("�u�[�X�g�ݒ�")]
    [SerializeField] private float _boostMultiplier = 1.5f;       // �{��
    [SerializeField] private float _maxBoostGauge = 100.0f;       // �ő�Q�[�W��
    [SerializeField] private float _gaugeConsumptionRate = 20.0f; // 1�b����������
    [SerializeField] private float _gaugeRecoveryRate = 20.0f;    // 1�b������񕜗�
    [SerializeField] private float _boostCooldown = 3.0f;         // �u�[�X�g��̃N�[���_�E��

    [SerializeField] private float _currentGauge;
    [SerializeField] private bool _isBoosting = true;

    // �ǂݎ���p
    public float BoostMultiplier => _boostMultiplier;
    public float MaxBoostGauge => _maxBoostGauge;
    public float GaugeConsumptionRate => _gaugeConsumptionRate;
    public float GaugeRecoveryRate => _gaugeRecoveryRate;
    public float BoostCooldown => _boostCooldown;

    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var machineBoostModule = new MachineBoostModule();

        // �����ݒ�
        machineBoostModule.BoostMultiplier = _boostMultiplier;
        machineBoostModule.MaxBoostGauge = _maxBoostGauge;
        machineBoostModule.GaugeConsumptionRate = _gaugeConsumptionRate;
        machineBoostModule.GaugeRecoveryRate = _gaugeRecoveryRate;
        machineBoostModule.BoostCoolDown = _boostCooldown;

        // ����������
        machineBoostModule.Initialize(vehicleController);

        return machineBoostModule;
    }

    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineBoostModuleData> machineBoostModule)
        {
            machineBoostModule.ResetModule(this);
        }
    }
}
