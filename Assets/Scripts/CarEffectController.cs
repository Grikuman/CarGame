using UnityEngine;
using UnityEngine.UI;

public class CarEffectController : MonoBehaviour
{
    [Header("�G�t�F�N�g�ݒ�")]
    [SerializeField] private ParticleSystem m_boostEffect;      // �u�[�X�g�p�G�t�F�N�g
    [SerializeField] private ParticleSystem m_ultimateEffect;   // �A���e�B���b�g�p�G�t�F�N�g

    private BoostSystem m_boostSystem;
    private UltimateSystem m_ultimateSystem;

    private bool boostEffectPlaying = false;
    private bool ultimateEffectPlaying = false;

    void Start()
    {
        // �R���|�[�l���g���擾����
        m_boostSystem = GetComponent<BoostSystem>();
        m_ultimateSystem = GetComponent<UltimateSystem>();
    }

    void Update()
    {
        // �u�[�X�g�̃G�t�F�N�g����
        BoostEffect();

        // �A���e�B���b�g�̃G�t�F�N�g����
        UltimateEffect();
    }

    private void BoostEffect()
    {
        bool isBoosting = m_boostSystem.IsActiveBoost();

        if (isBoosting && !boostEffectPlaying)
        {
            m_boostEffect.Play();
            boostEffectPlaying = true;
        }
        else if (!isBoosting && boostEffectPlaying)
        {
            m_boostEffect.Stop();
            boostEffectPlaying = false;
        }
    }

    private void UltimateEffect()
    {
        bool isUltimateActive = m_ultimateSystem.IsActiveUltimate(); // �� ������UltimateSystem�Ɏ������Ă��Ԏ擾���\�b�h

        if (isUltimateActive && !ultimateEffectPlaying)
        {
            m_ultimateEffect.Play();
            ultimateEffectPlaying = true;
        }
        else if (!isUltimateActive && ultimateEffectPlaying)
        {
            m_ultimateEffect.Stop();
            ultimateEffectPlaying = false;
        }
    }
}
