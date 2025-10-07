// ============================================
// 
// ファイル名: SteeringControllerInput.cs
// 概要: ステアリングコントローラーの入力系処理
// 
// 製作者 : 清水駿希
// 
// ============================================

public class SteeringControllerInput : IInputDevice
{
    public GamePlayInputSnapshot GetInput { get { return _gamePlayInputSnapshot; } }

    private GamePlayInputSnapshot _gamePlayInputSnapshot;
    private SteeringController _steeringController;

    /// <summary>
    /// 入力状態を更新する
    /// </summary>
    public void GamePlayInputUpdate()
    {
        // ステアリングコントローラー
        _steeringController = SteeringController.Instance;

        _steeringController.Update();

        if (_steeringController.GetState() == false) return;

        // 毎フレームで入力を集約して Snapshot を作成
        var snapshot = new GamePlayInputSnapshot(
            handle: _steeringController.GetSteeringPosition() * -1.0f,
            accelerator: _steeringController.GetAcceleratorPosition(),
            brake: _steeringController.GetBrakePosition(),
            boost: _steeringController.GetButtonWasPressedThisFrame(SteeringController.ButtonID.A),
            cameraView: _steeringController.GetButtonWasPressedThisFrame(SteeringController.ButtonID.B)
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

