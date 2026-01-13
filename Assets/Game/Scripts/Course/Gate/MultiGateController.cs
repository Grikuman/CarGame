using UnityEngine;
using Fusion;

public class MultiGateController : NetworkBehaviour
{
    [Header("Gate Parts")]
    public Transform gateTop;
    public Transform gateBottom;
    public Transform gateLeft;
    public Transform gateRight;

    [Header("Settings")]
    public float openDistance = 150f;
    public float moveAmount = 25f;
    public float moveSpeed = 5f;
    public float closeDelay = 2f;

    [Networked] private bool IsOpen { get; set; }

    private float closeTimer;

    private Vector3 topInit, bottomInit, leftInit, rightInit;

    public override void Spawned()
    {
        if (gateTop) topInit = gateTop.localPosition;
        if (gateBottom) bottomInit = gateBottom.localPosition;
        if (gateLeft) leftInit = gateLeft.localPosition;
        if (gateRight) rightInit = gateRight.localPosition;
    }

    public override void FixedUpdateNetwork()
    {
        // Åö èÛë‘îªíËÇÕÉzÉXÉgÇÃÇ›
        if (!Object.HasStateAuthority) return;

        bool anyPlayerNear = false;

        foreach (var player in Runner.ActivePlayers)
        {
            if (Runner.TryGetPlayerObject(player, out var playerObj))
            {
                float dist = Vector3.Distance(
                    playerObj.transform.position,
                    transform.position
                );

                if (dist < openDistance)
                {
                    anyPlayerNear = true;
                    break;
                }
            }
        }

        if (anyPlayerNear)
        {
            IsOpen = true;
            closeTimer = 0f;
        }
        else
        {
            closeTimer += Runner.DeltaTime;
            if (closeTimer > closeDelay)
                IsOpen = false;
        }
    }

    void Update()
    {
        if (Runner == null || !Runner.IsRunning) return;
        AnimateGate();
    }

    void AnimateGate()
    {
        MovePart(gateTop, topInit + Vector3.up * moveAmount, topInit);
        MovePart(gateBottom, bottomInit + Vector3.down * moveAmount, bottomInit);
        MovePart(gateLeft, leftInit + Vector3.back * moveAmount, leftInit);
        MovePart(gateRight, rightInit + Vector3.forward * moveAmount, rightInit);
    }

    void MovePart(Transform part, Vector3 openPos, Vector3 closePos)
    {
        if (!part) return;

        Vector3 target = IsOpen ? openPos : closePos;
        part.localPosition = Vector3.Lerp(
            part.localPosition,
            target,
            Time.deltaTime * moveSpeed
        );
    }
}
