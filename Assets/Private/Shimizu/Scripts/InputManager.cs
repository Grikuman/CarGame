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
        // ステアリングコントローラー
        _steeringController = SteeringController.Instance;
        Debug.Log("初期化");
    }

   
    // ドライバーの入力値を更新
    public void UpdateDrivingInputAxis()
    {
        _steeringController.Update();

        float horizontal = 0;
        float accelerator = 0;
        float brake = 0;

        // 現在のキーボード情報
        var currentKey = Keyboard.current;
        // 現在のパッド情報
        var currentPad = Gamepad.current;

        switch (_inputDeviceType)
        {
            case InputDeviceType.Keyboard:

                if (currentKey == null) return;

                // ハンドルの値
                if(currentKey.leftArrowKey.isPressed) horizontal = 1;
                if(currentKey.rightArrowKey.isPressed)horizontal = -1;

                // アクセルの値
                if (currentKey.upArrowKey.isPressed || currentKey.zKey.isPressed)
                    accelerator = 1.0f;

                // ブレーキの値
                if(currentKey.downArrowKey.isPressed || currentKey.xKey.isPressed)
                    brake = 1.0f;

                break;
            case InputDeviceType.Gamepad:

                if(currentPad == null) return;

                // ハンドルの値
                horizontal = currentPad.leftStick.x.ReadValue() * -1.0f;
                // アクセルの値
                accelerator = currentPad.rightTrigger.ReadValue();
                // ブレーキの値
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

        // 入力値の更新
        _handleAxis = horizontal;
        _acceleratorAxis = accelerator;
        _brakeAxis = brake;
    }



    // ブーストボタン状態を取得する
    public bool GetBoostButtonState()
    {
        bool isPressed = false;

        // 現在のキーボード情報
        var currentKey = Keyboard.current;
        // 現在のパッド情報
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

    // UI 左移動状態を取得する
    public bool GetLeftButtonState()
    {
        // 現在のキーボード情報
        var currentKey = Keyboard.current;
        // 現在のパッド情報
        var currentPad = Gamepad.current;

        if (currentKey.leftArrowKey.wasPressedThisFrame ||
            currentPad.leftShoulder.wasPressedThisFrame || 
            _steeringController.GetPOVWasPressedThisFrame(SteeringController.POVDirection.LEFT) )
            return true;

        return false;
    }
    // UI 右移動状態を取得する
    public bool GetRightButtonState()
    {
        // 現在のキーボード情報
        var currentKey = Keyboard.current;
        // 現在のパッド情報
        var currentPad = Gamepad.current;

        if (currentKey.rightArrowKey.wasPressedThisFrame ||
            currentPad.rightShoulder.wasPressedThisFrame ||
            _steeringController.GetPOVWasPressedThisFrame(SteeringController.POVDirection.RIGHT))
            return true;

        return false;
    }
    // UI 決定状態を取得する
    public bool GetSelectButtonState()
    {
        // 現在のキーボード情報
        var currentKey = Keyboard.current;
        // 現在のパッド情報
        var currentPad = Gamepad.current;

        if (currentKey.zKey.wasPressedThisFrame ||
            currentPad.aButton.wasPressedThisFrame ||
            _steeringController.GetButtonWasPressedThisFrame(SteeringController.ButtonID.A))
            return true;

        return false;
    }
    // キャンセルボタン状態を取得する
    public bool GetCancelButtonState()
    {
        // 現在のキーボード情報
        var currentKey = Keyboard.current;
        // 現在のパッド情報
        var currentPad = Gamepad.current;

        if (currentKey.escapeKey.wasPressedThisFrame ||
            currentPad.bButton.wasPressedThisFrame ||
            _steeringController.GetButtonWasPressedThisFrame(SteeringController.ButtonID.B))
            return true;

        return false;
    }

}
