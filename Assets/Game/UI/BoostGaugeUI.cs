using UnityEngine;
using UnityEngine.UI;

public class BoostGaugeUI : MonoBehaviour
{
    [SerializeField] private BoostSystem boostSystem; // ƒvƒŒƒCƒ„[‚ÌBoostSystem
    [SerializeField] private Image fillImage;         // BoostGauge_Fill‚ÌImage

    void Update()
    {
        if (boostSystem != null && fillImage != null)
        {
            // 0`1 ‚ÌŠ„‡‚ğ‚»‚Ì‚Ü‚Ü”½‰f
            fillImage.fillAmount = boostSystem.GetBoostGaugeNormalized();
        }
    }
}
