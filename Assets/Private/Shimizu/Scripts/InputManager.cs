// ============================================
// 
// ファイル名: InputManager.cs
// 概要: 入力系の管理クラス（シングルトン）
// 
// 製作者 : 清水駿希
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

        Debug.Log("初期化");
    }
    
   
    // ドライバーの入力値を更新
    public void UpdateDrivingInputAxis()
    {
        // 入力デバイスの更新処理
        foreach (var device in _inputDevices)
        {
            device.GamePlayInputUpdate();
        }   
    }

    // 現在のデバイスの入力値を取得
    public GamePlayInputSnapshot GetCurrentDeviceGamePlayInputSnapshot()
    {
        return _inputDevices[(uint)_inputDeviceType].GetInput;
    }
}
