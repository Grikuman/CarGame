using UnityEngine;

public class VehicleEngineModule : IVehicleModule , IResettableVehicleModule<EngineSettings>
{

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
    }
    /// <summary> �X�V���� </summary>
    public void UpdateModule()
    { 
        Debug.Log("Update Engine Module");
    }
    /// <summary> �����v�Z�X�V���� </summary>
    public void FixedUpdateModule()
    { 
        
    }

    // ���Z�b�g���̏���
    public void ResetModule(EngineSettings settings) 
    {
        Debug.Log("Reset Engine Settings");
    }
}