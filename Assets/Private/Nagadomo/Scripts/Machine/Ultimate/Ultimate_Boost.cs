using UnityEngine;

public class Ultimate_Boost : IUltimate
{
    private float _ultimateTime = 3.0f;   // �A���e�B���b�g�̌��ʎ���
    private float _boostMultiplier = 2.5f;// �u�[�X�g�̔{��
    private float _timer;                 // �^�C�}�[
    private bool _isActive;               // ������Ԃ̊Ǘ�
    private bool _isEnd = false;          // ���ʎ��ԏI���ʒm
    private MachineEngineModule _machineEngineModule;

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="machineEngineController">�}�V���G���W���R���g���[���[</param>
    public void Activate(MachineEngineModule machineEngineModule)
    {
        // �}�V���G���W���R���g���[���[��ݒ肷��
        _machineEngineModule = machineEngineModule;
        // �u�[�X�g�̔{����ݒ肷��
        _machineEngineModule.InputBoost = _boostMultiplier;
        // �A���e�B���b�g�̌��ʎ��Ԃ�ݒ肷��
        _timer = _ultimateTime;
        // �A���e�B���b�g�𔭓���Ԃɂ���
        _isActive = true;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    public void Update()
    {
        if (!_isActive) return;

        _timer -= Time.deltaTime;
        // �A���e�B���b�g���Ԃ��I��������
        if (_timer <= 0.0f)
        {
            // �I��������
            _isEnd = true;
        }
    }

    /// <summary>
    /// �I������
    /// </summary>
    public void End()
    {
        // �u�[�X�g�̔{�������Z�b�g����
        _machineEngineModule.InputBoost = 1.0f;
        // ������Ԃ���������
        _isActive = false;
        // �I����Ԃ���������
        _isEnd = false;
    }

    /// <summary>
    /// �A���e�B���b�g�̌��ʂ��I���������ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsEnd() { return _isEnd; }

    /// <summary>
    /// �A���e�B���b�g�����������ǂ���
    /// </summary>
    /// <returns>�������</returns>
    public bool IsActive() { return _isActive; }
}
