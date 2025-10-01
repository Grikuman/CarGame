// ============================================
// 
// �t�@�C����: InputManager.cs
// �T�v: ���͌n�̊Ǘ��N���X�i�V���O���g���j
// 
// ����� : �����x��
// 
// ============================================
using UnityEngine;
using UnityEngine.InputSystem;
using ShunLib.Utility;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance => Singleton<InputManager>.Instance;

    public enum InputDeviceType
    {
        Keyboard = 0,
        Gamepad = 1,
        SteeringWheel = 2
    }

    private IInputDevice[] _inputDevices;

    [SerializeField]
    private InputDeviceType _inputDeviceType;

    public void Init()
    {
        _inputDevices = new IInputDevice[3];

        _inputDevices[(uint)InputDeviceType.Keyboard] = new KeyboardInput();
        _inputDevices[(uint)InputDeviceType.Gamepad]  = new GamePadInput();
        _inputDevices[(uint)InputDeviceType.SteeringWheel] = new SteeringControllerInput();

        Debug.Log("������");
    }
    
   
    // �h���C�o�[�̓��͒l���X�V
    public void UpdateDrivingInputAxis()
    {
        // ���̓f�o�C�X�̍X�V����
        foreach (var device in _inputDevices)
        {
            device.GamePlayInputUpdate();
        }   
    }

    // ���݂̃f�o�C�X�̓��͒l���擾
    public GamePlayInputSnapshot GetCurrentDeviceGamePlayInputSnapshot()
    {
        return _inputDevices[(uint)_inputDeviceType].GetInput;
    }
}
