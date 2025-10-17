public interface IMachineState
{
    void Initialize(MachineStateController machine); // 開始時の処理
    void Update(MachineStateController machine);     // 毎フレーム処理
    void Finalize(MachineStateController machine);   // 終了時の処理
}
