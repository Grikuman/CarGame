using UnityEngine;

public class MachineBoostController : MonoBehaviour
{
    [Header("�u�[�X�g�ݒ�")]
    [SerializeField] private float _boostMultiplier = 1.5f;       // �{��
    [SerializeField] private float _maxBoostGauge = 100.0f;       // �ő�Q�[�W��
    [SerializeField] private float _gaugeConsumptionRate = 20.0f; // 1�b����������
    [SerializeField] private float _gaugeRecoveryRate = 20.0f;    // 1�b������񕜗�
    [SerializeField] private float _boostCooldown = 3.0f;         // �u�[�X�g��̃N�[���_�E��

    [SerializeField] private float _currentGauge;         // ���݂̃Q�[�W
    [SerializeField] private float _cooldownTimer = 0.0f; // �N�[���_�E���c�莞��
    [SerializeField] private bool _isBoosting = false;    // �u�[�X�g�̔�����ԊǗ�

    // �R���|�[�l���g
    private MachineEngineController _machineEngineController;

    void Start()
    {
        _machineEngineController = GetComponent<MachineEngineController>();
        _currentGauge = _maxBoostGauge; // �����̓Q�[�W�𒙂߂Ă���
    }

    void Update()
    {
        if (_isBoosting)
        {
            // �Q�[�W������
            _currentGauge -= _gaugeConsumptionRate * Time.deltaTime;

            // �Q�[�W�������Ȃ����ꍇ�u�[�X�g���I������
            if (_currentGauge <= 0)
            {
                _currentGauge = 0;
                EndBoost();
                Debug.Log("�u�[�X�g�I��");
            }
        }
        else
        {
            // �N�[���_�E�����Ȃ�񕜂��Ȃ�
            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;
            }
            else
            {
                // �Q�[�W����
                if (_currentGauge < _maxBoostGauge)
                {
                    _currentGauge += _gaugeRecoveryRate * Time.deltaTime;
                    _currentGauge = Mathf.Clamp(_currentGauge, 0, _maxBoostGauge);
                }
            }
        }
    }

    /// <summary>
    /// �u�[�X�g�����ł��邩�m�F����
    /// </summary>
    public void TryStartBoost()
    {
        // ��������
        // �u�[�X�g�������łȂ��@���@�Q�[�W�����܂��Ă���@���N�[���^�C�����I�����Ă���ꍇ
        if (!_isBoosting && _currentGauge > 0 && _cooldownTimer <= 0)
        {
            StartBoost();
        }
    }

    /// <summary>
    /// �u�[�X�g�𔭓�����
    /// </summary>
    private void StartBoost()
    {
        // �}�V���G���W���̃u�[�X�g���͂�ݒ肷��
        _machineEngineController.InputBoost = _boostMultiplier;
        // �u�[�X�g�������
        _isBoosting = true;
        Debug.Log("�u�[�X�g����");
    }

    /// <summary>
    /// �u�[�X�g���I������
    /// </summary>
    private void EndBoost()
    {
        // �u�[�X�g�̔{�������Z�b�g����
        _machineEngineController.InputBoost = 1.0f;
        // �u�[�X�g������Ԃ�����
        _isBoosting = false;
        // �u�[�X�g�̃N�[���^�C����ݒ肷��
        _cooldownTimer = _boostCooldown;
    }

    /// <summary>
    /// �u�[�X�g���L�����ǂ���
    /// </summary>
    /// <returns>�u�[�X�g�̔�����Ԃ�Ԃ�</returns>
    public bool IsActiveBoost()
    {
        return _isBoosting;
    }

    /// <summary>
    /// ���݂̃Q�[�W�������擾����(0�`1)
    /// </summary>
    /// <returns>���K�����ꂽ���݂̃Q�[�W������Ԃ�</returns>
    public float GetBoostGaugeNormalized()
    {
        return _currentGauge / _maxBoostGauge;
    }
}
