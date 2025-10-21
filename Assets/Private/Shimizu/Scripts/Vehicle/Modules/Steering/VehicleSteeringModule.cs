using UnityEngine;

public class VehicleSteeringModule : IVehicleModule , IResettableVehicleModule<SteeringSettings>
{
    private bool _isActive = true;
    private VehicleController _vehicleController;

    /// <summary> �A�N�e�B�u��Ԃ�ݒ� </summary>
    public void SetActive(bool value) => _isActive = value;
    /// <summary> �A�N�e�B�u��Ԃ��擾 </summary>
    public bool GetIsActive() => _isActive;

    /// <summary> ���������� </summary>
    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;
    }
    /// <summary> �X�V���� </summary>
    public void UpdateModule()
    {
        Debug.Log("Update Steering Module");
    }
    /// <summary> �����v�Z�X�V���� </summary>
    public void FixedUpdateModule()
    {

    }

    // ���Z�b�g���̏���
    public void ResetModule(SteeringSettings settings)
    {
        Debug.Log("Reset Steering Settings");
    }
}
