using UnityEngine;
using UnityEngine.UI;

public class UI_LapController : MonoBehaviour
{
    [Header("Race Progress Manager")]
    [SerializeField] private RaceProgressManager raceProgressManager;

    [Header("Lap Images (Index = Lap-1)")]
    [SerializeField] private Image[] lapImages; // 0:Lap1, 1:Lap2

    private int _currentLapUI = -1;

    private void Update()
    {
        if (raceProgressManager == null) return;

        int rawLap = raceProgressManager.GetCurrentLap();

        // 補正：0→1, 1→2
        int lapForUI = rawLap + 1;

        // 範囲外ガード（安全）
        lapForUI = Mathf.Clamp(lapForUI, 1, lapImages.Length);

        if (lapForUI == _currentLapUI) return;

        _currentLapUI = lapForUI;
        UpdateLapUI();
    }

    // ================================
    // ラップUI更新
    // ================================
    private void UpdateLapUI()
    {
        if (lapImages == null || lapImages.Length == 0) return;

        // 全部非表示
        foreach (var img in lapImages)
        {
            if (img != null) img.enabled = false;
        }

        int index = _currentLapUI - 1;
        if (index >= 0 && index < lapImages.Length)
        {
            lapImages[index].enabled = true;
        }
    }
}
