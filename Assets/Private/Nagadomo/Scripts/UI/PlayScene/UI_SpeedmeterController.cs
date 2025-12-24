using UnityEngine;
using UnityEngine.UI;

public class UI_SpeedmeterController : MonoBehaviour, IVehicleReceiver
{
    [Header("SpeedMeterの画像設定(Low -> High)")]
    [SerializeField] private Image[] speedSegments; // 14個

    private VehicleController _vehicleController;
    private MachineEngineModule _machineEngineModule;

    private const int SPEED_DIVISION = 14;

    // ================================
    // Vehicle 接続
    // ================================
    public void Receipt(GameObject vehicle, Rigidbody rigidbody)
    {
        _vehicleController = vehicle.GetComponent<VehicleController>();
        _machineEngineModule = _vehicleController.Find<MachineEngineModule>();
    }

    // ================================
    // 更新
    // ================================
    private void Update()
    {
        if (_machineEngineModule == null) return;
        if (speedSegments == null || speedSegments.Length == 0) return;

        UpdateSpeedMeter();
    }

    // ================================
    // スピードメーター更新
    // ================================
    private void UpdateSpeedMeter()
    {
        float currentSpeed = _machineEngineModule.CurrentSpeed;
        float maxSpeed = _machineEngineModule.MaxSpeed;

        if (maxSpeed <= 0f) return;

        // 0～1 に正規化
        float normalizedSpeed = Mathf.Clamp01(currentSpeed / maxSpeed);

        // 表示するメモリ数（0～14）
        int activeCount = Mathf.FloorToInt(normalizedSpeed * SPEED_DIVISION);
        activeCount = Mathf.Clamp(activeCount, 0, SPEED_DIVISION);

        // メモリ表示制御
        for (int i = 0; i < speedSegments.Length; i++)
        {
            if (speedSegments[i] == null) continue;
            speedSegments[i].enabled = (i < activeCount);
        }
    }
}
