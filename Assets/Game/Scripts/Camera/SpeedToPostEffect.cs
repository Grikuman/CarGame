using UnityEngine;

public class SpeedToPostEffect : MonoBehaviour,IVehicleReceiver
{
    public Material postEffectMaterial;  // FullScreenShaderGraph のマテリアル
    private Rigidbody machineRigidbody;              // マシン制御スクリプト

    public void Receipt(GameObject vehicle, Rigidbody rigidbody)
    {
        machineRigidbody = rigidbody;
    }

    void Update()
    {
        if (machineRigidbody == null) return;

        float speed = machineRigidbody.linearVelocity.magnitude; // 速度を取得

        //スピードを正規化して滑らかに
        float raw = Mathf.Clamp01(speed / 400f);
        float speedFactor = Mathf.SmoothStep(0f, 1f, raw);

        //シェーダーに渡す
        postEffectMaterial.SetFloat("_MachineSpeed", speedFactor);
    }
}
