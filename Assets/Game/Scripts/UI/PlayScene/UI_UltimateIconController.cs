using UnityEngine;
using UnityEngine.UI;
using static MachineUltimateModule;

public class UI_UltimateIconController : MonoBehaviour, IVehicleReceiver
{
    [Header("Icon Image")]
    [SerializeField] private Image iconImage;

    [Header("Ultimate Sprites")]
    [SerializeField] private Sprite boostSprite;
    [SerializeField] private Sprite empSprite;
    [SerializeField] private Sprite shieldSprite;

    [Header("Color Settings")]
    [SerializeField] private Color inactiveColor = new Color(0.7f, 0.7f, 0.7f, 1f); // 溜まってない
    [SerializeField] private Color activeColor = Color.white;                      // 満タン

    private VehicleController _vehicleController;
    private MachineUltimateModule _ultimateModule;

    // ================================
    // Vehicle 接続
    // ================================
    public void Receipt(GameObject vehicle, Rigidbody rigidbody)
    {
        _vehicleController = vehicle.GetComponent<VehicleController>();
        if (_vehicleController == null) return;

        _ultimateModule = _vehicleController.Find<MachineUltimateModule>();
    }

    private void Update()
    {
        if (_ultimateModule == null) return;

        UpdateIconSprite();
        UpdateIconColor();
    }

    // ================================
    // Sprite 切り替え
    // ================================
    private void UpdateIconSprite()
    {
        UltimateName name = _ultimateModule.GetUltimateName();

        switch (name)
        {
            case UltimateName.ULT_Boost:
                SetSprite(boostSprite);
                break;

            case UltimateName.ULT_EMP:
                SetSprite(empSprite);
                break;

            case UltimateName.ULT_Shield:
                SetSprite(shieldSprite);
                break;

            default:
                iconImage.enabled = false;
                break;
        }
    }

    private void SetSprite(Sprite sprite)
    {
        iconImage.sprite = sprite;
        iconImage.enabled = (sprite != null);
    }

    // ================================
    // 色（状態）制御
    // ================================
    private void UpdateIconColor()
    {
        if (!iconImage.enabled) return;

        bool isFull =
            _ultimateModule.CurrentGauge >= _ultimateModule.MaxUltimateGauge;

        iconImage.color = isFull ? activeColor : inactiveColor;
    }
}
