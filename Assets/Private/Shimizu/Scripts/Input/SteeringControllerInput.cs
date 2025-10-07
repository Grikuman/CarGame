// ============================================
// 
// �t�@�C����: SteeringControllerInput.cs
// �T�v: �X�e�A�����O�R���g���[���[�̓��͌n����
// 
// ����� : �����x��
// 
// ============================================

public class SteeringControllerInput : IInputDevice
{
    public GamePlayInputSnapshot GetInput { get { return _gamePlayInputSnapshot; } }

    private GamePlayInputSnapshot _gamePlayInputSnapshot;
    private SteeringController _steeringController;

    /// <summary>
    /// ���͏�Ԃ��X�V����
    /// </summary>
    public void GamePlayInputUpdate()
    {
        // �X�e�A�����O�R���g���[���[
        _steeringController = SteeringController.Instance;

        _steeringController.Update();

        if (_steeringController.GetState() == false) return;

        // ���t���[���œ��͂��W�񂵂� Snapshot ���쐬
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

