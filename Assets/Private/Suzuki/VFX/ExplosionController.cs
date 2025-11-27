using UnityEngine;
using System.Collections.Generic;

public class ExplosionController : MonoBehaviour
{
    // このエフェクトを構成する全てのパーティクルシステムを格納するリスト
    private List<ParticleSystem> allParticles = new List<ParticleSystem>();

    void Start()
    {
        // 1. スクリプトがアタッチされているゲームオブジェクト自身のParticleSystemを取得
        //    (親にもコンポーネントがある場合を考慮)
        ParticleSystem selfParticle = GetComponent<ParticleSystem>();
        if (selfParticle != null)
        {
            allParticles.Add(selfParticle);
        }

        // 2. 子オブジェクトも含めた全てのParticleSystemを取得
        //    (引数 true は、非アクティブな子オブジェクトも検索対象に含める)
        ParticleSystem[] childrenParticles = GetComponentsInChildren<ParticleSystem>(true);

        // 3. リストに追加 (重複しないようにする)
        foreach (ParticleSystem particle in childrenParticles)
        {
            // すでに親で取得している場合はスキップ
            if (!allParticles.Contains(particle))
            {
                allParticles.Add(particle);
            }
        }

        if (allParticles.Count == 0)
        {
            Debug.LogError("このゲームオブジェクト、またはその子オブジェクトにParticleSystemコンポーネントが見つかりません。", this);
        }
        else
        {
            Debug.Log($"合計 {allParticles.Count} 個のParticleSystemを見つけました。");
        }
    }

    void Update()
    {
        // Zキーが押されたかチェック
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (allParticles.Count > 0)
            {
                // リスト内の全てのパーティクルシステムを再生
                foreach (ParticleSystem particle in allParticles)
                {
                    // 確実に最初から再生するため、一度停止・クリアしてから再生することを推奨
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                    particle.Play();
                }

                Debug.Log("Zキーが押されました。全パーティクルシステムを再生します。");
            }
        }
    }
}