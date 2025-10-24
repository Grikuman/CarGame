using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineSteeringModule : IVehicleModule, IResettableVehicleModule<MachineSteeringModuleData>
{
    // �X�e�A�����O����
    public float InputSteer { get; set; } = 0.0f;

    // �����������䃂�W���[��
    private VehiclePhysicsModule _vehiclePhysicsModule;

    // �n�ʖ@��
    private Vector3 _groundUp = Vector3.zero;

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

    /// <summary> �J�n���� </summary>
    public void Start()
    {
        Debug.Log("Start Machine Engine Module");
        // ���W���[���f�[�^���Z�b�g����
        _vehicleController.ResetSettings<MachineSteeringModuleData>();

        // �����������䃂�W���[�����擾����
        _vehiclePhysicsModule = _vehicleController.Find<VehiclePhysicsModule>();
    }

    /// <summary> �X�V���� </summary>
    public void UpdateModule()
    {
        Debug.Log("Update MachineSteeringModule");
    }
    /// <summary> �����v�Z�X�V���� </summary>
    public void FixedUpdateModule()
    {
        Debug.Log("FixedUpdate MachineSteeringModule");

        // �@���̌������擾����
        _groundUp = _vehiclePhysicsModule.GroundNormal;

        // �n�ʖ@�������ɉ�]
        Quaternion turnRot = Quaternion.AngleAxis(InputSteer * 50.0f * Time.fixedDeltaTime,_groundUp);

        // ���݂̉�]�ɉ��Z����
        _vehicleController.transform.rotation = turnRot * _vehicleController.transform.rotation;
    }

    // ���Z�b�g���̏���
    public void ResetModule(MachineSteeringModuleData data)
    {
        Debug.Log("Reset MachineSteeringData");
    }
}
