// ============================================
// 
// ファイル名: GamePadInput.cs
// 概要: ゲームパッドの入力系処理
// 
// 製作者 : 清水駿希
// 
// ============================================
using UnityEngine.InputSystem;

public class GamePadInput : IInputDevice
{
    public GamePlayInputSnapshot GetInput { get { return _gamePlayInputSnapshot; } }

    private Gamepad _gamepad;
    private GamePlayInputSnapshot _gamePlayInputSnapshot;

    /// <summary>
    /// 入力状態を更新する
    /// </summary>
    public void GamePlayInputUpdate()
    {
        _gamepad = Gamepad.current;

        if (_gamepad == null ) return;

        // 毎フレームで入力を集約して Snapshot を作成
        var snapshot = new GamePlayInputSnapshot(
            handle: _gamepad.leftStick.x.ReadValue() * -1.0f,
            accelerator: _gamepad.rightTrigger.ReadValue(),
            brake: _gamepad.leftTrigger.ReadValue(),
            boost: _gamepad.aButton.wasPressedThisFrame,
            ultimate: _gamepad.bButton.wasPressedThisFrame,
            cameraView: _gamepad.yButton.wasPressedThisFrame
        );

        _gamePlayInputSnapshot = snapshot;
    }

    /// <summary>
    /// UI用の入力アクションが現在押されているかを判定する
    /// </summary>
    public bool IsPressed(UiInputActionID action)
    {
        switch (action)
        {
            case UiInputActionID.None:
                break;
            case UiInputActionID.RIGHT:
                break;
            case UiInputActionID.LEFT:
                break;
            case UiInputActionID.UP:
                break;
            case UiInputActionID.DOWN:
                break;
            case UiInputActionID.ESC:
                break;
            default:
                break;

        }

        return false;
    }

    /// <summary>
    /// UI用の入力アクションが今フレームで押されたかを判定する
    /// </summary>
    public bool WasPressedThisFrame(UiInputActionID action)
    {
        switch (action)
        {
            case UiInputActionID.None:
                break;
            case UiInputActionID.RIGHT:
                break;
            case UiInputActionID.LEFT:
                break;
            case UiInputActionID.UP:
                break;
            case UiInputActionID.DOWN:
                break;
            case UiInputActionID.ESC:
                break;
            default:
                break;

        }

        return false;
    }
}

