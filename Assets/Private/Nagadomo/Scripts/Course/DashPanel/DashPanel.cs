using System.Collections;
using UnityEngine;

public class DashPanel : MonoBehaviour
{
    [Header("加速設定")]
    public float speedBoostMultiplier = 1.2f;  // 一瞬だけ増やす倍率
    public float duration = 0.4f;

    private const float RESET_MULTIPLIER = 1.0f;

    [Header("カメラ演出設定")]
    public FollowCameraController followCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var vc = other.GetComponent<VehicleController>();
        if (vc == null) return;

        var engine = vc.Find<MachineEngineModule>();
        if (engine == null) return;

        // --- 加速処理開始 ---
        StartCoroutine(ApplySpeedBoost(engine));

        // --- カメラのダッシュパネル演出 ---
        if (followCamera != null)
        {
            followCamera.TriggerDashPanelZoom();
        }
    }

    private IEnumerator ApplySpeedBoost(MachineEngineModule engine)
    {
        // ダッシュパネル専用倍率に変更
        engine.ExternalBoostMultiplier = speedBoostMultiplier;

        // 指定時間だけ効果を持続
        yield return new WaitForSeconds(duration);

        // 元の倍率に戻す
        engine.ExternalBoostMultiplier = RESET_MULTIPLIER;
    }
}
