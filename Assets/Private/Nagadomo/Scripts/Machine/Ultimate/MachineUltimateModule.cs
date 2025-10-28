using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineUltimateModule : IVehicleModule, IResettableVehicleModule<MachineUltimateModuleData>
{
    public float CurrentGauge { get; set; }
    public float MaxUltimateGauge { get;set; }
    public float GaugeIncrease { get; set; }
    public bool InputUltimate {  get; set; }

    // ���݂̃A���e�B���b�g
    private IUltimate _currentUltimate;

    // �G���W�����W���[��
    private MachineEngineModule _machineEngineModule;

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
        CurrentGauge = MaxUltimateGauge;
        // �Ƃ肠����Boost Ultimate��ݒ肵�Ă���
        _currentUltimate = new Ultimate_Boost();
    }

    /// <summary> �J�n���� </summary>
    public void Start()
    {
        // ���W���[���f�[�^���Z�b�g����
        _vehicleController.ResetSettings<MachineBoostModuleData>();

        // �G���W�����W���[�����擾����
        _machineEngineModule = _vehicleController.Find<MachineEngineModule>();
    }

    /// <summary> �X�V���� </summary>
    public void UpdateModule()
    {
        // ���͎擾
        InputUltimate = _vehicleController.Ultimate;

        if(InputUltimate)
        {
            // �A���e�B���b�g�𔭓��ł��邩�m�F����
            this.TryActivateUltimate();
        }

        // �A���e�B���b�g�������Ȃ�X�V
        if (_currentUltimate.IsActive())
        {
            _currentUltimate.Update();
            // �A���e�B���b�g���I��������
            if (_currentUltimate.IsEnd())
            {
                Debug.Log("�A���e�B���b�g���I��");
                // �A���e�B���b�g�I���������s��
                _currentUltimate.End();
                // �Q�[�W�����Z�b�g����
                CurrentGauge = 0.0f;
            }
        }

        // �Q�[�W�l��␳����
        if (CurrentGauge >= MaxUltimateGauge)
        {
            CurrentGauge = MaxUltimateGauge;
        }

        // ���͂̏�����
        InputUltimate = false;
        _vehicleController.Ultimate = InputUltimate;
    }
    /// <summary> �����v�Z�X�V���� </summary>
    public void FixedUpdateModule()
    {
        
    }

    // ���Z�b�g���̏���
    public void ResetModule(MachineUltimateModuleData data)
    {
        CurrentGauge = data.CurrentGauge;
        MaxUltimateGauge = data.MaxUltimateGauge;
        GaugeIncrease = data.GaugeIncrease;

        // �����̃Q�[�W��ݒ肷��
        CurrentGauge = MaxUltimateGauge;
    }

    /// <summary>
    /// �A���e�B���b�g�𔭓��ł��邩�m�F����
    /// </summary>
    public void TryActivateUltimate()
    {
        // �Q�[�W�����܂��Ă���@���@�A���e�B���b�g����������Ă��Ȃ��ꍇ
        if (CurrentGauge >= MaxUltimateGauge && !_currentUltimate.IsActive())
        {
            _currentUltimate.Activate(_machineEngineModule);
            Debug.Log("�A���e�B���b�g�𔭓�");
        }
    }

    /// <summary>
    /// �A���e�B���b�g�Q�[�W�𑝉�������
    /// </summary>
    public void AddUltimateGauge()
    {
        CurrentGauge += GaugeIncrease;
    }

    /// <summary>
    /// �A���e�B���b�g�̎�ނ�ݒ肷��
    /// </summary>
    /// <param name="ultimate">�}�V���ɐݒ肷��A���e�B���b�g�̎��</param>
    public void SetUltimate(IUltimate ultimate)
    {
        _currentUltimate = ultimate;
    }

    /// <summary>
    /// �A���e�B���b�g���������Ă��邩�ǂ���
    /// </summary>
    /// <returns>�A���e�B���b�g�̔������</returns>
    public bool IsActiveUltimate()
    {
        if (_currentUltimate.IsActive())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ���݂̃Q�[�W�������擾����(0�`1)
    /// </summary>
    /// <returns>���K�����ꂽ���݂̃Q�[�W������Ԃ�</returns>
    public float GetUltimateGaugeNormalized()
    {
        return CurrentGauge / MaxUltimateGauge;
    }
}
