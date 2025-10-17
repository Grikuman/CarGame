using UnityEngine;

public class MachineStateController : MonoBehaviour
{
    // ���݂̃X�e�[�g
    private IMachineState _currentState;

    // �}�V���C���v�b�g
    public IMachineInput MachineInput { get; private set; }

    void Start()
    {
        // �Ƃ肠�����v���C���[�̓��͂ɂ��Ă����@���MachinAIController�Ȃ񂩂�t�������Ă��悳����
        MachineInput = GetComponent<MachinePlayerInput>();
        // �����X�e�[�g�ɐ؂�ւ�
        ChangeState(new MachineWaitingState());
    }

    void Update()
    {
        // ���݂̃X�e�[�g���X�V����
        _currentState?.Update(this);


        // �f�o�b�O�p
        if(Input.GetKeyDown(KeyCode.W))
        {
            ChangeState(new MachineWaitingState());
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeState(new MachinePlayingState());
        }
    }

    /// <summary>
    /// �}�V���̃X�e�[�g��ύX����
    /// </summary>
    /// <param name="newState">�ύX��̃X�e�[�g</param>
    public void ChangeState(IMachineState newState)
    {
        _currentState?.Finalize(this);
        _currentState = newState;
        _currentState.Initialize(this);
    }
}
