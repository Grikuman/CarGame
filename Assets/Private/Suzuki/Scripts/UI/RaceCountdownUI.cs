using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaceCountdownUI : MonoBehaviour
{
    [Header("Race Manager")]
    [SerializeField] private RaceManager raceManager;

    [Header("Countdown Images")]
    [SerializeField] private Image[] numberImages; // 0:1, 1:2, 2:3
    [SerializeField] private Image goImage;

    [Header("GO Settings")]
    [SerializeField] private float goDisplayTime = 0.6f;

    private int _currentNumber = -1;
    private RaceManager.RaceState _prevState;
    private Coroutine _goCoroutine;

    private void Awake()
    {
        HideAll();
    }

    private void Update()
    {
        if (raceManager == null) return;

        if (_prevState != raceManager.CurrentState)
        {
            OnRaceStateChanged(raceManager.CurrentState);
            _prevState = raceManager.CurrentState;
        }

        if (raceManager.CurrentState == RaceManager.RaceState.Countdown)
        {
            UpdateCountdownNumber();
        }
    }

    // ================================
    // State変化
    // ================================
    private void OnRaceStateChanged(RaceManager.RaceState state)
    {
        switch (state)
        {
            case RaceManager.RaceState.Waiting:
            case RaceManager.RaceState.Finished:
                HideAll();
                break;

            case RaceManager.RaceState.Countdown:
                _currentNumber = -1;
                HideAll();
                break;

            case RaceManager.RaceState.Racing:
                ShowGoWithAutoHide();
                break;
        }
    }

    // ================================
    // カウントダウン数字
    // ================================
    private void UpdateCountdownNumber()
    {
        float elapsed = Time.time - raceManager.CountdownStartTime;
        float remaining = raceManager.countdownTime - elapsed;

        int number = Mathf.CeilToInt(remaining);
        if (number <= 0 || number == _currentNumber) return;

        _currentNumber = number;
        ShowNumber(number);
    }

    // ================================
    // 表示処理
    // ================================
    private void ShowNumber(int number)
    {
        int index = number - 1;
        if (index < 0 || index >= numberImages.Length) return;

        StopGoCoroutine();

        HideAllNumbers();

        if (numberImages[index] != null)
            numberImages[index].enabled = true;

        if (goImage != null)
            goImage.enabled = false;
    }

    private void ShowGoWithAutoHide()
    {
        StopGoCoroutine();

        HideAllNumbers();

        if (goImage != null)
            goImage.enabled = true;

        _goCoroutine = StartCoroutine(GoAutoHideCoroutine());
    }

    private IEnumerator GoAutoHideCoroutine()
    {
        yield return new WaitForSeconds(goDisplayTime);
        HideAll();
    }

    private void StopGoCoroutine()
    {
        if (_goCoroutine != null)
        {
            StopCoroutine(_goCoroutine);
            _goCoroutine = null;
        }
    }

    // ================================
    // 非表示系
    // ================================
    private void HideAll()
    {
        StopGoCoroutine();
        HideAllNumbers();

        if (goImage != null)
            goImage.enabled = false;
    }

    private void HideAllNumbers()
    {
        foreach (var img in numberImages)
        {
            if (img != null)
                img.enabled = false;
        }
    }
}
