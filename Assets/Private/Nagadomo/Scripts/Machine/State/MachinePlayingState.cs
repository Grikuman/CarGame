// �}�V���̑ҋ@���
using UnityEngine;

public class MachinePlayingState : IMachineState
{
    public void Initialize(MachineStateController machine)
    {
        Debug.Log("�v���C��ԁF�J�n����");
    }

    public void Update(MachineStateController machine)
    {
        // �}�V���ւ̓��͂��󂯕t����
        machine.MachineInput.InputUpdate();
    }

    public void Finalize(MachineStateController machine)
    {
        Debug.Log("�v���C��ԁF�I������");
    }
}