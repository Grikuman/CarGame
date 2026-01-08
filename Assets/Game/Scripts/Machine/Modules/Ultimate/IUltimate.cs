public interface IUltimate
{
    void Activate(MachineEngineModule machineEngineModule); // 発動処理
    void Update(); // 毎フレーム更新
    void End(); // 終了処理
    bool IsEnd();
    bool IsActive();
}
