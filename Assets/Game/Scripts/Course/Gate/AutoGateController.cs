using UnityEngine;

public class AutoGateController : MonoBehaviour
{
    [Header("Gate Parts")]
    public Transform gateTop;       // 上
    public Transform gateBottom;    // 下
    public Transform gateLeft;      // 左（Z- に動く）
    public Transform gateRight;     // 右（Z+ に動く）

    [Header("Movement Settings")]
    public float openDistance = 40f;
    public float moveAmount = 5f;   // どれだけ開くか
    public float moveSpeed = 5f;

    [Header("Close Settings")]
    public float closeDelay = 2f;
    private float closeTimer = 0f;

    private bool isOpen = false;
    private Transform player;

    // 初期位置
    private Vector3 topInitialPos;
    private Vector3 bottomInitialPos;
    private Vector3 leftInitialPos;
    private Vector3 rightInitialPos;

    void Start()
    {
        if (gateTop) topInitialPos = gateTop.localPosition;
        if (gateBottom) bottomInitialPos = gateBottom.localPosition;
        if (gateLeft) leftInitialPos = gateLeft.localPosition;
        if (gateRight) rightInitialPos = gateRight.localPosition;
    }

    void Update()
    {
        // 毎フレーム Player 探す（Startだと見つからないことがあるため）
        if (!player)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj) player = pObj.transform;
            else return;
        }

        // 距離チェック
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist < openDistance)
        {
            isOpen = true;
            closeTimer = 0f;
        }
        else
        {
            closeTimer += Time.deltaTime;
            if (closeTimer > closeDelay)
                isOpen = false;
        }

        AnimateGate();
    }

    void AnimateGate()
    {
        // --- 上下 ---
        if (gateTop)
        {
            Vector3 target = isOpen ?
                topInitialPos + new Vector3(0, moveAmount, 0) :
                topInitialPos;

            gateTop.localPosition =
                Vector3.Lerp(gateTop.localPosition, target, Time.deltaTime * moveSpeed);
        }

        if (gateBottom)
        {
            Vector3 target = isOpen ?
                bottomInitialPos + new Vector3(0, -moveAmount, 0) :
                bottomInitialPos;

            gateBottom.localPosition =
                Vector3.Lerp(gateBottom.localPosition, target, Time.deltaTime * moveSpeed);
        }

        // --- 左右(Z軸方向に開閉) ---
        if (gateLeft)
        {
            Vector3 target = isOpen ?
                leftInitialPos + new Vector3(0, 0, -moveAmount) :  // Z-
                leftInitialPos;

            gateLeft.localPosition =
                Vector3.Lerp(gateLeft.localPosition, target, Time.deltaTime * moveSpeed);
        }

        if (gateRight)
        {
            Vector3 target = isOpen ?
                rightInitialPos + new Vector3(0, 0, moveAmount) :  // Z+
                rightInitialPos;

            gateRight.localPosition =
                Vector3.Lerp(gateRight.localPosition, target, Time.deltaTime * moveSpeed);
        }
    }
}
