using UnityEngine;

public class SpeedToPostEffect : MonoBehaviour
{
    public Material postEffectMaterial;  // FullScreenShaderGraph のマテリアル
    public Rigidbody machineRigidbody;              // マシン制御スクリプト

    void Update()
    {
        float speed = machineRigidbody.linearVelocity.magnitude; // 速度を取得

        //スピードを正規化して滑らかに
        float raw = Mathf.Clamp01(speed / 400f);
        float speedFactor = Mathf.SmoothStep(0f, 1f, raw);

        //シェーダーに渡す
        postEffectMaterial.SetFloat("_MachineSpeed", speedFactor);
    }
}
