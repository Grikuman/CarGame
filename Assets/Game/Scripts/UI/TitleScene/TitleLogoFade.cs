using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;
using UnityEngine.SceneManagement;

public class TitleLogoFade
{

    // タイトルロゴオブジェクト
    private RectTransform _logoObjec = null;

    private Vignette _vignette = null;
    private ColorAdjustments _colorAdjustments = null;



    /// <summary> コンストラクタ </summary>
    public TitleLogoFade(RectTransform logoObjec)
    {
        _logoObjec = logoObjec;
        _logoObjec.gameObject.SetActive(false);
    }

    /// <summary> フェードイン </summary>
    public void FadeIN(string nextSceneName)
    {


        // タイトルロゴをオンにする
        _logoObjec.gameObject.SetActive(true);

        // 待機してタイトルロゴを横にずらす
        _logoObjec.DOLocalMoveX(-1920.0f, 0.3f).SetDelay(1.5f).OnComplete(() =>
        {
            SceneManager.LoadScene("SoloSelect");
            //LoadingSceneController.LoadSelectSceneAsync().Forget();
        });
    }

    public void FadeINMulti(Action oncomplete = null)
    {


        // タイトルロゴをオンにする
        _logoObjec.gameObject.SetActive(true);

        // 待機してタイトルロゴを横にずらす
        _logoObjec.DOLocalMoveX(-1920.0f, 0.3f).SetDelay(1.5f).OnComplete(() =>
        {
            oncomplete?.Invoke();
        });
    }
}
