using UnityEngine;
using UnityEngine.InputSystem;
using ShunLib.Utility;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance => Singleton<InputManager>.Instance;

    public enum InputDeviceType
    {
        Keyboard,
        Gamepad,
        SteeringWheel
    }

    private SteeringController _steeringController;

    [SerializeField]
    private InputDeviceType _inputDeviceType;

    public float _handleAxis {  get; private set; }
    public float _acceleratorAxis { get; private set; }
    public float _brakeAxis { get; private set; }

    public void Init()
    {
        // �X�e�A�����O�R���g���[���[
        _steeringController = SteeringController.Instance;
        Debug.Log("������");
    }

   
    // �h���C�o�[�̓��͒l���X�V
    public void UpdateDrivingInputAxis()
    {
        _steeringController.Update();

        float horizontal = 0;
        float accelerator = 0;
        float brake = 0;

        // ���݂̃L�[�{�[�h���
        var currentKey = Keyboard.current;
        // ���݂̃p�b�h���
        var currentPad = Gamepad.current;

        switch (_inputDeviceType)
        {
            case InputDeviceType.Keyboard:

                if (currentKey == null) return;

                // �n���h���̒l
                if(currentKey.leftArrowKey.isPressed) horizontal = 1;
                if(currentKey.rightArrowKey.isPressed)horizontal = -1;

                // �A�N�Z���̒l
                if (currentKey.upArrowKey.isPressed || currentKey.zKey.isPressed)
                    accelerator = 1.0f;

                // �u���[�L�̒l
                if(currentKey.downArrowKey.isPressed || currentKey.xKey.isPressed)
                    brake = 1.0f;

                break;
            case InputDeviceType.Gamepad:

                if(currentPad == null) return;

                // �n���h���̒l
                horizontal = currentPad.leftStick.x.ReadValue() * -1.0f;
                // �A�N�Z���̒l
                accelerator = currentPad.rightTrigger.ReadValue();
                // �u���[�L�̒l
                brake = currentPad.leftTrigger.ReadValue();


                break;
            case InputDeviceType.SteeringWheel:

                horizontal  = _steeringController.GetSteeringPosition() * -1.0f;
                accelerator = _steeringController.GetAcceleratorPosition();
                brake       = _steeringController.GetBrakePosition();

                Debug.Log(accelerator);
                Debug.Log(horizontal);
                break;
        }

        // ���͒l�̍X�V
        _handleAxis = horizontal;
        _acceleratorAxis = accelerator;
        _brakeAxis = brake;
    }



    // �u�[�X�g�{�^����Ԃ��擾����
    public bool GetBoostButtonState()
    {
        bool isPressed = false;

        // ���݂̃L�[�{�[�h���
        var currentKey = Keyboard.current;
        // ���݂̃p�b�h���
        var currentPad = Gamepad.current;

        switch (_inputDeviceType)
        {
            case InputDeviceType.Keyboard:
                if (currentKey == null) return false;
                isPressed = currentKey.spaceKey.wasPressedThisFrame;
                break;
            case InputDeviceType.Gamepad:
                if (currentPad == null) return false;
                isPressed = currentPad.aButton.wasPressedThisFrame;
                break;
            case InputDeviceType.SteeringWheel:
                if (_steeringController.GetState() == false) return false;
                isPressed = _steeringController.GetButtonWasPressedThisFrame(SteeringController.ButtonID.A);
                break;
            default:
                break;
        }

        return isPressed;
    }

    // UI ���ړ���Ԃ��擾����
    public bool GetLeftButtonState()
    {
        // ���݂̃L�[�{�[�h���
        var currentKey = Keyboard.current;
        // ���݂̃p�b�h���
        var currentPad = Gamepad.current;

        if (currentKey.leftArrowKey.wasPressedThisFrame ||
            currentPad.leftShoulder.wasPressedThisFrame || 
            _steeringController.GetPOVWasPressedThisFrame(SteeringController.POVDirection.LEFT) )
            return true;

        return false;
    }
    // UI �E�ړ���Ԃ��擾����
    public bool GetRightButtonState()
    {
        // ���݂̃L�[�{�[�h���
        var currentKey = Keyboard.current;
        // ���݂̃p�b�h���
        var currentPad = Gamepad.current;

        if (currentKey.rightArrowKey.wasPressedThisFrame ||
            currentPad.rightShoulder.wasPressedThisFrame ||
            _steeringController.GetPOVWasPressedThisFrame(SteeringController.POVDirection.RIGHT))
            return true;

        return false;
    }
    // UI �����Ԃ��擾����
    public bool GetSelectButtonState()
    {
        // ���݂̃L�[�{�[�h���
        var currentKey = Keyboard.current;
        // ���݂̃p�b�h���
        var currentPad = Gamepad.current;

        if (currentKey.zKey.wasPressedThisFrame ||
            currentPad.aButton.wasPressedThisFrame ||
            _steeringController.GetButtonWasPressedThisFrame(SteeringController.ButtonID.A))
            return true;

        return false;
    }
    // �L�����Z���{�^����Ԃ��擾����
    public bool GetCancelButtonState()
    {
        // ���݂̃L�[�{�[�h���
        var currentKey = Keyboard.current;
        // ���݂̃p�b�h���
        var currentPad = Gamepad.current;

        if (currentKey.escapeKey.wasPressedThisFrame ||
            currentPad.bButton.wasPressedThisFrame ||
            _steeringController.GetButtonWasPressedThisFrame(SteeringController.ButtonID.B))
            return true;

        return false;
    }

}
