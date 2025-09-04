using UnityEngine;
using UnityEngine.UI;

public class BoostGaugeUI : MonoBehaviour
{
    [SerializeField] private BoostSystem boostSystem; // �v���C���[��BoostSystem
    [SerializeField] private Image fillImage;         // BoostGauge_Fill��Image

    void Update()
    {
        if (boostSystem != null && fillImage != null)
        {
            // 0�`1 �̊��������̂܂ܔ��f
            fillImage.fillAmount = boostSystem.GetBoostGaugeNormalized();
        }
    }
}
