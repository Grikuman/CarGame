// �}�V���̑ҋ@���
using UnityEngine;

public class MachineWaitingState : IMachineState
{
    public void Initialize(MachineStateController machine)
    {
        Debug.Log("�ҋ@��ԁF�J�n����");
    }

    public void Update(MachineStateController machine)
    {
        // �ҋ@���̓}�V���ւ̓��͂��󂯕t���Ȃ�
    }

    public void Finalize(MachineStateController machine)
    {
        Debug.Log("�ҋ@��ԁF�I������");
    }
}