
public readonly struct GamePlayInputSnapshot
{
    public float Handle { get; }
    public float Accelerator { get; }
    public float Brake { get; }
    public bool Boost { get; }
    public bool CameraView { get; }

    public GamePlayInputSnapshot(float handle, float accelerator, float brake, bool boost, bool cameraView)
    {
        Handle = handle;
        Accelerator = accelerator;
        Brake = brake;
        Boost = boost;
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
    public GamePlayInputSnapshot GetInput { get; }
    public void GamePlayInputUpdate();

    public bool IsPressed(UiInputActionID action);

    public bool WasPressedThisFrame(UiInputActionID action);
}
