// ============================================
// 
// �t�@�C����: KeyboardInput.cs
// �T�v: �L�[�{�[�h�̓��͌n����
// 
// ����� : �����x��
// 
// ============================================
using UnityEngine;

public class KeyboardInput : IInputDevice
{
    public GamePlayInputSnapshot GetInput { get { return _gamePlayInputSnapshot; } }


    private GamePlayInputSnapshot _gamePlayInputSnapshot;


    /// <summary>
    /// ���͏�Ԃ��X�V����
    /// </summary>
    public void GamePlayInputUpdate()
    {
        // ���t���[���œ��͂��W�񂵂� Snapshot ���쐬
        var snapshot = new GamePlayInputSnapshot(
            handle: this.GetHandleAxis(),
            accelerator: this.GetAcceleratorAxis(),
            brake: this.GetBrakeAxis(),
            boost: Input.GetKeyDown(KeyCode.Space),
            cameraView: Input.GetKeyDown(KeyCode.C)
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
                return Input.GetKeyDown(KeyCode.RightArrow);
            case UiInputActionID.LEFT:
                return Input.GetKeyDown(KeyCode.LeftArrow);
            case UiInputActionID.UP:
                return Input.GetKeyDown(KeyCode.UpArrow);
            case UiInputActionID.DOWN:
                return Input.GetKeyDown(KeyCode.DownArrow);
            case UiInputActionID.ESC:
                return Input.GetKeyDown(KeyCode.Escape);
            default:
                break;
        }
        return false;
    }

    /// <summary>
    /// �n���h���̓��͒l���擾����
    /// </summary>
    private float GetHandleAxis()
    {
        float handle = 0f;
        // �E
        if(Input.GetKey(KeyCode.RightArrow)) handle = -1f;
        // ��
        if(Input.GetKey(KeyCode.LeftArrow)) handle = 1f;

        return handle;
    }
    /// <summary>
    /// �A�N�Z���̓��͒l���擾����
    /// </summary>
    private float GetAcceleratorAxis()
    {
        float accelerator = 0f;
        // ��L�[
        if(Input.GetKey(KeyCode.UpArrow)) accelerator = 1f;

        return accelerator; 
    }
    /// <summary>
    /// �u���[�L�̓��͒l���擾����
    /// </summary>
    private float GetBrakeAxis()
    {
        float brake = 0f;
        // ��L�[
        if (Input.GetKey(KeyCode.DownArrow)) brake = 1f;

        return brake;
    }

}
