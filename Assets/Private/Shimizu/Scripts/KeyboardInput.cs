using UnityEngine;

public class KeyboardInput : IInputDevice
{

    private GamePlayInputSnapshot _gamePlayInputSnapshot;

    public GamePlayInputSnapshot GetInput { get { return _gamePlayInputSnapshot; } }

    public void GamePlayInputUpdate()
    {
        // 毎フレームで入力を集約して Snapshot を作成
        var snapshot = new GamePlayInputSnapshot(
            handle: this.GetHandleAxis(),
            accelerator: this.GetAcceleratorAxis(),
            brake: this.GetBrakeAxis(),
            boost: Input.GetKeyDown(KeyCode.Space),
            cameraView: Input.GetKeyDown(KeyCode.C)
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

    private float GetHandleAxis()
    {
        float handle = 0;
        // 右
        if(Input.GetKey(KeyCode.RightArrow)) handle = 1;
        // 左
        if(Input.GetKey(KeyCode.LeftArrow)) handle = -1;

        return handle;
    }

    private float GetAcceleratorAxis()
    {
        float accelerator = 0;
        // 上キー
        if(Input.GetKey(KeyCode.UpArrow)) accelerator = 1;

        return accelerator; 
    }

    private float GetBrakeAxis()
    {
        float brake = 0;
        // 上キー
        if (Input.GetKey(KeyCode.DownArrow)) brake = 1;

        return brake;
    }

}
