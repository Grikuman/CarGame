public interface IMachineState
{
    void Initialize(MachineStateController machine); // �J�n���̏���
    void Update(MachineStateController machine);     // ���t���[������
    void Finalize(MachineStateController machine);   // �I�����̏���
}
