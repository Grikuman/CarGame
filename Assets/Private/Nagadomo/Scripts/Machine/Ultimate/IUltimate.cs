public interface IUltimate
{
    void Activate(MachineEngineController machineEngineController); // ��������
    void Update(); // ���t���[���X�V
    void End(); // �I������
    bool IsEnd();
    bool IsActive();
}
