using UnityEngine;
using UnityEngine.UI;

public class UltimateGaugeUI : MonoBehaviour
{
    [SerializeField] private UltimateSystem ultimateSystem; // �v���C���[��UltimateSystem
    [SerializeField] private Image fillImage;               // �~�`�Q�[�W��Image

    void Update()
    {
        if (ultimateSystem != null && fillImage != null)
        {
            fillImage.fillAmount = ultimateSystem.GetUltimateGaugeNormalized();
        }
    }
}
