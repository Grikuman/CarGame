// ============================================
// 
// ファイル名: KeyboardInput.cs
// 概要: キーボードの入力系処理
// 
// 製作者 : 清水駿希
// 
// ============================================
using UnityEngine;

public class KeyboardInput : IInputDevice
{
    public GamePlayInputSnapshot GetInput { get { return _gamePlayInputSnapshot; } }


    private GamePlayInputSnapshot _gamePlayInputSnapshot;


    /// <summary>
    /// 入力状態を更新する
    /// </summary>
    public void GamePlayInputUpdate()
    {
        // 毎フレームで入力を集約して Snapshot を作成
        var snapshot = new GamePlayInputSnapshot(
            handle: this.GetHandleAxis(),
            accelerator: this.GetAcceleratorAxis(),
            brake: this.GetBrakeAxis(),
            boost: Input.GetKeyDown(KeyCode.Space),
            ultimate: Input.GetKeyDown(KeyCode.R),
            cameraView: Input.GetKeyDown(KeyCode.C)
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
                return Input.GetKeyDown(KeyCode.RightArrow);
            case UiInputActionID.LEFT:
                return Input.GetKeyDown(KeyCode.LeftArrow);
            case UiInputActionID.UP:
                return Input.GetKeyDown(KeyCode.UpArrow);
            case UiInputActionID.DOWN:
                return Input.GetKeyDown(KeyCode.DownArrow);
            case UiInputActionID.ESC:
                return Input.GetKeyDown(KeyCode.Escape);
            case UiInputActionID.SELECT:
                return Input.GetKeyDown(KeyCode.Z);
            default:
                break;
        }
        return false;
    }

    /// <summary>
    /// ハンドルの入力値を取得する
    /// </summary>
    private float GetHandleAxis()
    {
        float handle = 0f;
        // 右
        if(Input.GetKey(KeyCode.RightArrow)) handle = -1f;
        // 左
        if(Input.GetKey(KeyCode.LeftArrow)) handle = 1f;

        return handle;
    }
    /// <summary>
    /// アクセルの入力値を取得する
    /// </summary>
    private float GetAcceleratorAxis()
    {
        float accelerator = 0f;
        // 上キー
        if(Input.GetKey(KeyCode.UpArrow)) accelerator = 1f;

        return accelerator; 
    }
    /// <summary>
    /// ブレーキの入力値を取得する
    /// </summary>
    private float GetBrakeAxis()
    {
        float brake = 0f;
        // 上キー
        if (Input.GetKey(KeyCode.DownArrow)) brake = 1f;

        return brake;
    }

}
