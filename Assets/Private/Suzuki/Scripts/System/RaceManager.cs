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
    public float CountdownStartTime { get; private set; }

    // インプットマネージャー
    private InputManager _inputManager;
    string sceneName;

    private void Start()
    {
        // インプットマネージャーのインスタンスを取得・初期化
        _inputManager = InputManager.Instance;
        _inputManager.Initialize();

        sceneName = SceneManager.GetActiveScene().name;
    }
    private void Update()
    {
        // 入力値を取得する
        var input = _inputManager.GetCurrentDeviceGamePlayInputSnapshot();

        Debug.Log($"[Scene] Current Scene Name: {sceneName}");

        // スペースキーでレース開始
        if (CurrentState == RaceState.Waiting &&
            input.Ultimate
            //&&sceneName=="MultiTest"
            )
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
        CountdownStartTime = Time.time;

        float timer = countdownTime;

        while (timer > 0f)
        {
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
