// ============================================
// 
// ファイル名: InputManager.cs
// 概要: 入力系の管理クラス（シングルトン）
// 
// 製作者 : 清水駿希
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

    // シングルトン
    public static InputManager Instance => Singleton<InputManager>.Instance;

    // 入力を受け付けるデバイス
    [SerializeField] private InputDeviceType _inputDeviceType;

    // 各デバイス
    private IInputDevice[] _inputDevices;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        _inputDevices = new IInputDevice[3];

        _inputDevices[(uint)InputDeviceType.Keyboard] = new KeyboardInput();
        _inputDevices[(uint)InputDeviceType.Gamepad]  = new GamePadInput();
        _inputDevices[(uint)InputDeviceType.SteeringController] = new SteeringControllerInput();
    }

    /// <summary>
    /// ドライバーの入力値を更新
    /// </summary>
    public void UpdateDrivingInputAxis()
    {
        // 入力デバイスの更新処理
        _inputDevices[(uint)_inputDeviceType].GamePlayInputUpdate();
    }

    /// <summary>
    /// 現在のデバイスの入力値を取得
    /// </summary>
    public GamePlayInputSnapshot GetCurrentDeviceGamePlayInputSnapshot()
    {
        return _inputDevices[(uint)_inputDeviceType].GetInput;
    }
}
