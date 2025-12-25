using UnityEngine;

public class Ultimate_EMP : UltimateBase
{
    private float _range = 50f;

    // âºÇÃêîíl
    private float _enemyBoostDamage = 100f;
    private float _selfBoostHeal = 100f;

    public override void Activate(MachineEngineModule engine)
    {
        Debug.Log("[Ultimate_EMP] Activate called");

        base.Activate(engine);

        var ownerVC = engine.Owner;
        if (ownerVC == null)
        {
            Debug.LogWarning("[Ultimate_EMP] ownerVC is null");
            return;
        }

        bool hitEnemy = false;

        Collider[] hits = Physics.OverlapSphere(
            ownerVC.transform.position,
            _range
        );

        Debug.Log($"[Ultimate_EMP] OverlapSphere hits: {hits.Length}");

        foreach (var hit in hits)
        {
            Debug.Log($"[Ultimate_EMP] Hit: {hit.name}");

            if (!hit.CompareTag("Player"))
            {
                Debug.Log("[Ultimate_EMP] Not Player tag");
                continue;
            }

            if (!hit.TryGetComponent(out VehicleController targetVC))
            {
                Debug.Log("[Ultimate_EMP] No VehicleController");
                continue;
            }

            if (targetVC == ownerVC)
            {
                Debug.Log("[Ultimate_EMP] Skip self");
                continue;
            }

            var net = targetVC.GetComponent<VehicleEMPNetwork>();
            if (net != null)
            {
                Debug.Log("[Ultimate_EMP] Enemy found Å® Request EMP");
                hitEnemy = true;
                net.RPC_RequestEMP(_enemyBoostDamage);
            }
            else
            {
                Debug.LogWarning("[Ultimate_EMP] VehicleEMPNetwork not found");
            }
        }

        if (hitEnemy)
        {
            Debug.Log("[Ultimate_EMP] Hit enemy Å® Heal self");

            var selfNet = ownerVC.GetComponent<VehicleEMPNetwork>();
            if (selfNet != null)
            {
                selfNet.RPC_RequestSelfHeal(_selfBoostHeal);
            }
            else
            {
                Debug.LogWarning("[Ultimate_EMP] Self VehicleEMPNetwork not found");
            }
        }
        else
        {
            Debug.Log("[Ultimate_EMP] No enemy hit Å® No self heal");
        }
    }

}
