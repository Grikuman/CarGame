// マシンの待機状態
using UnityEngine;

public class MachinePlayingState : IMachineState
{
    public void Initialize(MachineStateController machine)
    {
        Debug.Log("プレイ状態：開始処理");
    }

    public void Update(MachineStateController machine)
    {
        // マシンへの入力を受け付ける
        machine.MachineInput.InputUpdate();
    }

    public void Finalize(MachineStateController machine)
    {
        Debug.Log("プレイ状態：終了処理");
    }
}