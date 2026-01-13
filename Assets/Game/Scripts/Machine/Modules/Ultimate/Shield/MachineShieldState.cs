using UnityEngine;
using System.Linq;
using System.Collections; // コルーチンに必要

public class MachineShieldState : MonoBehaviour
{
    public bool IsShieldActive { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    private VehicleController _vehicleController;
    private GameObject _shieldModel;
    private Renderer _shieldRenderer; 
    private Coroutine _fadeCoroutine;

    private void Start()
    {
        _vehicleController = GetComponent<VehicleController>();
        InitShieldModel();

        if (_shieldModel != null)
        {
            // モデルが見つかったらRendererを取得しておく
            _shieldRenderer = _shieldModel.GetComponent<Renderer>();

            // 初期状態：透明にしてから非表示にする
            SetAlpha(0);
            _shieldModel.SetActive(false);
            IsShieldActive = false;

            Debug.Log($"[MachineShieldState] ShieldModel linked & Fade ready: {_shieldModel.name}");
        }
    }

    public void Enable()
    {
        if (IsShieldActive) return;
        IsShieldActive = true;

        // フェードイン
        StartFade(1.0f);
        Debug.Log($"[MachineShieldState] ENABLE (Fade In) : {gameObject.name}");
    }

    public void Disable()
    {
        if (!IsShieldActive) return;
        IsShieldActive = false;

        // フェードアウト
        StartFade(0.0f);
        Debug.Log($"[MachineShieldState] DISABLE (Fade Out) : {gameObject.name}");
    }

    private void StartFade(float targetAlpha)
    {
        // すでにフェード中なら止めて、新しいフェードを優先する
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        if (_shieldModel == null || _shieldRenderer == null) yield break;

        // 出現するときは先にActiveにする
        if (targetAlpha > 0) _shieldModel.SetActive(true);

        Color color = _shieldRenderer.material.color;
        float startAlpha = color.a;
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / fadeDuration;
            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, percent));
            yield return null;
        }

        SetAlpha(targetAlpha);

        // 消えるときは最後に非表示にする
        if (targetAlpha <= 0) _shieldModel.SetActive(false);

        _fadeCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        if (_shieldRenderer == null) return;
        Color c = _shieldRenderer.material.color;
        c.a = alpha;
        _shieldRenderer.material.color = c;
    }

   
    // Shieldモデルの初期化処理
    private void InitShieldModel()
    {
        var shieldTransform = _vehicleController
            .GetComponentsInChildren<Transform>(true)
            .FirstOrDefault(t => t.name == "Shield");

        if (shieldTransform == null)
        {
            Debug.LogWarning("[MachineShieldState] マシンのShieldModelが見つかりません");
            return;
        }

        _shieldModel = shieldTransform.gameObject;
    }
}