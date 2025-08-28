using UnityEngine;
using UnityEngine.UI;

public class CarEffectController : MonoBehaviour
{
    [Header("エフェクト設定")]
    [SerializeField] private ParticleSystem m_boostEffect;      // ブースト用エフェクト
    [SerializeField] private ParticleSystem m_ultimateEffect;   // アルティメット用エフェクト

    private BoostSystem m_boostSystem;
    private UltimateSystem m_ultimateSystem;

    private bool boostEffectPlaying = false;
    private bool ultimateEffectPlaying = false;

    void Start()
    {
        // コンポーネントを取得する
        m_boostSystem = GetComponent<BoostSystem>();
        m_ultimateSystem = GetComponent<UltimateSystem>();
    }

    void Update()
    {
        // ブーストのエフェクト制御
        BoostEffect();

        // アルティメットのエフェクト制御
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
        bool isUltimateActive = m_ultimateSystem.IsActiveUltimate(); // ← ここはUltimateSystemに実装してる状態取得メソッド

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
