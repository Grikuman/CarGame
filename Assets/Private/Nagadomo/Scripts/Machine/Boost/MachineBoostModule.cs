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
    public float CoolDownTimer { get; set; }
    public bool IsBoosting { get; set; }

    // �G���W�����W���[��
    private MachineEngineModule _machineEngineModule;
    // �A���e�B���b�g���W���[��
    private MachineUltimateModule _machineUltimateModule;

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
        CurrentGauge = MaxBoostGauge;
    }

    /// <summary> �J�n���� </summary>
    public void Start()
    {
        Debug.Log("Start MachineBoostModule");
        // ���W���[���f�[�^���Z�b�g����
        _vehicleController.ResetSettings<MachineBoostModuleData>();

        // �G���W�����W���[�����擾����
        _machineEngineModule = _vehicleController.Find<MachineEngineModule>();
        // �A���e�B���b�g���W���[�����擾����
        _machineUltimateModule = _vehicleController.Find<MachineUltimateModule>();
    }

    /// <summary> �X�V���� </summary>
    public void UpdateModule()
    {
        Debug.Log("Update MachineBoostModule");

        if (IsBoosting)
        {
            // �Q�[�W������
            CurrentGauge -= GaugeConsumptionRate * Time.deltaTime;

            // �A���e�B���b�g�Q�[�W�𒙂߂�
            //_machineUltimateModule.AddUltimateGauge();

            // �Q�[�W�������Ȃ����ꍇ�u�[�X�g���I������
            if (CurrentGauge <= 0)
            {
                CurrentGauge = 0;
                EndBoost();
                Debug.Log("�u�[�X�g�I��");
            }
        }
        else
        {
            // �N�[���_�E�����Ȃ�񕜂��Ȃ�
            if (CoolDownTimer > 0)
            {
                CoolDownTimer -= Time.deltaTime;
            }
            else
            {
                // �Q�[�W����
                if (CurrentGauge < MaxBoostGauge)
                {
                    CurrentGauge += GaugeRecoveryRate * Time.deltaTime;
                    CurrentGauge = Mathf.Clamp(CurrentGauge, 0, MaxBoostGauge);
                }
            }
        }
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

        BoostMultiplier = data.BoostMultiplier;
        MaxBoostGauge = data.MaxBoostGauge;
        GaugeConsumptionRate = data.GaugeConsumptionRate;
        GaugeRecoveryRate = data.GaugeRecoveryRate;
        BoostCoolDown = data.BoostCooldown;
        CurrentGauge = data.CurrentGauge;
        CoolDownTimer = data.CoolDownTimer;
        IsBoosting = data.IsBoosting;

        // �����̃Q�[�W��ݒ肷��
        CurrentGauge = MaxBoostGauge;
    }

    /// <summary>
    /// �u�[�X�g�����ł��邩�m�F����
    /// </summary>
    public void TryActivateBoost()
    {
        // ��������
        // �u�[�X�g�������łȂ��@���@�Q�[�W�����܂��Ă���@���N�[���^�C�����I�����Ă���ꍇ
        if (!IsBoosting && CurrentGauge > 0 && CoolDownTimer <= 0)
        {
        // �A���e�B���b�g��������ԂłȂ���΃u�[�X�g�𔭓�����
        if (!_machineUltimateModule.IsActiveUltimate())
            {
                ActivateBoost();
            }
        }
        ActivateBoost();
    }

    /// <summary>
    /// �u�[�X�g�𔭓�����
    /// </summary>
    private void ActivateBoost()
    {
        // �}�V���G���W���̃u�[�X�g���͂�ݒ肷��
        _machineEngineModule.InputBoost = BoostMultiplier;
        // �u�[�X�g�������
        IsBoosting = true;
        Debug.Log("�u�[�X�g����");
    }

    /// <summary>
    /// �u�[�X�g���I������
    /// </summary>
    private void EndBoost()
    {
        // �u�[�X�g�̔{�������Z�b�g����
        _machineEngineModule.InputBoost = 1.0f;
        // �u�[�X�g������Ԃ�����
        IsBoosting = false;
        // �u�[�X�g�̃N�[���^�C����ݒ肷��
        CoolDownTimer = BoostCoolDown;
    }

    /// <summary>
    /// �u�[�X�g���L�����ǂ���
    /// </summary>
    /// <returns>�u�[�X�g�̔�����Ԃ�Ԃ�</returns>
    public bool IsActiveBoost()
    {
        return IsBoosting;
    }

    /// <summary>
    /// ���݂̃Q�[�W�������擾����(0�`1)
    /// </summary>
    /// <returns>���K�����ꂽ���݂̃Q�[�W������Ԃ�</returns>
    public float GetBoostGaugeNormalized()
    {
        return CurrentGauge / MaxBoostGauge;
    }
}
