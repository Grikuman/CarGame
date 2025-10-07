// ============================================
// 
// �t�@�C����: IInputDevice.cs
// �T�v: ���̓f�o�C�X�i�C���^�[�t�F�[�X�j
// 
// ����� : �����x��
// 
// ============================================
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
    // ���͒l�̎擾
    public GamePlayInputSnapshot GetInput { get; }
    // ���͏�Ԃ��X�V����
    public void GamePlayInputUpdate();
    // UI�p�̓��̓A�N�V���������݉�����Ă��邩�𔻒肷��
    public bool IsPressed(UiInputActionID action);
    // UI�p�̓��̓A�N�V���������t���[���ŉ����ꂽ���𔻒肷��
    public bool WasPressedThisFrame(UiInputActionID action);
}
