using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    public enum RaceState
    {
        Waiting,
        Countdown,
        Racing,
        Finished
    }

    public RaceState CurrentState { get; private set; } = RaceState.Waiting;
    public float countdownTime = 3f;

    private float raceStartTime;
    private float raceEndTime;

    public float CurrentRaceTime =>
        CurrentState == RaceState.Racing
            ? Time.time - raceStartTime
            : raceEndTime - raceStartTime;

    public RaceCountdownUI countdownUI;
    public LapUI lapUI;
    private CheckpointManager checkpointManager;

    private void Start()
    {
        checkpointManager = FindFirstObjectByType<CheckpointManager>();
        lapUI?.Show(false);
    }
    private void Update()
    {
        // スペースキーでレース開始
        if (CurrentState == RaceState.Waiting &&
            Input.GetKeyDown(KeyCode.Space))
        {
            StartRaceSequence();
        }
    }

    public void StartRaceSequence()
    {
        if (CurrentState != RaceState.Waiting) return;
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        CurrentState = RaceState.Countdown;

        float timer = countdownTime;

        while (timer > 0f)
        {
            int count = Mathf.CeilToInt(timer);

            countdownUI?.ShowNumber(count);

            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        countdownUI?.ShowGo();
        StartRace();

        yield return new WaitForSeconds(0.5f);
        countdownUI?.Hide();
    }


    private void StartRace()
    {
        CurrentState = RaceState.Racing;
        raceStartTime = Time.time;

        lapUI?.Show(true);
        lapUI?.UpdateLap(
            checkpointManager.CurrentLap + 1,
            checkpointManager.TotalLaps
        );

        Debug.Log("GO!");
    }


    public void FinishRace()
    {
        if (CurrentState != RaceState.Racing) return;

        CurrentState = RaceState.Finished;
        raceEndTime = Time.time;
        SoloPlayResultData.Instance.SetCurrentTime(raceEndTime);
        lapUI?.Show(false);
        SceneManager.LoadScene("SoloResultScene");
        Debug.Log($"🏁 ゴール！ {CurrentRaceTime:F2} 秒");
    }
}
