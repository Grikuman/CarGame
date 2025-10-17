using UnityEngine;

public class MachineStateController : MonoBehaviour
{
    // 現在のステート
    private IMachineState _currentState;

    // マシンインプット
    public IMachineInput MachineInput { get; private set; }

    void Start()
    {
        // とりあえずプレイヤーの入力にしておく　後でMachinAIControllerなんかを付け加えてもよさそう
        MachineInput = GetComponent<MachinePlayerInput>();
        // 初期ステートに切り替え
        ChangeState(new MachineWaitingState());
    }

    void Update()
    {
        // 現在のステートを更新する
        _currentState?.Update(this);


        // デバッグ用
        if(Input.GetKeyDown(KeyCode.W))
        {
            ChangeState(new MachineWaitingState());
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeState(new MachinePlayingState());
        }
    }

    /// <summary>
    /// マシンのステートを変更する
    /// </summary>
    /// <param name="newState">変更先のステート</param>
    public void ChangeState(IMachineState newState)
    {
        _currentState?.Finalize(this);
        _currentState = newState;
        _currentState.Initialize(this);
    }
}
