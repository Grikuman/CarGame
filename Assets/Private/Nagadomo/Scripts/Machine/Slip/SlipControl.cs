using UnityEngine;

public class SlipControl
{
    private readonly Rigidbody _rb;

    // ������̗}�����鋭��
    private readonly float _lateralGrip;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="rb">����Ώۂ�Rigidbody</param>
    /// <param name="lateralGrip">���̃O���b�v�W��</param>
    public SlipControl(Rigidbody rb, float lateralGrip)
    {
        _rb = rb;
        _lateralGrip = lateralGrip;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    public void UpdateSlip()
    {
        // �������}���鏈��
        this.ApplyGrip();
    }

    /// <summary>
    /// �������}���鏈��
    /// </summary>
    public void ApplyGrip()
    {
        // ���݂̑��x���擾
        Vector3 velocity = _rb.linearVelocity;
        // �O�������x�N�g��
        Vector3 forward = _rb.transform.forward;

        // �O�������Ɖ����������𕪉�
        Vector3 forwardVel = Vector3.Project(velocity, forward);
        Vector3 lateralVel = velocity - forwardVel;

        // ���������x��ł������͂�������
        _rb.AddForce(-lateralVel * _lateralGrip, ForceMode.Acceleration);
    }
}
