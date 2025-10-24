using UnityEngine;

public class MachineUltimateController : MonoBehaviour
{
    [Header("�Q�[�W�ݒ�")]
    [SerializeField] private float _currentGauge = 0.0f;   // ���݂̃A���e�B���b�g�Q�[�W
    [SerializeField] private float _maxUltimateGauge = 100.0f;     // �ő�A���e�B���b�g�Q�[�W
    [SerializeField] private float _gaugeIncrease = 0.01f; // �Q�[�W������

    // ���݂̃A���e�B���b�g
    private IUltimate _currentUltimate;

    private MachineEngineModule _machineEngineModule;

    void Start()
    {
        // �R���|�[�l���g���擾����
        _machineEngineModule = GetComponent<MachineEngineModule>();

        // �Ƃ肠����Boost Ultimate��ݒ肵�Ă���
        _currentUltimate = new Ultimate_Boost();
    }

    void Update()
    {
        // �A���e�B���b�g�������Ȃ�X�V
        if (_currentUltimate.IsActive())
        {
            _currentUltimate.Update();
            // �A���e�B���b�g���I��������
            if (_currentUltimate.IsEnd())
            {
                // �A���e�B���b�g�I���������s��
                _currentUltimate.End();
                // �Q�[�W�����Z�b�g����
                _currentGauge = 0.0f;
            }
        }

        // �Q�[�W�l��␳����
        if (_currentGauge >= _maxUltimateGauge)
        {
            _currentGauge = _maxUltimateGauge;
        }
    }

    /// <summary>
    /// �A���e�B���b�g�𔭓��ł��邩�m�F����
    /// </summary>
    public void TryActivateUltimate()
    {
        // �Q�[�W�����܂��Ă���@���@�A���e�B���b�g����������Ă��Ȃ��ꍇ
        if (_currentGauge >= _maxUltimateGauge && !_currentUltimate.IsActive())
        {
            _currentUltimate.Activate(_machineEngineModule);
        }
    }

    /// <summary>
    /// �A���e�B���b�g�Q�[�W�𑝉�������
    /// </summary>
    public void AddUltimateGauge()
    {
        _currentGauge += _gaugeIncrease;
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
        return _currentGauge / _maxUltimateGauge;
    }
}
