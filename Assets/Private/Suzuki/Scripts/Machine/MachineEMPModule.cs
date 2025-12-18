using UnityEngine;

public class MachineEMPModule :
    IVehicleModule,
    IResettableVehicleModule<MachineEMPModuleData>
{
    public float EMPDuration { get; set; }

    private bool _isActive = true;

    private VehicleController _vehicleController;
    private MachineBoostModule _boost;

    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;
    }

    public void Start()
    {
        _boost = _vehicleController.Find<MachineBoostModule>();
    }

    public void SetActive(bool value) => _isActive = value;
    public bool GetIsActive() => _isActive;

    public void UpdateModule() { }
    public void FixedUpdateModule() { }

    public void ResetModule(MachineEMPModuleData data)
    {
        EMPDuration = data.EMPDuration;
    }

    // 敵用：ブースト減少
    public void ApplyEnemyEMP(float amount)
    {
        if (!_isActive) return;
        _boost?.DecreaseGauge(amount);
    }

    // 自分用：ブースト回復
    public void ApplySelfHeal(float amount)
    {
        if (!_isActive) return;
        _boost?.IncreaseGauge(amount);
    }
}
