using UnityEngine;
using UnityEngine.UI;

public class UltimateGaugeUI : MonoBehaviour
{
    [SerializeField] private UltimateSystem ultimateSystem; // プレイヤーのUltimateSystem
    [SerializeField] private Image fillImage;               // 円形ゲージのImage

    void Update()
    {
        if (ultimateSystem != null && fillImage != null)
        {
            fillImage.fillAmount = ultimateSystem.GetUltimateGaugeNormalized();
        }
    }
}
