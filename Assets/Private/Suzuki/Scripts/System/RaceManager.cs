using UnityEngine;
using System.Collections;

public class RaceManager : MonoBehaviour
{
    public enum RaceState
    {
        Waiting,    // レース開始前
        Countdown,  // カウントダウン中
        Racing,     // レース中
        Finished    // ゴール後
    }

    public RaceState currentState = RaceState.Waiting;
    public PlayerInputHandler playerInputHandler;
    public float countdownTime = 3f;

    private float raceStartTime;
    private float raceEndTime;

    void Start()
    {
        // 最初は待機状態
        currentState = RaceState.Waiting;
        // 最初は操作を無効にしておく
        if (playerInputHandler != null)
            playerInputHandler.enabled = false;
    }

    void Update()
    {
        // 状態によって処理を分ける
        switch (currentState)
        {
            case RaceState.Waiting:
                // キーを押したらカウントダウン開始
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine(StartCountdown());
                }
                break;

            case RaceState.Racing:
                // レース中の処理
                float elapsed = Time.time - raceStartTime;
                // Debug.Log($"Race Time: {elapsed:F2}");
                break;

            case RaceState.Finished:
                // ゴール後の処理
                break;
        }
    }

    private IEnumerator StartCountdown()
    {
        currentState = RaceState.Countdown;

        float timer = countdownTime;
        while (timer > 0f)
        {
            Debug.Log(Mathf.CeilToInt(timer));
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        Debug.Log("GO!");
        StartRace();
    }

    private void StartRace()
    {
        currentState = RaceState.Racing;
        raceStartTime = Time.time;

        // 操作を有効化
        if (playerInputHandler != null)
            playerInputHandler.enabled = true;
    }

    public void FinishRace()
    {
        if (currentState != RaceState.Racing) return;

        currentState = RaceState.Finished;
        raceEndTime = Time.time;
        float totalTime = raceEndTime - raceStartTime;
        Debug.Log($"🏁 ゴール！ 総タイム: {totalTime:F2}秒");

        // ゴールしたら操作無効化
        if (playerInputHandler != null)
            playerInputHandler.enabled = false;
    }
}
