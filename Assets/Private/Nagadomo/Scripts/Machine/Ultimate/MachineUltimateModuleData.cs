using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Ultimate Module Data")]
public class MachineUltimateModuleData : VehicleModuleFactoryBase
{
    [Header("�Q�[�W�ݒ�")]
    [SerializeField] private float _currentGauge = 0.0f;   // ���݂̃A���e�B���b�g�Q�[�W
    [SerializeField] private float _maxUltimateGauge = 100.0f;     // �ő�A���e�B���b�g�Q�[�W
    [SerializeField] private float _gaugeIncrease = 0.01f; // �Q�[�W������

    // �ǂݎ���p
    public float CurrentGauge => _currentGauge;
    public float MaxUltimateGauge => _maxUltimateGauge;
    public float GaugeIncrease => _gaugeIncrease;

    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var machineUltimateModule = new MachineUltimateModule();

        // �����ݒ�
        machineUltimateModule.CurrentGauge = _currentGauge;
        machineUltimateModule.MaxUltimateGauge = _maxUltimateGauge;
        machineUltimateModule.GaugeIncrease = _gaugeIncrease;

        // ����������
        machineUltimateModule.Initialize(vehicleController);

        return machineUltimateModule;
    }

    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineUltimateModuleData> machineBoostModule)
        {
            machineBoostModule.ResetModule(this);
        }
    }
}
