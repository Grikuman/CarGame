using UnityEngine;
using UnityEngine.InputSystem;

public class TEST_Controller : MonoBehaviour
{
    private TEST_1 _machineEngineController;
    private VehiclePhysicsModule _vehiclePhysicsModule;
    private InputManager _inputManager;

    private void Start()
    {
        // コンポーネントを取得する
        _machineEngineController = GetComponent<TEST_1>();
        _vehiclePhysicsModule = GetComponent<VehiclePhysicsModule>();

        // インプットマネージャーのインスタンスを取得・初期化
        _inputManager = InputManager.Instance;
        _inputManager.Initialize();
    }

    private void Update()
    {
        // 入力値を更新する
        _inputManager.UpdateDrivingInputAxis();
        // 入力値を取得する
        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        // ハンドルの更新
        _vehiclePhysicsModule._input = input.Handle * 0.3f;
        // アクセルの更新
        _machineEngineController.InputThrottle = input.Accelerator;
        // ブレーキの更新
        _machineEngineController.InputBrake = input.Brake;

        // テスト用
        float inputHorizontal = Input.GetAxis("Horizontal");
        // 横滑りの追加
        _machineEngineController.InputSteer = (inputHorizontal);
    }
}
