using UnityEngine.InputSystem;

public class GamePadInput : IInputDevice
{
    private Gamepad _gamepad;

    private GamePlayInputSnapshot _gamePlayInputSnapshot;

    public GamePlayInputSnapshot GetInput { get { return _gamePlayInputSnapshot; } }

    public void GamePlayInputUpdate()
    {
        _gamepad = Gamepad.current;

        if (_gamepad == null ) return;

        // ñàÉtÉåÅ[ÉÄÇ≈ì¸óÕÇèWñÒÇµÇƒ Snapshot ÇçÏê¨
        var snapshot = new GamePlayInputSnapshot(
            handle: _gamepad.leftStick.x.ReadValue() * -1.0f,
            accelerator: _gamepad.rightTrigger.ReadValue(),
            brake: _gamepad.leftTrigger.ReadValue(),
            boost: _gamepad.aButton.wasPressedThisFrame,
            cameraView: _gamepad.bButton.wasPressedThisFrame
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

