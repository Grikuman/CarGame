// ============================================
// 
// �t�@�C����: InputManager.cs
// �T�v: ���͌n�̊Ǘ��N���X�i�V���O���g���j
// 
// ����� : �����x��
// 
// ============================================
using UnityEngine;
using ShunLib.Utility;

public class InputManager : MonoBehaviour
{
    public enum InputDeviceType : uint
    {
        Keyboard           = 0,
        Gamepad            = 1,
        SteeringController = 2
    }

    // �V���O���g��
    public static InputManager Instance => Singleton<InputManager>.Instance;

    // ���͂��󂯕t����f�o�C�X
    [SerializeField] private InputDeviceType _inputDeviceType;

    // �e�f�o�C�X
    private IInputDevice[] _inputDevices;

    /// <summary>
    /// ����������
    /// </summary>
    public void Initialize()
    {
        _inputDevices = new IInputDevice[3];

        _inputDevices[(uint)InputDeviceType.Keyboard] = new KeyboardInput();
        _inputDevices[(uint)InputDeviceType.Gamepad]  = new GamePadInput();
        _inputDevices[(uint)InputDeviceType.SteeringController] = new SteeringControllerInput();
    }

    /// <summary>
    /// �h���C�o�[�̓��͒l���X�V
    /// </summary>
    public void UpdateDrivingInputAxis()
    {
        // ���̓f�o�C�X�̍X�V����
        _inputDevices[(uint)_inputDeviceType].GamePlayInputUpdate();
    }

    /// <summary>
    /// ���݂̃f�o�C�X�̓��͒l���擾
    /// </summary>
    public GamePlayInputSnapshot GetCurrentDeviceGamePlayInputSnapshot()
    {
        return _inputDevices[(uint)_inputDeviceType].GetInput;
    }
}
