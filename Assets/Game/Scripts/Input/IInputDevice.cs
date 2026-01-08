// ============================================
// 
// ファイル名: IInputDevice.cs
// 概要: 入力デバイス（インターフェース）
// 
// 製作者 : 清水駿希
// 
// ============================================
public readonly struct GamePlayInputSnapshot
{
    public float Handle { get; }
    public float Accelerator { get; }
    public float Brake { get; }
    public bool Boost { get; }
    public bool Ultimate { get; }
    public bool CameraView { get; }

    public GamePlayInputSnapshot(float handle, float accelerator, float brake, bool boost, bool ultimate, bool cameraView)
    {
        Handle = handle;
        Accelerator = accelerator;
        Brake = brake;
        Boost = boost;
        Ultimate = ultimate;
        CameraView = cameraView;
    }
}

public enum UiInputActionID
{
    None = 0,
    RIGHT,
    LEFT,
    UP,
    DOWN,
    SELECT,
    ESC,
}


public interface IInputDevice
{
    // 入力値の取得
    public GamePlayInputSnapshot GetInput { get; }
    // 入力状態を更新する
    public void GamePlayInputUpdate();
    // UI用の入力アクションが現在押されているかを判定する
    public bool IsPressed(UiInputActionID action);
    // UI用の入力アクションが今フレームで押されたかを判定する
    public bool WasPressedThisFrame(UiInputActionID action);
}
