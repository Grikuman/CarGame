public class SteeringControllerInput : IInputDevice
{
    private GamePlayInputSnapshot _gamePlayInputSnapshot;

    public GamePlayInputSnapshot GetInput { get { return _gamePlayInputSnapshot; } }

    private SteeringController _steeringController;

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

