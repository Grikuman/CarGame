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
            Debug.Log(Mathf.CeilToInt(timer));
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        StartRace();
    }

    private void StartRace()
    {
        CurrentState = RaceState.Racing;
        raceStartTime = Time.time;
        Debug.Log("GO!");
    }

    public void FinishRace()
    {
        if (CurrentState != RaceState.Racing) return;

        CurrentState = RaceState.Finished;
        raceEndTime = Time.time;
        SoloPlayResultData.Instance.SetCurrentTime(raceEndTime);
        SceneManager.LoadScene("SoloResultScene");
        Debug.Log($"🏁 ゴール！ {CurrentRaceTime:F2} 秒");
    }
}
