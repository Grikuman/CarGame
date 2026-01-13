using UnityEngine;

public class Ultimate_EMP : UltimateBase
{
    private float _range = 15f;

    // ‰¼‚Ì”’l
    private float _enemyBoostDamage = 30f;
    private float _selfBoostHeal = 20f;

    public override void Activate(MachineEngineModule engine)
    {
        base.Activate(engine);

        var ownerVC = engine.Owner;
        if (ownerVC == null) return;

        bool hitEnemy = false;

        Collider[] hits = Physics.OverlapSphere(
            ownerVC.transform.position,
            _range
        );

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Player")) continue;
            if (!hit.TryGetComponent(out VehicleController targetVC)) continue;
            if (targetVC == ownerVC) continue;

            var net = targetVC.GetComponent<NetworkMachineEMP>();
            if (net != null)
            {
                Debug.Log("“G”­Œ©");
                hitEnemy = true;
                net.RPC_RequestEMP(_enemyBoostDamage);
            }
        }

        // “G‚ª‚¢‚½ê‡‚Ì‚İ©•ª‰ñ•œ
        if (hitEnemy)
        {
            var boost = ownerVC.Find<MachineBoostModule>();
            boost.IncreaseGauge(_selfBoostHeal);
        }
    }
}
