public interface IUltimate
{
    void Activate(MachineEngineModule machineEngineModule); // ��������
    void Update(); // ���t���[���X�V
    void End(); // �I������
    bool IsEnd();
    bool IsActive();
}
