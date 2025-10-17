// マシンの待機状態
using UnityEngine;

public class MachineWaitingState : IMachineState
{
    public void Initialize(MachineStateController machine)
    {
        Debug.Log("待機状態：開始処理");
    }

    public void Update(MachineStateController machine)
    {
        // 待機中はマシンへの入力を受け付けない
    }

    public void Finalize(MachineStateController machine)
    {
        Debug.Log("待機状態：終了処理");
    }
}