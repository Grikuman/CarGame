using UnityEngine;
using UnityEngine.UI;

public class UI_UltimateGaugeController : MonoBehaviour, IVehicleReceiver
{
    [Header("Mask")]
    [SerializeField] private RectMask2D mask;

    [Header("Top Padding Settings")]
    [SerializeField] private float topPaddingAtFull = 935f;  // 100%
    [SerializeField] private float topPaddingAtEmpty = 1065f; // 0%

    private VehicleController _vehicleController;
    private MachineUltimateModule _machineUltimateModule;

    // ================================
    // Vehicle 接続
    // ================================
    public void Receipt(GameObject vehicle, Rigidbody rigidbody)
    {
        _vehicleController = vehicle.GetComponent<VehicleController>();
        if (_vehicleController == null) return;

        _machineUltimateModule = _vehicleController.Find<MachineUltimateModule>();
    }

    private void Update()
    {
        if (_machineUltimateModule == null) return;
        UpdateUltimateGauge();
    }

    // ================================
    // アルティメットゲージ更新
    // ================================
    private void UpdateUltimateGauge()
    {
        float current = _machineUltimateModule.CurrentGauge;
        float max = _machineUltimateModule.MaxUltimateGauge;
        if (max <= 0f) return;

        float ratio = Mathf.Clamp01(current / max);

        // ratio = 1 → 935
        // ratio = 0 → 1065
        float topPadding = Mathf.Lerp(
            topPaddingAtEmpty,
            topPaddingAtFull,
            ratio
        );

        // RectMask2D.padding の順番は
        // Left, Bottom, Right, Top
        mask.padding = new Vector4(
            0f,        // Left
            0f,        // Bottom
            0f,        // Right
            topPadding // Top ★ここだけ制御
        );
    }
}
