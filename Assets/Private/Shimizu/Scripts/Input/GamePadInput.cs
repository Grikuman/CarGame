// ============================================
// 
// �t�@�C����: GamePadInput.cs
// �T�v: �Q�[���p�b�h�̓��͌n����
// 
// ����� : �����x��
// 
// ============================================
using UnityEngine.InputSystem;

public class GamePadInput : IInputDevice
{
    public GamePlayInputSnapshot GetInput { get { return _gamePlayInputSnapshot; } }

    private Gamepad _gamepad;
    private GamePlayInputSnapshot _gamePlayInputSnapshot;

    /// <summary>
    /// ���͏�Ԃ��X�V����
    /// </summary>
    public void GamePlayInputUpdate()
    {
        _gamepad = Gamepad.current;

        if (_gamepad == null ) return;

        // ���t���[���œ��͂��W�񂵂� Snapshot ���쐬
        var snapshot = new GamePlayInputSnapshot(
            handle: _gamepad.leftStick.x.ReadValue() * -1.0f,
            accelerator: _gamepad.rightTrigger.ReadValue(),
            brake: _gamepad.leftTrigger.ReadValue(),
            boost: _gamepad.aButton.wasPressedThisFrame,
            cameraView: _gamepad.bButton.wasPressedThisFrame
        );

        _gamePlayInputSnapshot = snapshot;
    }

    /// <summary>
    /// UI�p�̓��̓A�N�V���������݉�����Ă��邩�𔻒肷��
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
    /// UI�p�̓��̓A�N�V���������t���[���ŉ����ꂽ���𔻒肷��
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

