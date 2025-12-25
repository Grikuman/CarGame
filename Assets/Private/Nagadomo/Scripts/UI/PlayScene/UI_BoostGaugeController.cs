using UnityEngine;
using UnityEngine.UI;

public class UI_BoostGaugeController : MonoBehaviour, IVehicleReceiver
{
    [Header("Mask")]
    [SerializeField] private RectMask2D mask;

    [Header("Padding Settings")]
    [SerializeField] private float maxPadding = 1010f; // 0ゲージ時のPadding

    private VehicleController _vehicleController;
    private MachineBoostModule _machineBoostModule;

    // ================================
    // Vehicle 接続
    // ================================
    public void Receipt(GameObject vehicle, Rigidbody rigidbody)
    {
        _vehicleController = vehicle.GetComponent<VehicleController>();
        if (_vehicleController == null) return;

        _machineBoostModule = _vehicleController.Find<MachineBoostModule>();
    }

    private void Update()
    {
        if (_machineBoostModule == null) return;
        UpdateBoostGauge();
    }

    // ================================
    // ゲージ更新（Padding方式）
    // ================================
    private void UpdateBoostGauge()
    {
        float current = _machineBoostModule.CurrentGauge;
        float max = _machineBoostModule.MaxBoostGauge;
        if (max <= 0f) return;

        float ratio = Mathf.Clamp01(current / max);

        // ratio = 1 → padding 0
        // ratio = 0 → padding maxPadding
        float paddingLR = Mathf.Lerp(maxPadding, 0f, ratio);

        // ★順番に注意！！
        mask.padding = new Vector4(
            paddingLR, // Left
            0f,        // Bottom（固定）
            paddingLR, // Right
            0f         // Top（固定）
        );
    }

}
